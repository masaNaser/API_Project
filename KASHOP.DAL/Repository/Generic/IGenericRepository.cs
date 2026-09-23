using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task <bool> DeleteAsync(T entity);
        Task<T> GetOneAsync(Expression<Func<T, bool>> filter, string[]? includes = null);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,string[]?includes = null);
    }
}
