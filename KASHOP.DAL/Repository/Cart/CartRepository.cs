using KASHOP.DAL.Data;
using KASHOP.DAL.Repository.Cart;
using KASHOP.DAL.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository.Category
{
    public class CartRepository : GenericRepository<Models.CartItem>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task ClearAsync(string userId)
        {

            throw new NotImplementedException();
        }
    }
}

