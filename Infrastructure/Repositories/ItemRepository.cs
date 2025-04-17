using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Here, include the navigation property if needed.
        public async Task<Item> GetByIdAsync(int id)
        {
            return await _context.Items
                .AsNoTracking()
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.Items
                .AsNoTracking()
                .Include(i => i.Product)
                .ToListAsync();
        }

        public async Task AddAsync(Item item)
        {
            await _context.Items.AddAsync(item);
        }

        public void Update(Item item)
        {
            _context.Items.Update(item);
        }

        public void Remove(Item item)
        {
            _context.Items.Remove(item);
        }
    }
}
