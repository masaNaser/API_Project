using KASHOP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace KASHOP.DAL.Repository.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    { 
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<T> Create(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<List<T>> GetAll(string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if(includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetOne(Expression<Func<T,bool>>filter,string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if(includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(filter);
        }
    }
}
