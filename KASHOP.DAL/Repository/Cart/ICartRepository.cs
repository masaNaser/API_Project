using KASHOP.DAL.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository.Cart
{
    public interface ICartRepository : IGenericRepository<Models.CartItem>
    {
        Task ClearAsync(string userId);

    }
}
