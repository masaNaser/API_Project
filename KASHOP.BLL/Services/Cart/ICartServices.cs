using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Cart
{
    public interface ICartServices
    {
        Task<Result<CartItemResponse>> GetUserCart();
        Task<Result<bool>> AddToCart(CartItemRequest request);
        Task<Result<bool>> IncreaseCount(int productId);
        Task<Result<bool>> DecreaseCount(int productId);
        Task<Result<bool>> RemoveItem(int productId);
        Task<Result<bool>> Clear();
    }
}
