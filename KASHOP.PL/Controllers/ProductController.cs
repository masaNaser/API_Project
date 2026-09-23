using KASHOP.BLL.Services.Product;
using KASHOP.DAL.Dto.Request;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest request)
        {
            var result = await _productServices.CreateProduct(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productServices.GetProduct(c => c.Id == id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpGet("GetByCategory/{categoryId}")]
        public async Task<IActionResult> GetProductByCategoryId(int categoryId)
        {
            var result = await _productServices.GetAllProducts(p=>p.CategoryId== categoryId);
            return result.Success ? Ok(result) : NotFound(result);
        }
        //public async Task<IActionResult> GetProductByBrandId(int brandId)
        //{
        //    var result = await _productServices.GetProduct(p=>p.);
        //    return result.Success ? Ok(result) : NotFound(result);
        //}

    }
}
