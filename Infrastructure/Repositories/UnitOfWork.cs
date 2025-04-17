using Infrastructure.Data;
using Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IProductRepository _productRepository;
        private IItemRepository _itemRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository =>
            _productRepository ??= new ProductRepository(_context);

        public IItemRepository ItemRepository =>
            _itemRepository ??= new ItemRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
