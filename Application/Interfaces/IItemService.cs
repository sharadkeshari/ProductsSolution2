using Application.DTO;

namespace Application.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllItemsAsync();
        Task<ItemDto> GetItemByIdAsync(int id);
        Task<ItemDto> CreateItemAsync(CreateItemDto dto);
        Task UpdateItemAsync(int id, CreateItemDto dto);
        Task DeleteItemAsync(int id);
    }
}
