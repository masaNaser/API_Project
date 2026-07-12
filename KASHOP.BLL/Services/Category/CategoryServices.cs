using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository.Category;
using Mapster;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryResponse>> Create(CategoryRequest request)
        {
            var categories = request.Adapt<DAL.Models.Category>();
           var savedCategory = await _categoryRepository.Create(categories);
            var categoryResponse = savedCategory.Adapt<CategoryResponse>();
            return new List<CategoryResponse> { categoryResponse };
        }

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            var categories =await _categoryRepository.GetAll(new string[]
            {
                (nameof(DAL.Models.Category.Translations))
            }); // category
            //لازم نحوله ل ريسبونس
            return categories.Adapt<List<CategoryResponse>>();
        }

        public async Task<CategoryResponse> GetCategory(Expression<Func<DAL.Models.Category, bool>> filter)
        {
            var category =await _categoryRepository.GetOne(filter,new string[]
            {
                (nameof(DAL.Models.Category.Translations))
            });
            return category.Adapt<CategoryResponse>();

        }
    }
}
