using KASHOP.DAL.Data;
using KASHOP.DAL.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository.Category
{
    public class CategoryRepository : GenericRepository<Models.Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
       public CategoryRepository(ApplicationDbContext context ) : base(context)
        {
            _context = context;
        }
    }
}
