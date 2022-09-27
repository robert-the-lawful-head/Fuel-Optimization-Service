using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.Aircraft;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.EntityServices;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{
    public interface IAircraftService : IBaseDTOService<AirCraftsDto, DB.Models.AirCrafts>
    {
        Task<List<AirCraftsDto>> GetAllAircrafts(bool useCache = true);
        IIncludableQueryable<AirCrafts, AircraftSpecifications> GetAllAircraftsAsQueryable();
        IQueryable<AirCrafts> GetAllAircraftsOnlyAsQueryable();
        Task<AirCraftsDto> GetAircrafts(int oid);
        Task AddAirCrafts(AirCraftsDto airCrafts);
        Task UpdateAirCrafts(AirCraftsDto airCrafts);
        Task RemoveAirCrafts(AirCraftsDto airCrafts);
    }

    public class AircraftService : BaseDTOService<AirCraftsDto, DB.Models.AirCrafts, DegaContext>, IAircraftService
    {
        private int _CacheLifeSpanInMinutes = 60;
        private string _AllAircraftCacheKey = "AllAircraftFactorySpecifications";

        private AircraftEntityService _AircraftEntityService;
        private IMemoryCache _MemoryCache;

        public AircraftService(AircraftEntityService aircraftEntityService, IMemoryCache memoryCache) : base(aircraftEntityService)
        {
            _MemoryCache = memoryCache;
            _AircraftEntityService = aircraftEntityService;
        }

        public async Task<List<AirCraftsDto>> GetAllAircrafts(bool useCache = true)
        {
            List<AirCraftsDto> result = null;

            if (useCache)
            {
                _MemoryCache.TryGetValue(_AllAircraftCacheKey, out result);
            }

            if (result == null)
            {
                result = await GetListbySpec(new AircraftSpecification());
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_CacheLifeSpanInMinutes));
                _MemoryCache.Set(_AllAircraftCacheKey, result, cacheEntryOptions);
            }

            return result;
        }

        public IIncludableQueryable<AirCrafts, AircraftSpecifications> GetAllAircraftsAsQueryable()
        {
            return _AircraftEntityService.GetAllAircraftsAsQueryable();
        }

        public IQueryable<AirCrafts> GetAllAircraftsOnlyAsQueryable()
        {
            return _AircraftEntityService.GetAllAircraftsOnlyAsQueryable();
        }

        public async Task<AirCraftsDto> GetAircrafts(int oid)
        {
            return await GetSingleBySpec(new AircraftSpecification(new List<int>() { oid }));
        }

        public async Task AddAirCrafts(AirCraftsDto airCrafts)
        {
            await AddAsync(airCrafts);
            _MemoryCache.Remove(_AllAircraftCacheKey);
        }

        public async Task UpdateAirCrafts(AirCraftsDto airCrafts)
        {
            await UpdateAsync(airCrafts);
            _MemoryCache.Remove(_AllAircraftCacheKey);
        }

        public async Task RemoveAirCrafts(AirCraftsDto airCrafts)
        {
            await DeleteAsync(airCrafts);
            _MemoryCache.Remove(_AllAircraftCacheKey);
        }
    }
}
