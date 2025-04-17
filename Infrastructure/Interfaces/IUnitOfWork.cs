using System;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IItemRepository ItemRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
