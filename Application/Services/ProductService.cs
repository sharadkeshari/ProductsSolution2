using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                CreatedBy = p.CreatedBy,
                CreatedOn = p.CreatedOn,
                ModifiedOn = p.ModifiedOn,
                ModifiedBy = p.ModifiedBy ?? ""
            });
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            var product = new Product
            {
                ProductName = createDto.ProductName,
                CreatedBy = createDto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // Map the created product to ProductDto that includes the generated Id.
            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn
            };
        }

        public async Task UpdateProductAsync(int id, UpdateProductDto updateDto)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            product.ProductName = updateDto.ProductName;
            product.ModifiedBy = updateDto.ModifiedBy;
            product.ModifiedOn = DateTime.UtcNow;

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            _unitOfWork.ProductRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
