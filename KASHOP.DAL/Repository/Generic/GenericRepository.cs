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
        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAllAsync(string[]? includes = null)
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

        public async Task<T> GetOneAsync(Expression<Func<T,bool>>filter,string[]? includes = null)
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

        public async Task<T> UpdateAsync(T entity)
        {
             _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
