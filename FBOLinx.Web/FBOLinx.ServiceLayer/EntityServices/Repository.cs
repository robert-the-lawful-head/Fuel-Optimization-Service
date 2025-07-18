﻿using EFCore.BulkExtensions;
using FBOLinx.Core.BaseModels.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using FBOLinx.Core.BaseModels.Queries;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class Repository<TEntity,TContext> : IRepository<TEntity,TContext>
    where TEntity : class where TContext : DbContext
    {
        private IDbContextTransaction dbContextTransaction;

        protected readonly TContext context;
        public DbSet<TEntity> dbSet;
        public Repository(TContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
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
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public IQueryable<TEntity> Get() => context.Set<TEntity>().AsQueryable();

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

        public async Task<List<TEntity>> GetAsync(QueryableOptions<TEntity> queryableOptions)
        {
            var query = context.Set<TEntity>().AsQueryable();
            if (queryableOptions.Predicate != null)
                query = query.Where(queryableOptions.Predicate);
            if (queryableOptions.MaxRecords.GetValueOrDefault() > 0)
                query = query.Take(queryableOptions.MaxRecords.GetValueOrDefault());
            if (queryableOptions.OrderByExpression != null)
                query = query.OrderBy(queryableOptions.OrderByExpression);
            if (queryableOptions.OrderByDescendingExpression != null)
                query = query.OrderByDescending(queryableOptions.OrderByDescendingExpression);
            return await query.ToListAsync();
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
        public virtual IQueryable<TEntity> GetListBySpecAsQueryable(ISpecification<TEntity> spec)
        {
            return GetEntityListQueryable(spec);
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
            var executionStrategy = context.Database.CreateExecutionStrategy();
            await executionStrategy.Execute(async () =>
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (bulkConfig == null)
                        {
                            bulkConfig = new BulkConfig();
                            bulkConfig.SetOutputIdentity = true;
                        }
                        await context.BulkDeleteAsync(entities, bulkConfig);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Something went wrong processing delete bulk operation", ex);
                    }
                }
            });
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
            var executionStrategy = context.Database.CreateExecutionStrategy();
            await executionStrategy.Execute(async () =>
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (bulkConfig == null)
                        {
                            bulkConfig = new BulkConfig();
                            bulkConfig.BatchSize = 500;
                            bulkConfig.SetOutputIdentity = false;
                            bulkConfig.BulkCopyTimeout = 0;
                            bulkConfig.WithHoldlock = false;
                        }

                        await context.BulkInsertAsync(entities, bulkConfig);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Something went wrong processing insert bulk operation", ex);
                    }
                }
            });
        }

        public async Task BulkUpdate(List<TEntity> entities, BulkConfig? bulkConfig = null)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            await executionStrategy.Execute(async () =>
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (bulkConfig == null)
                        {
                            bulkConfig = new BulkConfig();
                            bulkConfig.BatchSize = 500;
                            bulkConfig.SetOutputIdentity = false;
                            bulkConfig.BulkCopyTimeout = 0;
                            bulkConfig.WithHoldlock = false;
                        }
                        await context.BulkUpdateAsync(entities);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Something went wrong processing update bulk operation", ex);
                    }
                }
            });
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            IExecutionStrategy executionStrategy = context.Database.CreateExecutionStrategy();

            return executionStrategy;
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

        public async Task DeleteRangeAsync(List<TEntity> entityList)
        {
            context.Set<TEntity>().RemoveRange(entityList);
            await context.SaveChangesAsync();
        }
    }
}