using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Cart
{
    public class CartServices : ICartServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartServices(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        // دالة مساعدة خاصة لجلب الـ UserId من الـ Token بأمان
        private string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public async Task<Result<CartItemResponse>> GetUserCart()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Result<CartItemResponse>.FailureResult("User not authenticated");
            }
           var items= await _unitOfWork.CartRepository.GetOneAsync(
                c => c.UserId == userId,
                  new string[]
                {
                    nameof(DAL.Models.CartItem.Product),
                    $"{nameof(DAL.Models.CartItem.Product)}.{nameof(DAL.Models.Product.Translations)}"
                }
                );
            return Result<CartItemResponse>.SuccessResult(items.Adapt<CartItemResponse>(), "Success");
        }
        public async Task<Result<bool>> AddToCart(CartItemRequest request)
        {
            var userId = GetCurrentUserId();
            var product =await _unitOfWork.ProductRepository.GetOneAsync(p => p.Id == request.ProductId);
            if (product == null) {
                return Result<bool>.FailureResult("Product not found");
            }
            if(request.Count <= 0)
            {
                return Result<bool>.FailureResult("Count must be greater than zero");
            }
            if (product.Quantity< request.Count)
            {
                return Result<bool>.FailureResult("Not enough stock available");
            }
            var existingCartItem = await _unitOfWork.CartRepository.GetOneAsync(c => c.UserId == userId && c.ProductId == request.ProductId);
            if(existingCartItem is not null){
                existingCartItem.Count += request.Count;
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Count = request.Count
                };
                await _unitOfWork.CartRepository.CreateAsync(cartItem);
            }
            await _unitOfWork.CompleteAsync();
            return Result<bool>.SuccessResult(true, "Item added to cart successfully.");
        }
        public async Task<Result<bool>> DecreaseCount(int productId)
        {
            var userId = GetCurrentUserId();
            var cartItem = await _unitOfWork.CartRepository.GetOneAsync(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem != null) {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    await _unitOfWork.CompleteAsync();
                    return Result<bool>.SuccessResult(true, "Item count decreased successfully.");
                }
                else
                {
                    var deleted = _unitOfWork.CartRepository.DeleteAsync(cartItem);
                    await _unitOfWork.CompleteAsync();
                    return Result<bool>.SuccessResult(deleted, deleted ? "Item deleted successfully." : "Failed to delete Item.");
                }
            }
            return Result<bool>.FailureResult("Item not found in cart");

        }

        public async Task<Result<bool>> IncreaseCount(int productId)
        {
            var userId = GetCurrentUserId();
            var cartItem = await _unitOfWork.CartRepository.GetOneAsync(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem != null)
            {
                    cartItem.Count++;
                    await _unitOfWork.CompleteAsync();
                    return Result<bool>.SuccessResult(true, "Item count increased successfully.");
            }
            return Result<bool>.FailureResult("Item not found in cart");
        }

        public async Task<Result<bool>> RemoveItem(int productId)
        {
            var userId = GetCurrentUserId();
            var cartItem =await _unitOfWork.CartRepository.GetOneAsync(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem != null) {
                var deleted = _unitOfWork.CartRepository.DeleteAsync(cartItem);
                await _unitOfWork.CompleteAsync();
                return Result<bool>.SuccessResult(deleted, deleted ? "Item deleted successfully." : "Failed to delete Item.");
            }

            return Result<bool>.FailureResult("Item not found in cart");
        }

        public async Task<Result<bool>> Clear()
        {
            var userId = GetCurrentUserId();
            var cartItems = await _unitOfWork.CartRepository.GetAllAsync(c => c.UserId == userId);
            if (cartItems != null)
            {
                await _unitOfWork.CartRepository.ClearAsync(userId);
                await _unitOfWork.CompleteAsync();
                return Result<bool>.SuccessResult(true, "Cart cleared successfully");
            }
            return Result<bool>.FailureResult("Cart is already empty");
        }

    }
}
