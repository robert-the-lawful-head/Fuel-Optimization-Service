using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using Geolocation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FBOLinx.ServiceLayer.BusinessServices.Airport
{
    public interface IAirportService
    {
        Task<Fboairports> GetAirportForFboId(int fboId);
        Task<AcukwikAirport> GetAirportByAcukwikAirportId(int acukwikAirportId);
        Task<List<AcukwikAirport>> GetAirportsByAcukwikAirportIds(List<int> acukwikAirportIds);
        Task<List<AcukwikAirport>> GetAirportsByAirportIdentifier(List<string> airportIdentifiers);
        Task<AirportPosition> GetNearestAirportPosition(double latitude, double longitude);
        Task<List<AirportPosition>> GetAirportPositions();
    }

    //TODO: Convert this to a DTO and Entity Service!
    public class AirportService : IAirportService
    {
        private string _AllAirportsPositioningCacheKey = "AirportWatchService_AllAirportsPositioning";
        private FboLinxContext _fboLinxContext;
        private DegaContext _degaContext;
        private IMemoryCache _MemoryCache;
        private List<AirportPosition> _AirportPositions;

        public AirportService(FboLinxContext fboLinxContext, DegaContext degaContext, IMemoryCache memoryCache)
        {
            _MemoryCache = memoryCache;
            _degaContext = degaContext;
            _fboLinxContext = fboLinxContext;
        }

        public async Task<Fboairports> GetAirportForFboId(int fboId)
        {
            var airport = await _fboLinxContext.Fboairports.FirstOrDefaultAsync(x => x.Fboid == fboId);
            return airport;
        }

        public async Task<AcukwikAirport> GetAirportByAcukwikAirportId(int acukwikAirportId)
        {
            var airport = await _degaContext.AcukwikAirports.FirstOrDefaultAsync(x => x.Oid == acukwikAirportId);
            return airport;
        }

        public async Task<List<AcukwikAirport>> GetAirportsByAcukwikAirportIds(List<int> acukwikAirportIds)
        {
            var airports = await _degaContext.AcukwikAirports.Where(x => acukwikAirportIds.Contains(x.Oid)).ToListAsync();
            return airports;
        }

        public async Task<List<AcukwikAirport>> GetAirportsByAirportIdentifier(List<string> airportIdentifiers)
        {
            var airports = await _degaContext.AcukwikAirports.Where(x =>
                ((x.Icao != null && airportIdentifiers.Contains(x.Icao))
                 || (x.Iata != null && airportIdentifiers.Contains(x.Icao))
                 || (x.Faa != null && airportIdentifiers.Contains(x.Faa))))
                .Include(x => x.AcukwikFbohandlerDetailCollection)
                .AsNoTracking()
                .ToListAsync();
            return airports;
        }

        public async Task<AirportPosition> GetNearestAirportPosition(double latitude, double longitude)
        {
            List<AirportPosition> airportPositions = await GetAirportPositions();
            double minDistance = -1;
            AirportPosition nearestAirportPosition = null;
            var closestAirports = airportPositions.Where(a => a.Latitude >= latitude - 1 && a.Latitude <= latitude + 1 && a.Longitude >= longitude - 1 && a.Longitude <= longitude + 1).ToList();
            foreach (var airport in closestAirports)
            {
                double distance = GeoCalculator.GetDistance(latitude, longitude, airport.Latitude, airport.Longitude, 5, DistanceUnit.Miles);

                if (minDistance == -1 || distance < minDistance)
                {
                    minDistance = distance;
                    nearestAirportPosition = airport;
                }
            }

            return nearestAirportPosition;
        }

        public async Task<List<AirportPosition>> GetAirportPositions()
        {
            if (_AirportPositions?.Count > 0)
                return _AirportPositions;

            _AirportPositions = LookupAirportRecordsByFromCache();

            if (_AirportPositions == null)
            {
                var airports = (await (_degaContext.AcukwikAirports

                            .Where(x => !string.IsNullOrEmpty(x.Latitude) && !string.IsNullOrEmpty(x.Longitude))
                            .AsNoTracking())
                        .ToListAsync())
                    .Select(a =>
                    {
                        var (alat, alng) = GetGeoLocationFromGPS(a.Latitude, a.Longitude);

                        return new AirportPosition
                        {
                            Latitude = alat,
                            Longitude = alng,
                            Icao = a.Icao,
                            Iata = a.Iata,
                            Faa = a.Faa,
                            DaylightSavingsYn = a.DaylightSavingsYn,
                            IntlTimeZone = a.IntlTimeZone
                        };
                    })
                    .ToList();

                //Store in cache before returning
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
                _MemoryCache.Set(_AllAirportsPositioningCacheKey, airports, cacheEntryOptions);

                _AirportPositions = airports;
            }

            return _AirportPositions;
        }

        private Tuple<double, double> GetGeoLocationFromGPS(string lat, string lng)
        {
            var latDirection = lat.Substring(0, 1);
            var lngDirection = lng.Substring(0, 1);

            double latitude = double.Parse(lat.Substring(1, 2)) + double.Parse(lat.Substring(4, 2)) / 60 + double.Parse(lat[7..]) / 3600;
            double longitude = lng.Length == 8 ?
                double.Parse(lng.Substring(1, 2)) + double.Parse(lng.Substring(4, 2)) / 60 + double.Parse(lng[6..]) / 3600 :
                double.Parse(lng.Substring(1, 3)) + double.Parse(lng.Substring(5, 2)) / 60 + double.Parse(lng[7..]) / 3600;

            if (latDirection != "N") latitude = -latitude;
            if (lngDirection != "E") longitude = -longitude;

            return new Tuple<double, double>(latitude, longitude);
        }

        private List<AirportPosition> LookupAirportRecordsByFromCache()
        {
            List<AirportPosition> result = null;
            if (_MemoryCache.TryGetValue(_AllAirportsPositioningCacheKey, out result))
                return result;
            return result;
        }
    }
}
