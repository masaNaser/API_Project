using KASHOP.BLL.Services.Product;
using KASHOP.DAL.Dto.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductControllers : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductControllers(IProductServices productServices)
        {
            _productServices = productServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest request)
        {
            var result = await _productServices.CreateProduct(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
