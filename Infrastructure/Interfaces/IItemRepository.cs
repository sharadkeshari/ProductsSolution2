using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IItemRepository
    {
        Task<Item> GetByIdAsync(int id);
        Task<IEnumerable<Item>> GetAllAsync();
        Task AddAsync(Item item);
        void Update(Item item);
        void Remove(Item item);
    }
}
