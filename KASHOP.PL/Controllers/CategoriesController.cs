using KASHOP.DAL.Data;
using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CategoriesController(ApplicationDbContext context, IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _localizer = localizer;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            // جلب الأقسام مع ترجماتها فوراً من قاعدة البيانات
            var categories = _context.Categories
                                     .Include(c => c.Translations)
                                     .ToList();

            var categoryDtos = categories.Adapt<List<CategoryResponse>>();

            return Ok(new
            {
                Message = _localizer["Success"].Value,
                Data = categoryDtos
            });
        }
        [HttpPost("Create")]
        public IActionResult Create(CategoryRequest request)
        {
            //if (request == null) {
            //    return BadRequest(_localizer["InvalidRequest"].Value);
            //}
            var category = request.Adapt<Category>();
            _context.Add(category);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
