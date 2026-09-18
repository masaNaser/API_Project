using KASHOP.BLL.Services.Product;
using KASHOP.DAL.Dto.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productServices.GetAllProducts();
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest request)
        {
            var result = await _productServices.CreateProduct(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
