using KASHOP.BLL.Services.Category;
using KASHOP.DAL.Data;
using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICategoryServices _categoryServices;
        public CategoriesController( IStringLocalizer<SharedResources> localizer,ICategoryServices categoryServices)
        {
            _localizer = localizer;
            _categoryServices = categoryServices;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {       
            //لو بدي اعتمد هاي الطريقة رح احتاج ارسل هاي مع كل ريكوست 
            //var lang = Request.Headers["Accept-Language"].ToString();

            var categories =await _categoryServices.GetAllCategories();
            // جلب الأقسام مع ترجماتها فوراً من قاعدة البيانات
            //var categories = _context.Categories
            //                         .Include(c => c.Translations)
            //                         .ToList();

            //var categoryDtos = categories.Adapt<List<CategoryResponse>>();
            return Ok(new
            {
                Message = _localizer["Success"].Value,
                Data = categories
            });
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            //var category = request.Adapt<Category>();
            //_context.Add(category);
            //_context.SaveChanges();
           var response =await _categoryServices.Create(request);
            return Ok(new {Data = response});
        }
      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryServices.GetCategory(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(new { Data = category });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]  int id)
        {
            var result = await _categoryServices.Delete(id);
            if (!result)
            {
                return NotFound(new { Message = _localizer["Error"].Value });
            }
            return NoContent();
        }

    }
}
