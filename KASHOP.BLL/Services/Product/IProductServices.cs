using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Product
{
    public interface IProductServices
    {
        Task<Result<List<ProductListResponse>>> GetAllProducts(Expression<Func<DAL.Models.Product, bool>>? filter = null);
        Task<Result<bool>> CreateProduct(ProductRequest request);
        Task<Result<ProductListResponse>> Update(int id, ProductRequest request);
        Task<Result<bool>> Delete(int id);
        Task<Result<ProductDetailsResponse>> GetProduct(Expression<Func<DAL.Models.Product,bool>>filter);        
    }
}
