using FBOLinx.Core.BaseModels.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IRepository<T,TContext>
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> DeleteAsync(int id);
        Task<T> GetSingleBySpec(ISpecification<T> spec);
        Task<List<T>> GetListBySpec(ISpecification<T> spec);
        Task BulkDeleteEntities(List<T> entities);
        Task BulkDeleteEntities(ISpecification<T> spec);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(int id);
        Task BulkInsert(List<T> entities, bool includeGraph = false);
        Task BulkUpdate(List<T> entities);
    }
}