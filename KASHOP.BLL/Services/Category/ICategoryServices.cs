using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using System.Linq.Expressions;

namespace KASHOP.BLL.Services.Category
{
    public interface ICategoryServices
    {
         Task<Result<List<CategoryResponse>>> GetAllCategories();
         Task<Result<CategoryResponse>> GetCategory(Expression<Func<DAL.Models.Category,bool>>filter);
         Task<Result<bool>> Create(CategoryRequest request);
         Task<Result<CategoryResponse>> Update(int id, CategoryRequest request);
         Task<Result<bool>> Delete(int id);

    }
}
