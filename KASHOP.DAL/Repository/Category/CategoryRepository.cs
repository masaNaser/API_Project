using KASHOP.DAL.Data;
using KASHOP.DAL.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace KASHOP.DAL.Repository.Category
{
    public class CategoryRepository : GenericRepository<Models.Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task ClearAsync(string userId)
        {
            var items = await _context.Set<Models.CartItem>()
                                      .Where(c => c.UserId == userId)
                                      .ToListAsync();
            _context.RemoveRange(items);
        }
    }
}

