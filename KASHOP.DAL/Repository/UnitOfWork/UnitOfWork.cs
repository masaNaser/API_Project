using KASHOP.DAL.Data;
using KASHOP.DAL.Repository.Category;
using KASHOP.DAL.Repository.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        //طبقنا موضوع ال
        //lazy loading
        //عشان لما نحتاج ال
        //repository
        //نعمله
        //instance 
        //مرة واحدة مش كل مرة نعمل اوبجكت جديد
        private IProductRepository? _ProductRepository;
        private ICategoryRepository? _CategoryRepository;
        public IProductRepository ProductRepository { get
            {
                if (_ProductRepository == null)
                {
                    _ProductRepository = new ProductRepository(_context);
                }
                return _ProductRepository;
            } }
        public ICategoryRepository CategoryRepository { get
            {
                if (_CategoryRepository == null)
                {
                    _CategoryRepository = new CategoryRepository(_context);
                }
                return _CategoryRepository;
            } }

        public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
        }


        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> CompleteAsync()
        {
          return await _context.SaveChangesAsync();
        }
    }
}
