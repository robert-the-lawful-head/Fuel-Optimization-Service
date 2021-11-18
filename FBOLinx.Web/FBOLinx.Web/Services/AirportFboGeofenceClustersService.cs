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

        public async Task<List<FboGeofenceClustersViewModel>> GetAllClusters()
        {
            var allFboGeoClusters = await (from afgc in _context.AirportFboGeofenceClusters select afgc).ToListAsync();

            var allFbos = await (from aa in _degaContext.AcukwikAirports
                                 join al in _degaContext.AcukwikFbohandlerDetail on aa.AirportId equals al.AirportId
                                 select new FboGeofenceClustersViewModel
                                 {
                                     AcukwikAirportID = aa.AirportId,
                                     AcukwikFBOHandlerID = al.HandlerId,
                                     Icao = aa.Icao,
                                     Fbo = al.HandlerLongName,
                                 }).ToListAsync();

            var fboClusters = (from afgc in allFboGeoClusters
                               join af in allFbos on new { afgc.AcukwikFBOHandlerID, afgc.AcukwikAirportID } equals new { af.AcukwikFBOHandlerID, af.AcukwikAirportID }
                               select new FboGeofenceClustersViewModel
                               {
                                   Oid = afgc.Oid,
                                   AcukwikAirportID = af.AcukwikAirportID,
                                   AcukwikFBOHandlerID = af.AcukwikFBOHandlerID,
                                   CenterLatitude = afgc.CenterLatitude,
                                   CenterLongitude = afgc.CenterLongitude,
                                   Icao = af.Icao,
                                   Fbo = af.Fbo
                               }).ToList();

            //var fboClusters = (from af in allFbos
            //                   join afgc in allFboGeoClusters on new { af.AcukwikFBOHandlerID, af.AcukwikAirportID } equals new { afgc.AcukwikFBOHandlerID, afgc.AcukwikAirportID }
            //                   into afgcLeftJoin
            //                   from afgc in afgcLeftJoin.DefaultIfEmpty()
            //                   select new FboGeofenceClustersViewModel
            //                   {
            //                       Oid = afgc == null ? 0 : afgc.Oid,
            //                       AcukwikAirportID = af.AcukwikAirportID,
            //                       AcukwikFBOHandlerID = af.AcukwikFBOHandlerID,
            //                       CenterLatitude = afgc == null ? 0 : afgc.CenterLatitude,
            //                       CenterLongitude = afgc == null ? 0 : afgc.CenterLongitude,
            //                       Icao = af.Icao,
            //                       Fbo = af.Fbo
            //                   }).ToList();

            return fboClusters;
        }
    }
}
