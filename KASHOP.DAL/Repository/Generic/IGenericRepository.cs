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
        Task<T> Create(T entity);
        //Task<T> Update(T entity);
        Task Delete(int id);
        Task<T> GetOne(Expression<Func<T, bool>> filter, string[]? includes = null);
        Task<List<T>> GetAll(string[]?includes = null);
    }
}
