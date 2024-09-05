﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.Logging;
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
        Task BulkDeleteAsync(List<TDTO> dtos, BulkConfig? bulkConfig = null);
        Task BulkInsert(List<TDTO> dtos, BulkConfig? bulkConfig = null);
        Task BulkUpdate(List<TDTO> dtos, BulkConfig? bulkConfig = null);
    }

    public class BaseDTOService<TDTO, T, TContext> : IBaseDTOService<TDTO, T> where T : class
    {
        protected IRepository<T, TContext> _EntityService;
        private readonly ILoggingService _loggingService;

        public IUserService EntityService { get; }

        public BaseDTOService(IRepository<T, TContext> entityService)
        {
            _EntityService = entityService;
        }

        public BaseDTOService(IUserService entityService)
        {
            EntityService = entityService;
        }

        public async Task<TDTO> FindAsync(int id)
        {
            var result = await _EntityService.FindAsync(id);
            return result == null ? default(TDTO) : result.Adapt<TDTO>();
        }

        public virtual async Task<TDTO> GetSingleBySpec(ISpecification<T> spec)
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

        public virtual async Task<List<TDTO>> GetListbySpec(ISpecification<T> spec)
        {
            var result = await _EntityService.GetListBySpec(spec);
            return result == null ? null : result.Adapt<List<TDTO>>();
        }

        public async Task<TDTO> AddAsync(TDTO dto)
        {
            try
            {
                var result = await _EntityService.AddAsync(dto.Adapt<T>());
                await HandlePostChangeEvents();
                return result == null ? default(TDTO) : result.Adapt<TDTO>();
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Add Async error: " + ex.Message + " Inner exception: " + ex.InnerException, ex.StackTrace, LogLevel.Error, LogColorCode.Red);
                return default(TDTO);
            }
        }

        public async Task UpdateAsync(TDTO dto)
        {
            await _EntityService.UpdateAsync(dto.Adapt<T>());
            await HandlePostChangeEvents();
        }

        public async Task DeleteAsync(TDTO dto)
        {
            await _EntityService.DeleteAsync(dto.Adapt<T>());
            await HandlePostChangeEvents();
        }

        public virtual async Task BulkDeleteAsync(List<TDTO> dtos, BulkConfig? bulkConfig = null)
        {
            if (dtos?.Count == 0)
                return;
            await _EntityService.BulkDeleteEntities(dtos.Adapt<List<T>>(), bulkConfig);
        }

        public async Task BulkInsert(List<TDTO> dtos, BulkConfig? bulkConfig = null)
        {
            if (dtos?.Count == 0)
                return;
            await _EntityService.BulkInsert(dtos.Adapt<List<T>>(), bulkConfig);
            await HandlePostChangeEvents();
        }

        public async Task BulkUpdate(List<TDTO> dtos, BulkConfig? bulkConfig = null)
        {
            if (dtos?.Count == 0)
                return;
            await _EntityService.BulkUpdate(dtos.Adapt<List<T>>(), bulkConfig);
            await HandlePostChangeEvents();
        }

        protected virtual async Task HandlePostChangeEvents()
        {

        }
    }
}
