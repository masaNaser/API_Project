using KASHOP.DAL.Repository.Category;
using KASHOP.DAL.Repository.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IProductRepository ProductRepository { get; } //property for product repository
        ICategoryRepository CategoryRepository { get; }
      
        Task<int> CompleteAsync();
    }
}
