using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.Enums;
using FBOLinx.Core.Utilities.Geography;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.AcukwikAirport;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using FBOLinx.ServiceLayer.EntityServices;
using Geolocation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SendGrid.Helpers.Mail;

namespace FBOLinx.ServiceLayer.BusinessServices.Airport
{
    public interface IAirportService : IBaseDTOService<AcukwikAirportDTO, DB.Models.AcukwikAirport>
    {
        Task<Fboairports> GetAirportForFboId(int fboId);
        Task<AcukwikAirport> GetAirportByAcukwikAirportId(int acukwikAirportId);
        Task<List<AcukwikAirport>> GetAirportsByAcukwikAirportIds(List<int> acukwikAirportIds);
        Task<AcukwikAirport> GetAirportByAirportIdentifier(string airportIdentifier);
        Task<List<AcukwikAirport>> GetAirportsByAirportIdentifier(List<string> airportIdentifiers);
        Task<AirportPosition> GetAirportPositionForFbo(int fboId);
        Task<AirportPosition> GetAirportPositionByAirportIdentifier(string airportIdentifier);
        Task<AirportPosition> GetNearestAirportPosition(double latitude, double longitude);

        Task<List<AcukwikAirportDTO>> GetAirportsWithinRange(string airportIdentifierForCenterAirport,
            int nauticalMileRadius, bool mustProvideJetFuel = true, bool excludeMilitary = true);
        Task<List<AirportPosition>> GetAirportPositions();
        Task<List<Fuelerlinx.SDK.GeneralAirportInformation>> GetGeneralAirportInformationList();
        Task<Fuelerlinx.SDK.GeneralAirportInformation> GetGeneralAirportInformation(string airportIdentifier);
    }

    public class AirportService :
        BaseDTOService<AcukwikAirportDTO, DB.Models.AcukwikAirport, FboLinxContext>, IAirportService
    {
        private string _AllAirportsPositioningCacheKey = "AirportWatchService_AllAirportsPositioning";
        private string _GeneralAirportInfoCacheKey = "AirportWatchService_AllAirports_GeneralAirportInfo";
        private FboLinxContext _fboLinxContext;
        private DegaContext _degaContext;
        private IMemoryCache _MemoryCache;
        private List<AirportPosition> _AirportPositions;
        private IFboEntityService _FboEntityService;
        private AcukwikAirportEntityService _AcukwikAirportEntityService;
        private FuelerLinxApiService _FuelerLinxApiService;

        public AirportService(IRepository<AcukwikAirport, FboLinxContext> entityService, FboLinxContext fboLinxContext, DegaContext degaContext, IMemoryCache memoryCache, IFboEntityService fboEntityService, 
            AcukwikAirportEntityService acukwikAirportEntityService,
            FuelerLinxApiService fuelerLinxApiService) : base(
            entityService)
        {
            _FuelerLinxApiService = fuelerLinxApiService;
            _AcukwikAirportEntityService = acukwikAirportEntityService;
            _FboEntityService = fboEntityService;
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
            var airports = await _degaContext.AcukwikAirports.Where(x => acukwikAirportIds.Contains(x.Oid)).Include(x => x.AcukwikFbohandlerDetailCollection).ToListAsync();
            return airports;
        }

        public async Task<AcukwikAirport> GetAirportByAirportIdentifier(string airportIdentifier)
        {
            return (await GetAirportsByAirportIdentifier(new List<string>() { airportIdentifier })).FirstOrDefault();
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

        public async Task<AirportPosition> GetAirportPositionForFbo(int fboId)
        {
            var fbo = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));
            if (fbo == null || fbo.FboAirport == null)
                return null;
            return await GetAirportPositionByAirportIdentifier(fbo.FboAirport.Icao);
        }

