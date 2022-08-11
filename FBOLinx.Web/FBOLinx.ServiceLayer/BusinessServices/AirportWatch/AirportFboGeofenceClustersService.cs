using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FboGeofence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.ServiceLayer.BusinessServices.Airport;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public class AirportFboGeofenceClustersService
    {
        private readonly IServiceProvider _services;
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private IAirportService _airportService;

        public AirportFboGeofenceClustersService(FboLinxContext context, IServiceProvider services, DegaContext degaContext,
            IAirportService airportService)
        {
            _airportService = airportService;
            _context = context;
            _services = services;
            _degaContext = degaContext;
        }

        public async Task<AirportFboGeofenceClusters> CreateNewCluster(AirportFboGeofenceClusters airportFboGeoFenceClusters)
        {
            try
            {
                _context.AirportFboGeofenceClusters.Add(airportFboGeoFenceClusters);
                await _context.SaveChangesAsync();
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
                var airportFboGeoFenceClusters = await _context.AirportFboGeofenceClusters.Where(x => x.Oid == id)
                    .Include(x => x.ClusterCoordinatesCollection).FirstOrDefaultAsync();
                if (airportFboGeoFenceClusters.ClusterCoordinatesCollection != null)
                {
                    foreach (var clusterCoordinate in airportFboGeoFenceClusters.ClusterCoordinatesCollection)
                    {
                        _context.Entry(clusterCoordinate).State = EntityState.Deleted;
                    }
                }

                _context.Entry(airportFboGeoFenceClusters).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
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


        public async Task<List<AirportFboGeofenceClusters>> GetAllClusters(int acukwikAirportId = 0)
        {
            var allFboGeoClusters = await _context.AirportFboGeofenceClusters
                .Where(x => (acukwikAirportId == 0 || x.AcukwikAirportID == acukwikAirportId))
                .Include(x => x.ClusterCoordinatesCollection)
                .ToListAsync();

            var airportIds = allFboGeoClusters.Select(x => x.AcukwikAirportID).Distinct().ToList();

            var airports = await _degaContext.AcukwikAirports.Where(x => airportIds.Contains(x.Oid)).Include(x => x.AcukwikFbohandlerDetailCollection)
                .ToListAsync();

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
                x.ClusterCoordinatesCollection = x.ClusterCoordinatesCollection.OrderBy(x => x.Oid).ToList();
            });

            return allFboGeoClusters;
        }
        public async Task<IQueryable<AirportFboGeofenceClusters>> GetAllClustersIQueryable(int acukwikAirportId = 0)
        {
            var allFboGeoClusters = _context.AirportFboGeofenceClusters
                .Where(x => (acukwikAirportId == 0 || x.AcukwikAirportID == acukwikAirportId))
                .Include(x => x.ClusterCoordinatesCollection)
                .AsNoTracking();

            var airportIds = await allFboGeoClusters.Select(x => x.AcukwikAirportID).Distinct().ToListAsync();

            var airports = _degaContext.AcukwikAirports.Where(x => airportIds.Contains(x.Oid)).Include(x => x.AcukwikFbohandlerDetailCollection).AsNoTracking();

            (await allFboGeoClusters.ToListAsync()).ForEach(x =>
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
            });
            return allFboGeoClusters;
            
        }
        public async Task<List<AirportFboGeofenceClusterCoordinates>> GetClusterCoordinatesByClusterId(int clusterId)
        {
            var clusterCoordinates = await _context.AirportFboGeoFenceClusterCoordinates.Where(a => a.ClusterID == clusterId).ToListAsync();
            return clusterCoordinates;
        }
        public async Task<IQueryable<AirportFboGeofenceClusterCoordinates>> GetClusterCoordinatesByClusterIdIQueryable(int clusterId)
        {
            return _context.AirportFboGeoFenceClusterCoordinates.Where(a => a.ClusterID == clusterId).AsNoTracking();
        }
    }
}
