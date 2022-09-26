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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{


    public class AircraftService : BaseDTOService<AirCraftsDto, DB.Models.AirCrafts, DegaContext>
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
            return _degaContext.AirCrafts.AsNoTracking();
        }

        public async Task<AirCraftsDto> GetAircrafts(int oid)
        {
            return await GetSingleBySpec(new AircraftSpecification(new List<int>() { oid }));
        }

        public async Task AddAirCrafts(AirCrafts airCrafts)
        {
            _degaContext.AirCrafts.Add(airCrafts);
            await _degaContext.SaveChangesAsync();
        }

        public async Task UpdateAirCrafts(AirCrafts airCrafts)
        {
            _degaContext.AirCrafts.Update(airCrafts);
            await _degaContext.SaveChangesAsync();
        }

        public async Task RemoveAirCrafts(AirCrafts airCrafts)
        {
            _degaContext.AirCrafts.Remove(airCrafts);
            await _degaContext.SaveChangesAsync();
        }
    }
}
