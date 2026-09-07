using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using System.Linq.Expressions;

namespace KASHOP.BLL.Services.Category
{
    public interface ICategoryServices
    {
         Task<List<CategoryResponse>> GetAllCategories();
         Task<CategoryResponse> GetCategory(Expression<Func<DAL.Models.Category,bool>>filter);
        Task<List<CategoryResponse>> Create(CategoryRequest request);
        Task<CategoryResponse> Update(int id, CategoryRequest request);
        Task<bool> Delete(int id);

    }
}
