using KASHOP.DAL.Data;
using KASHOP.DAL.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository.Product
{
    public class ProductRepository : GenericRepository<Models.Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context;
       public ProductRepository(ApplicationDbContext context ) : base(context)
        {
            _context = context;
        }
    }
}
