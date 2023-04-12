using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class BaseEntityService<T, TDTO, TIDType> where T : FBOLinxBaseEntityModel<TIDType> where TDTO : IEntityModelDTO<T, TIDType>
    {
        protected DbContext _Context;

        protected BaseEntityService(DbContext context)
        {
            _Context = context;
        }

        public virtual async Task<TDTO> GetById(TIDType id)
        {
            var entity = await GetEntityById(id);

            return entity.Adapt<TDTO>();

        }

        protected virtual async Task<T> GetEntityById(TIDType id)
        {
            var result = await _Context.Set<T>().FindAsync(id);
            _Context.Entry(result).State = EntityState.Detached;
            return result;
        }

        public virtual async Task<TDTO> GetSingleBySpec(ISpecification<T> spec)
        {
            var entity = await GetEntitySingleBySpec(spec);

            return entity.Adapt<TDTO>();
        }

        protected async Task<T> GetEntitySingleBySpec(ISpecification<T> spec)
        {
            var result = await GetEntityListBySpec(spec);
            return result.FirstOrDefault();
        }

        public async Task<List<TDTO>> GetListBySpec(ISpecification<T> spec)
        {
            var result = await GetEntityListBySpec(spec);

            return result.Adapt<List<TDTO>>();

        }

        protected async Task<List<T>> GetEntityListBySpec(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_Context.Set<T>().AsQueryable(),
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

        public virtual async Task<TDTO> Add(TDTO entityDTO)
        {
            var entity = await AddEntity(entityDTO.Adapt<T>());
            return entity.Adapt<TDTO>();

        }

        protected async Task<T> AddEntity(T entity)
        {
            _Context.Set<T>().Add(entity);
            await _Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TDTO> Update(TDTO entityDTO)
        {
            var entity = entityDTO.Adapt<T>();
            if (EqualityComparer<TIDType>.Default.Equals(entity.Oid, default(TIDType)))
                await AddEntity(entity);
            else
            {
                await UpdateEntity(entity);
            }
            return entity.Adapt<TDTO>();
        }

        protected async Task UpdateEntity(T entity)
        {
            try
            {
                _Context.Set<T>().Attach(entity);
            }
            catch (System.Exception exception)
            {
                //Do nothing... the entity was already attached
            }
            _Context.Entry(entity).State = EntityState.Modified;
            await _Context.SaveChangesAsync();
        }

        public async Task Delete(TDTO entityDTO)
        {
            await Delete(entityDTO.Oid);
        }

        public async Task Delete(TIDType id)
        {
            var entity = await GetEntityById(id);
            if (entity == null)
                return;
            await DeleteEntity(entity);
        }

        protected async Task DeleteEntity(T entity)
        {
            try
            {
                _Context.Set<T>().Remove(entity);
                await _Context.SaveChangesAsync();
            }
            catch (System.Exception exception)
            {
                //Do nothing... the entity was already deleted
            }
        }

        public async Task BulkDelete(List<TDTO> entityDTOs)
        {
            if (entityDTOs == null)
                return;

            var entities = entityDTOs.Select(x => x.Adapt<T>()).ToList();
            await BulkDeleteEntities(entities);
        }

        public async Task BulkDeleteEntities(List<T> entities)
        {
            try
            {
                await _Context.BulkDeleteAsync(entities);
                await _Context.SaveChangesAsync();
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
                await _Context.Set<T>().Where(spec.Criteria).BatchDeleteAsync();
                await _Context.SaveChangesAsync();
            }
            catch (System.Exception exception)
            {
                //Do nothing... the entities were already deleted
            }
        }

        public async Task BulkInsertOrUpdate(List<TDTO> entityDTOs)
        {
            var entities = entityDTOs.Select(x => x.Adapt<T>()).ToList();
            await BulkInsertOrUpdateEntities(entities);
            for (int entityIndex = 0; entityIndex < entities.Count; entityIndex++)
            {
                try
                {
                    entityDTOs[entityIndex].Oid = entities[entityIndex].Oid;
                }
                catch (System.Exception exception)
                {
                    //Do nothing - somehow more entities than DTOs?
                }
            }
        }

        protected virtual async Task BulkInsertOrUpdateEntities(List<T> entities)
        {
            await using var transaction = await _Context.Database.BeginTransactionAsync();
            await _Context.BulkInsertOrUpdateAsync(entities, config => {
                config.BatchSize = 500;
                config.SetOutputIdentity = false;
                config.BulkCopyTimeout = 0;
                config.WithHoldlock = false;
        });
            await transaction.CommitAsync();
        }

        public IQueryable<T> GetEntitiesAsQueryable()
        {
            return _Context.Set<T>().AsQueryable();
        }
    }
}
