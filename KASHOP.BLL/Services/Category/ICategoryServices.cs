using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace KASHOP.BLL.Services.Category
{
    public interface ICategoryServices
    {
         Task<List<CategoryResponse>> GetAllCategories();
         Task<CategoryResponse> GetCategory(Expression<Func<DAL.Models.Category,bool>>filter);
        Task<List<CategoryResponse>> Create(CategoryRequest request);

    }
}
