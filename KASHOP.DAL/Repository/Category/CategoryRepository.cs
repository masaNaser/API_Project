using KASHOP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        async Task<Models.Category> ICategoryRepository.Create(Models.Category category)
        {
           await _context.Categories.AddAsync(category);
           await _context.SaveChangesAsync();
           return category;     
        }

        async Task<List<Models.Category>> ICategoryRepository.GetAll()
        {
            var categories =await _context.Categories.Include(c => c.Translations).ToListAsync();
            return categories;
        }
    }
}
