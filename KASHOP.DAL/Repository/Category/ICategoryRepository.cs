using KASHOP.DAL.Models;
using KASHOP.DAL.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository.Category
{
    public interface ICategoryRepository:IGenericRepository<Models.Category>
    {
        //public Task<List<Models.Category>> GetAll();
        //public Task<Models.Category> Create(Models.Category category);
    }
}
