using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FboGeofence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class AirportFboGeofenceClustersService
    {
        private readonly IServiceProvider _services;
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        public AirportFboGeofenceClustersService(FboLinxContext context, IServiceProvider services, DegaContext degaContext)
        {
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
            var airportFboGeoFenceClusters = _context.AirportFboGeofenceClusters.Find(id);
            _context.AirportFboGeofenceClusters.Remove(airportFboGeoFenceClusters);

            await _context.SaveChangesAsync();
        }

        public async Task<List<AirportFboGeofenceClusters>> GetAllClusters(int acukwikAirportId = 0)
        {
            var allFboGeoClusters = await _context.AirportFboGeofenceClusters.Where(x => (acukwikAirportId == 0 || x.AcukwikAirportID == acukwikAirportId)).ToListAsync();

            var airportIds = allFboGeoClusters.Select(x => x.AcukwikAirportID).Distinct().ToList();

            var airports = await _degaContext.AcukwikAirports.Where(x => airportIds.Contains(x.AirportId)).Include(x => x.AcukwikFbohandlerDetailCollection)
                .ToListAsync();
            
            allFboGeoClusters.ForEach(x =>
            {
                var airport = airports.FirstOrDefault(a => a.AirportId == x.AcukwikAirportID);
                if (airport == null)
                    return;
                x.Icao = airport.Icao;
                var fbo = airport.AcukwikFbohandlerDetailCollection?.FirstOrDefault(f =>
                    f.HandlerId == x.AcukwikFBOHandlerID);
                if (fbo == null)
                    return;
                x.AcukwikFBOHandlerID = fbo.HandlerId;
            });

            return allFboGeoClusters;
        }
    }
}
