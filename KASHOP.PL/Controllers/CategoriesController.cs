using KASHOP.BLL.Services.Category;
using KASHOP.DAL.Data;
using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

            var result =await _categoryServices.GetAllCategories();
            return result.Success ? Ok(result): BadRequest(result);
            // جلب الأقسام مع ترجماتها فوراً من قاعدة البيانات
            //var categories = _context.Categories
            //                         .Include(c => c.Translations)
            //                         .ToList();

            //var categoryDtos = categories.Adapt<List<CategoryResponse>>();
            //return Ok(new
            //{
            //    Message = _localizer["Success"].Value,
            //    Data = categories
            //});


        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // يعرض الخطأ بدقة إذا كانت المشكلة بالـ Validation
            }
            //var category = request.Adapt<Category>();
            //_context.Add(category);
            //_context.SaveChanges();
            var result =await _categoryServices.Create(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryServices.GetCategory(c => c.Id == id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CategoryRequest request)
        {
            var result = await _categoryServices.Update(id, request);
           
            return result.Success ? Ok(result) : NotFound(result);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]  int id)
        {
            var result = await _categoryServices.Delete(id);
            return result.Success ? Ok(result) : NotFound();
        }

    }
}