        public async Task<AirportPosition> GetAirportPositionByAirportIdentifier(string airportIdentifier)
        {
            var positions = await GetAirportPositions();
            airportIdentifier = airportIdentifier.ToUpper();
            return positions?.Where(x => x.GetProperAirportIdentifier() == airportIdentifier).FirstOrDefault();
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

        public async Task<List<AcukwikAirportDTO>> GetAirportsWithinRange(string airportIdentifierForCenterAirport,
            int nauticalMileRadius, bool mustProvideJetFuel = true, bool excludeMilitary = true)
        {
            List<AirportPosition> airportPositions = await GetAirportPositions();
            var centerAirportPosition =
                airportPositions.FirstOrDefault(x => x.Icao?.ToUpper() == airportIdentifierForCenterAirport?.ToUpper());

            if (centerAirportPosition == null)
                return new List<AcukwikAirportDTO>();

            var airportTypeFilter = new string[] { EnumHelper.GetDescription(AirportTypeEnum.JointCivilMilitary), EnumHelper.GetDescription(AirportTypeEnum.Civil) };
            var fuelTypeFilter = new string[] { EnumHelper.GetDescription(FuelTypeEnum.AvgasJet), EnumHelper.GetDescription(FuelTypeEnum.Jet), EnumHelper.GetDescription(FuelTypeEnum.JetOnly), EnumHelper.GetDescription(FuelTypeEnum.Unknown) };

            var nearestAirportPositions = airportPositions.Where(x => DistanceHelper.MetersToNauticalMiles(
                DistanceHelper.GetDistanceBetweenTwoPoints(
                    centerAirportPosition.Latitude, centerAirportPosition.Longitude,
                    x.Latitude, x.Longitude)) < nauticalMileRadius).ToList();

            var distinctMatchingAirports = nearestAirportPositions?.Select(x => x.GetProperAirportIdentifier());

            var result = await _degaContext.AcukwikAirports
                .Where(x => distinctMatchingAirports.Contains(x.Icao))
                .Where(x => !excludeMilitary || airportTypeFilter.Contains(x.AirportType))
                .Where(x => !mustProvideJetFuel || fuelTypeFilter.Contains(x.FuelType))
                .ToListAsync();

            return result?.Select(x => x.Adapt<AcukwikAirportDTO>()).ToList();
        }

        public async Task<List<AirportPosition>> GetAirportPositions()
        {
            if (_AirportPositions?.Count > 0)
                return _AirportPositions;

            _AirportPositions = LookupAirportPositionRecordsByFromCache();

            if (_AirportPositions == null)
            {
                var airports = (await (_degaContext.AcukwikAirports

                            .Where(x => !string.IsNullOrEmpty(x.Latitude) && !string.IsNullOrEmpty(x.Longitude))
                            .Select(x => new 
                            {
                                Latitude = x.Latitude,
                                Longitude = x.Longitude,
                                Icao = x.Icao,
                                Iata = x.Iata,
                                Faa = x.Faa,
                                DaylightSavingsYn = x.DaylightSavingsYn,
                                IntlTimeZone = x.IntlTimeZone
                            })
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

        public async Task<Fuelerlinx.SDK.GeneralAirportInformation> GetGeneralAirportInformation(string airportIdentifier)
        {
            var generalInfoList = await GetGeneralAirportInformationList();
            airportIdentifier = airportIdentifier?.ToUpper();
            return generalInfoList?.FirstOrDefault(x =>
                x.ProperAirportIdentifier?.ToUpper() == airportIdentifier);
        }

        public async Task<List<Fuelerlinx.SDK.GeneralAirportInformation>> GetGeneralAirportInformationList()
        {
            var cachedResult = GetGeneralAirportInfoListFromCache();
            if (cachedResult != null && cachedResult.Count > 0)
                return cachedResult;

            cachedResult = await _FuelerLinxApiService.GetAllAirportGeneralInformation();

            //Store in cache before returning
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
            _MemoryCache.Set(_GeneralAirportInfoCacheKey, cachedResult, cacheEntryOptions);
            return cachedResult;
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

        private List<AirportPosition> LookupAirportPositionRecordsByFromCache()
        {
            List<AirportPosition> result = null;
            if (_MemoryCache.TryGetValue(_AllAirportsPositioningCacheKey, out result))
                return result;
            return result;
        }


        private List<Fuelerlinx.SDK.GeneralAirportInformation> GetGeneralAirportInfoListFromCache()
        {
            try
            {
                List<Fuelerlinx.SDK.GeneralAirportInformation> result;
                if (_MemoryCache.TryGetValue(_GeneralAirportInfoCacheKey, out result))
                    return result;
                return null;
            }
            catch (System.Exception exception)
            {
                return null;
            }
        }
    }
}
