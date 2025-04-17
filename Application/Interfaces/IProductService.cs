using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
        Task UpdateProductAsync(int id, CreateProductDto updateDto);
        Task DeleteProductAsync(int id);
    }
}
