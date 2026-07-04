using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Category
{
    public interface ICategoryServices
    {
         Task<List<CategoryResponse>> GetAllCategories();
         Task<List<CategoryResponse>> Create(CategoryRequest request);

    }
}
