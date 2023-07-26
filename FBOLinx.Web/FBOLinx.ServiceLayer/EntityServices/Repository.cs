using EFCore.BulkExtensions;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class Repository<TEntity,TContext> : IRepository<TEntity,TContext>
    where TEntity : class where TContext : DbContext
    {
        private IDbContextTransaction dbContextTransaction;

        protected readonly TContext context;
        public Repository(TContext context)
        {
            this.context = context;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entity)
        {
            context.Set<TEntity>().AddRangeAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteAsync(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<TEntity> DeleteAsync(int id)
        {
            var entity = context.Set<TEntity>().Find(id);
            if (entity == null)
            {
                return null;
            }
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<TEntity> GetAsync(int id) => await context.Set<TEntity>().FindAsync(id);
        
        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                context.Set<TEntity>().Attach(entity);
            }
            catch (System.Exception exception)
            {
                //Do nothing... the entity was already attached
            }
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = context.Set<TEntity>().AsQueryable();
            return await query.Where(predicate).ToListAsync();
        }
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        => await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        => context.Set<TEntity>().Where(predicate);
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        => await context.Set<TEntity>().AnyAsync(predicate);
        public async Task<TEntity> FindAsync(int id)
        => await context.Set<TEntity>().FindAsync(id);
        public virtual async Task<TEntity> GetSingleBySpec(ISpecification<TEntity> spec) => await GetEntitySingleBySpec(spec);
        
        public virtual async Task<List<TEntity>> GetListBySpec(ISpecification<TEntity> spec)
        {
            var queryable = GetEntityListQueryable(spec);
            return await queryable.ToListAsync();
        }

        public async Task<List<TProjection>> GetListBySpec<TProjection>(ISpecification<TEntity, TProjection> spec)
        {
            var queryable = GetEntityListQueryable((Specification<TEntity>)spec);
            return await queryable.Select(spec.Projection).ToListAsync();
        }

        protected async Task<TEntity> GetEntitySingleBySpec(ISpecification<TEntity> spec)
        {
            var queryable = GetEntityListQueryable(spec);
            return await queryable.FirstOrDefaultAsync();
        }

        protected IQueryable<TEntity> GetEntityListQueryable(ISpecification<TEntity> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            IQueryable<TEntity> queryableResult = spec.Includes
                .Aggregate(context.Set<TEntity>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            queryableResult = spec.IncludeStrings
                .Aggregate(queryableResult,
                    (current, include) => current.Include(include));

            queryableResult = queryableResult.AsNoTracking().Where(spec.Criteria);

            return queryableResult;
        }
        
        public async Task BulkDeleteEntities(List<TEntity> entities, BulkConfig? bulkConfig = null)
        {
            try
            {
                if (bulkConfig == null)
                {
                    bulkConfig = new BulkConfig();
                    bulkConfig.SetOutputIdentity = true;
                }
                await using var transaction = await context.Database.BeginTransactionAsync();
                await context.BulkDeleteAsync(entities, bulkConfig);
                await transaction.CommitAsync();

            }
            catch (System.Exception exception)
            {
                //Do nothing... the entities were already deleted
            }
        }
        public async Task BulkDeleteEntities(ISpecification<TEntity> spec, BulkConfig? bulkConfig = null)
        {
            try
            {
                var entities = await GetListBySpec(spec);
                await BulkDeleteEntities(entities, bulkConfig);
            }
            catch (System.Exception exception)
            {
                //Do nothing... the entities were already deleted
            }
        }

        public async Task BulkInsert(List<TEntity> entities, BulkConfig? bulkConfig = null)
        {
            if (bulkConfig == null)
            {
                bulkConfig = new BulkConfig();
                bulkConfig.BatchSize = 500;
                bulkConfig.SetOutputIdentity = false;
                bulkConfig.BulkCopyTimeout = 0;
                bulkConfig.WithHoldlock = false;
            }

            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.BulkInsertAsync(entities, bulkConfig);
            await transaction.CommitAsync();
        }

        public async Task BulkUpdate(List<TEntity> entities, BulkConfig? bulkConfig = null)
        {
            if (bulkConfig == null)
            {
                bulkConfig = new BulkConfig();
                bulkConfig.BatchSize = 500;
                bulkConfig.SetOutputIdentity = false;
                bulkConfig.BulkCopyTimeout = 0;
                bulkConfig.WithHoldlock = false;
            }
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.BulkUpdateAsync(entities);
            await transaction.CommitAsync();
        }

        public async Task BeginDbTransaction()
        {
            dbContextTransaction = await context.Database.BeginTransactionAsync();
        }

        public async Task RollbackDbTransaction()
        {
            if (dbContextTransaction == null)
            {
                return;
            }

            await dbContextTransaction.RollbackAsync();
        }
    }
}