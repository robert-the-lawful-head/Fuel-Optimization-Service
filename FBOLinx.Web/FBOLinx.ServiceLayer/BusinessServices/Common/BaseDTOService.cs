using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.Mapping;
using Mapster;
using NetTopologySuite.IO;

namespace FBOLinx.ServiceLayer.BusinessServices.Common
{
    public interface IBaseDTOService<TDTO, T>
    {
        Task<TDTO> FindAsync(int id);
        Task<TDTO> GetSingleBySpec(ISpecification<T> spec);
        Task<List<TDTO>> GetListbySpec(ISpecification<T> spec);
        Task<TDTO> AddAsync(TDTO dto);
        Task UpdateAsync(TDTO dto);
        Task DeleteAsync(TDTO dto);
        Task BulkDeleteAsync(List<TDTO> dtos);
        Task BulkInsert(List<TDTO> dtos);
        Task BulkUpdate(List<TDTO> dtos);
    }

    public class BaseDTOService<TDTO, T, TContext> : IBaseDTOService<TDTO, T>
    {
        protected IRepository<T, TContext> _EntityService;

        public BaseDTOService(IRepository<T, TContext> entityService)
        {
            _EntityService = entityService;
        }

        public async Task<TDTO> FindAsync(int id)
        {
            var result = await _EntityService.FindAsync(id);
            return result == null ? default(TDTO) : result.Adapt<TDTO>();
        }

        public async Task<TDTO> GetSingleBySpec(ISpecification<T> spec)
        {
            var result = await _EntityService.GetSingleBySpec(spec);
            try
            {
                return result == null ? default(TDTO) : result.Adapt<TDTO>();
            }
            catch (Exception ex)
            {
                return default(TDTO);
            }
            
        }

        public async Task<List<TDTO>> GetListbySpec(ISpecification<T> spec)
        {
            var result = await _EntityService.GetListBySpec(spec);
            return result == null ? null : result.Adapt<List<TDTO>>();
        }

        public async Task<TDTO> AddAsync(TDTO dto)
        {
            var result = await _EntityService.AddAsync(dto.Adapt<T>());
            return result == null ? default(TDTO) : result.Adapt<TDTO>();
        }

        public async Task UpdateAsync(TDTO dto)
        {
            await _EntityService.UpdateAsync(dto.Adapt<T>());
        }

        public async Task DeleteAsync(TDTO dto)
        {
            await _EntityService.DeleteAsync(dto.Adapt<T>());
        }

        public async Task BulkDeleteAsync(List<TDTO> dtos)
        {
            await _EntityService.BulkDeleteEntities(dtos.Adapt<List<T>>());
        }

        public async Task BulkInsert(List<TDTO> dtos)
        {
            await _EntityService.BulkInsert(dtos.Adapt<List<T>>());
        }

        public async Task BulkUpdate(List<TDTO> dtos)
        {
            await _EntityService.BulkUpdate(dtos.Adapt<List<T>>());
        }
    }
}
