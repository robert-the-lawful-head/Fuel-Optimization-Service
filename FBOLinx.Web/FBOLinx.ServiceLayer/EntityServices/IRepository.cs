using FBOLinx.Core.BaseModels.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IRepository<TEntity, TContext>
    {
        IQueryable<TEntity> Get();
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> AddRangeAsync(List<TEntity> entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> DeleteAsync(int id);
        Task<TEntity> GetSingleBySpec(ISpecification<TEntity> spec);
        Task<List<TEntity>> GetListBySpec(ISpecification<TEntity> spec);
        Task<List<TProjection>> GetListBySpec<TProjection>(ISpecification<TEntity, TProjection> spec);
        Task BulkDeleteEntities(List<TEntity> entities, BulkConfig? bulkConfig = null);
        Task BulkDeleteEntities(ISpecification<TEntity> spec, BulkConfig? bulkConfig = null);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(int id);
        Task BulkInsert(List<TEntity> entities, BulkConfig? bulkConfig = null);
        Task BulkUpdate(List<TEntity> entities, BulkConfig? bulkConfig = null);
    }
}