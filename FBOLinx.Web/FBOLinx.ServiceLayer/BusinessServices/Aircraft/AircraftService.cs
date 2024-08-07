using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.Aircraft;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.JetNet;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
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
        Task<AirCraftsDto> GetAircraftTypeByIcao(string icao);
    }

    public class AircraftService : BaseDTOService<AirCraftsDto, DB.Models.AirCrafts, DegaContext>, IAircraftService
    {
        private int _CacheLifeSpanInMinutes = 60;
        private string _AllAircraftCacheKey = "AllAircraftFactorySpecifications";

        private AircraftEntityService _AircraftEntityService;
        private IMemoryCache _MemoryCache;
        private FuelerLinxApiService _FuelerLinxApiService;

        public AircraftService(AircraftEntityService aircraftEntityService, IMemoryCache memoryCache, FuelerLinxApiService fuelerLinxApiService) : base(aircraftEntityService)
        {
            _MemoryCache = memoryCache;
            _AircraftEntityService = aircraftEntityService;
            _FuelerLinxApiService = fuelerLinxApiService;
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
                result = await GetListbySpec(new DB.Specifications.Aircraft.AircraftSpecification());
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
            return await GetSingleBySpec(new DB.Specifications.Aircraft.AircraftSpecification(new List<int>() { oid }));
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

        public async Task<AirCraftsDto> GetAircraftTypeByIcao(string icao)
        {
            List<AircraftDTO> result = null;
            var cacheKey = "AircraftTypes";
            var aircraft = new AircraftDTO();

            _MemoryCache.TryGetValue(cacheKey, out result);
            
            if (result == null)
            {
                var aircrafts = await _FuelerLinxApiService.GetAircraftTypes();
                result = aircrafts.Result.ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_CacheLifeSpanInMinutes));
                _MemoryCache.Set(cacheKey, result, cacheEntryOptions);
            }

            aircraft = result.FirstOrDefault(x => x.Icao == icao);
            if (aircraft == null)
            {
                return null;
            }

            AirCraftsDto aircraftResult = new AirCraftsDto() { Make = aircraft.Make, Model = aircraft.Model};
            return aircraftResult;
        }
    }
}
