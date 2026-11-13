using KASHOP.BLL.Services.Cart;
using KASHOP.DAL.Data;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly ICartServices _cartServices;

        public CartsController(IStringLocalizer<SharedResources> stringLocalizer,ICartServices cartServices)
        {
            _stringLocalizer = stringLocalizer;
            _cartServices = cartServices;
        }

        [HttpGet("GetUserCart")]
        public async Task<IActionResult> GetUserCart()
        {
            var result = await _cartServices.GetUserCart();
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] DAL.Dto.Request.CartItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _cartServices.AddToCart(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("IncreaseCount/{productId}")]
        public async Task<IActionResult> IncreaseCount(int productId)
        {
            var result = await _cartServices.IncreaseCount(productId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("DecreaseCount/{productId}")]
        public async Task<IActionResult> DecreaseCount(int productId)
        {
            var result = await _cartServices.DecreaseCount(productId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("RemoveItem/{productId}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var result = await _cartServices.RemoveItem(productId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("Clear")]
        public async Task<IActionResult> Clear()
        {
            var result = await _cartServices.Clear();
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
