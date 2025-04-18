using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ItemDto>> GetAllItemsAsync()
        {
            var items = await _unitOfWork.ItemRepository.GetAllAsync();
            return items.Select(i => new ItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                ProductName = i.Product != null ? i.Product.ProductName : string.Empty
            });
        }

        public async Task<ItemDto> GetItemByIdAsync(int id)
        {
            var item = await _unitOfWork.ItemRepository.GetByIdAsync(id);
            if (item == null)
                return null;

            return new ItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                ProductName = item.Product != null ? item.Product.ProductName : string.Empty,
            };
        }

        public async Task<ItemDto> CreateItemAsync(CreateItemDto dto)
        {
            // Ensure the specified product exists.
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new Exception("Related product not found.");

            // Create a new Item. The foreign key (ProductId) is sufficient; no need to pass the Product itself.
            var item = new Item
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await _unitOfWork.ItemRepository.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return new ItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                ProductName = item.Product != null ? item.Product.ProductName : string.Empty
            };
        }

        public async Task UpdateItemAsync(int id, CreateItemDto dto)
        {
            var item = await _unitOfWork.ItemRepository.GetByIdAsync(id);
            if (item == null)
                throw new Exception("Item not found.");

            // If the ProductId is changing, check the existence of the new product.
            if (item.ProductId != dto.ProductId)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(dto.ProductId);
                if (product == null)
                    throw new Exception("Related product not found.");
            }

            item.ProductId = dto.ProductId;
            item.Quantity = dto.Quantity;
            // Update other fields as necessary.

            _unitOfWork.ItemRepository.Update(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _unitOfWork.ItemRepository.GetByIdAsync(id);
            if (item == null)
                throw new Exception("Item not found.");

            _unitOfWork.ItemRepository.Remove(item);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
