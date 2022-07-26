using EFCore.BulkExtensions;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class Repository<T,TContext> : IRepository<T,TContext>
    where T : class where TContext : DbContext
    {
        private readonly TContext context;
        public Repository(TContext context)
        {
            this.context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            context.Set<T>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity = context.Set<T>().Find(id);
            if (entity == null)
            {
                return null;
            }
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> GetAsync(int id) => await context.Set<T>().FindAsync(id);
        
        public async Task UpdateAsync(T entity)
        {
            try
            {
                context.Set<T>().Attach(entity);
            }
            catch (System.Exception exception)
            {
                //Do nothing... the entity was already attached
            }
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var query = context.Set<T>().AsQueryable();
            return await query.Where(predicate).ToListAsync();
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        => await context.Set<T>().FirstOrDefaultAsync(predicate);
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        => context.Set<T>().Where(predicate);
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await context.Set<T>().AnyAsync(predicate);
        public async Task<T> FindAsync(int id)
        => await context.Set<T>().FindAsync(id);
        public virtual async Task<T> GetSingleBySpec(ISpecification<T> spec) => await GetEntitySingleBySpec(spec);

        protected async Task<T> GetEntitySingleBySpec(ISpecification<T> spec)
        {
            var result = await GetEntityListBySpec(spec);
            return result.FirstOrDefault();
        }

        public async Task<List<T>> GetListBySpec(ISpecification<T> spec) => await GetEntityListBySpec(spec);

        protected async Task<List<T>> GetEntityListBySpec(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(context.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult.AsNoTracking()
                .Where(spec.Criteria)
                .ToListAsync();
        }
        public async Task BulkDeleteEntities(List<T> entities)
        {
            try
            {
                await context.BulkDeleteAsync(entities);
                await context.SaveChangesAsync();
            }
            catch (System.Exception exception)
            {
                //Do nothing... the entities were already deleted
            }
        }
        public async Task BulkDeleteEntities(ISpecification<T> spec)
        {
            try
            {
                await context.Set<T>().Where(spec.Criteria).BatchDeleteAsync();
                await context.SaveChangesAsync();
            }
            catch (System.Exception exception)
            {
                //Do nothing... the entities were already deleted
            }
        }

        public async Task BulkInsert(List<T> entities, bool includeGraph = false)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            BulkConfig bulkConfig = new BulkConfig();
            bulkConfig.SetOutputIdentity = true;
            bulkConfig.IncludeGraph = includeGraph;
            await context.BulkInsertAsync(entities, bulkConfig);
            await transaction.CommitAsync();
        }

        public async Task BulkUpdate(List<T> entities)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.BulkUpdateAsync(entities);
            await transaction.CommitAsync();
        }
    }
}