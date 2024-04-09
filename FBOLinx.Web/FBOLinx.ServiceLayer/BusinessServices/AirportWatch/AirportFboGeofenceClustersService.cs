using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FboGeofence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.DB.Specifications.AirportFboGeoFence;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.Extensions.Caching.Memory;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface IAirportFboGeofenceClustersService : IBaseDTOService<AirportFboGeofenceClustersDto, DB.Models.AirportFboGeofenceClusters>
    {
        Task<AirportFboGeofenceClustersDto> CreateNewCluster(AirportFboGeofenceClustersDto airportFboGeoFenceClusters);
        Task DeleteCluster(int id);
        Task<List<string>> GetDistinctAirportIdentifiersWithClusters();
        Task<List<AirportFboGeofenceClustersDto>> GetAllClusters(int acukwikAirportId = 0, int acukwikFboHandlerId = 0, bool useCache = true);
        Task<List<AirportFboGeofenceClusterCoordinatesDto>> GetClusterCoordinatesByClusterId(int clusterId);
    }

    public class AirportFboGeofenceClustersService : BaseDTOService<AirportFboGeofenceClustersDto, DB.Models.AirportFboGeofenceClusters, FboLinxContext>, IAirportFboGeofenceClustersService
    {
        private int _CacheLifeSpanInMinutes = 60;
        private string _AllGeoFenceClustersCacheKey = "AllGeoFenceClusters";
        
        private readonly FboLinxContext _context;
        private IAirportService _airportService;
        private IMemoryCache _MemoryCache;

        public AirportFboGeofenceClustersService(IAirportFboGeoFenceClusterEntityService airportFboGeoFenceClusterEntityService, FboLinxContext context,
            IAirportService airportService,
            IMemoryCache memoryCache) : base(airportFboGeoFenceClusterEntityService)
        {
            _MemoryCache = memoryCache;
            _airportService = airportService;
            _context = context;
        }

        public async Task<AirportFboGeofenceClustersDto> CreateNewCluster(AirportFboGeofenceClustersDto airportFboGeoFenceClusters)
        {
            try
            {
                var result = await AddAsync(airportFboGeoFenceClusters);
                _MemoryCache.Remove(_AllGeoFenceClustersCacheKey);
                return result;
            }
            catch (Exception ex)
            {

            }

            return airportFboGeoFenceClusters;
        }

        public async Task DeleteCluster(int id)
        {
            try
            {
                await _EntityService.DeleteAsync(id);
                _MemoryCache.Remove(_AllGeoFenceClustersCacheKey);
            }
            catch (System.Exception exception)
            {

            }
        }

        public async Task<List<string>> GetDistinctAirportIdentifiersWithClusters()
        {
            var distinctAirportIds = await _context.AirportFboGeofenceClusters.Select(x => x.AcukwikAirportID).Distinct().ToListAsync();
            var airports = await _airportService.GetAirportsByAcukwikAirportIds(distinctAirportIds);
            return airports.Select(x => x.Icao).Distinct().ToList();
        }


        public async Task<List<AirportFboGeofenceClustersDto>> GetAllClusters(int acukwikAirportId = 0, int acukwikFboHandlerId = 0, bool useCache = true)
        {
            List<AirportFboGeofenceClustersDto> allFboGeoClusters = null;

            if (useCache)
            {
                _MemoryCache.TryGetValue(_AllGeoFenceClustersCacheKey, out allFboGeoClusters);
            }

            if (allFboGeoClusters != null)
                return allFboGeoClusters.Where(x => (acukwikAirportId == 0 || x.AcukwikAirportID == acukwikAirportId)
                && (acukwikFboHandlerId == 0 || x.AcukwikFBOHandlerID == acukwikFboHandlerId)
                ).ToList();

            allFboGeoClusters = await GetListbySpec(new AirportFboGeoFenceClusterSpecification());

            var airportIds = allFboGeoClusters.Select(x => x.AcukwikAirportID).Distinct().ToList();

            var airports = await _airportService.GetAirportsByAcukwikAirportIds(airportIds);

            allFboGeoClusters.ForEach(x =>
            {
                var airport = airports.FirstOrDefault(a => a.Oid == x.AcukwikAirportID);
                if (airport == null)
                    return;
                x.Icao = airport.Icao;
                var fbo = airport.AcukwikFbohandlerDetailCollection?.FirstOrDefault(f =>
                    f.HandlerId == x.AcukwikFBOHandlerID);
                if (fbo == null)
                    return;
                x.AcukwikFBOHandlerID = fbo.HandlerId;
                x.FboName = fbo.HandlerLongName;
                if (x.ClusterCoordinatesCollection?.Count == 0)
                    return;
                x.ClusterCoordinatesCollection = x.ClusterCoordinatesCollection?.OrderBy(x => x.Oid).ToList();
            });

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_CacheLifeSpanInMinutes));
            _MemoryCache.Set(_AllGeoFenceClustersCacheKey, allFboGeoClusters, cacheEntryOptions);

            return allFboGeoClusters.Where(x => (acukwikAirportId == 0 || x.AcukwikAirportID == acukwikAirportId)
                                                && (acukwikFboHandlerId == 0 || x.AcukwikFBOHandlerID == acukwikFboHandlerId)).ToList();
        }

        public async Task<List<AirportFboGeofenceClusterCoordinatesDto>> GetClusterCoordinatesByClusterId(int clusterId)
        {
            var cluster = await GetSingleBySpec(new AirportFboGeoFenceClusterSpecification(clusterId));
            return cluster?.ClusterCoordinatesCollection?.OrderBy(c => c.Oid).ToList();
        }
    }
}
