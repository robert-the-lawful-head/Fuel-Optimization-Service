using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using Geolocation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.DB;
using System.Diagnostics;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.Extensions;
using FBOLinx.DB.Specifications.Aircraft;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using Fuelerlinx.SDK;
using Microsoft.Extensions.Options;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using Microsoft.Extensions.Caching.Memory;
using FBOLinx.ServiceLayer.DTO;
using Microsoft.Extensions.DependencyInjection;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.DTO.Requests.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Requests.FuelPricing;
using FBOLinx.ServiceLayer.DTO.Responses.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Responses.FuelPricing;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.Extensions.Aircraft;
using FBOLinx.Core.Utilities.Geography;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public class AirportWatchService
    {
        private const int _distance = 250;

        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private readonly AircraftService _aircraftService;
        private readonly IFboService _FboService;
        private List<AirportWatchLiveDataDto> _LiveDataToUpdate;
        private List<AirportWatchLiveDataDto> _LiveDataToInsert;
        private List<AirportWatchLiveDataDto> _LiveDataToDelete;
        private List<AirportWatchHistoricalDataDto> _HistoricalDataToUpdate;
        private List<AirportWatchHistoricalDataDto> _HistoricalDataToInsert;
        private FuelerLinxApiService _fuelerLinxApiService;
        private IOptions<DemoData> _demoData;
        private readonly AirportFboGeofenceClustersService _airportFboGeofenceClustersService;
        private readonly FbopricesService _fboPricesService;
        private ICustomerAircraftEntityService _customerAircraftsEntityService;
        private ICustomerInfoByGroupEntityService _customerInfoByGroupEntityService;
        private IServiceProvider _ServiceProvider;
        private readonly ILoggingService _LoggingService;
        private IFuelReqService _FuelReqService;
        private IAirportWatchLiveDataService _AirportWatchLiveDataService;
        private readonly AFSAircraftEntityService _AFSAircraftEntityService;
        private IAirportWatchHistoricalDataService _AirportWatchHistoricalDataService;
        private readonly AirportWatchLiveDataEntityService _AirportWatchLiveDataEntityService;
        private readonly SWIMFlightLegEntityService _SWIMFlightLegEntityService;
        private readonly IAirportWatchDistinctBoxesService _AirportWatchDistinctBoxesService;
        private IAirportService _AirportService;

        public AirportWatchService(FboLinxContext context, DegaContext degaContext, AircraftService aircraftService, 
            IFboService fboService, FuelerLinxApiService fuelerLinxApiService,
            IOptions<DemoData> demoData, AirportFboGeofenceClustersService airportFboGeofenceClustersService,
            FbopricesService fboPricesService, ICustomerAircraftEntityService customerAircraftsEntityService, 
            ICustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            IServiceProvider serviceProvider, ILoggingService loggingService,
            IFuelReqService fuelReqService,
            IAirportWatchLiveDataService airportWatchLiveDataService,
            AFSAircraftEntityService afsAircraftEntityService,
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService,
            SWIMFlightLegEntityService swimFlightLegEntityService,
            IAirportWatchHistoricalDataService airportWatchHistoricalDataService,
            IAirportWatchDistinctBoxesService airportWatchDistinctBoxesService,
            IAirportService airportService)
        {
            _AirportService = airportService;
            _AirportWatchHistoricalDataService = airportWatchHistoricalDataService;
            _AirportWatchLiveDataService = airportWatchLiveDataService;
            _FuelReqService = fuelReqService;
            _ServiceProvider = serviceProvider;
            _demoData = demoData;
            _context = context;
            _degaContext = degaContext;
            _aircraftService = aircraftService;
            _FboService = fboService;
            _fuelerLinxApiService = fuelerLinxApiService;
            _airportFboGeofenceClustersService = airportFboGeofenceClustersService;
            _fboPricesService = fboPricesService;
            _customerAircraftsEntityService = customerAircraftsEntityService;
            _customerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _LoggingService = loggingService;
            _AFSAircraftEntityService = afsAircraftEntityService;
            _AirportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
            _SWIMFlightLegEntityService = swimFlightLegEntityService;
            _AirportWatchDistinctBoxesService = airportWatchDistinctBoxesService;
        }
        public async Task<AircraftWatchLiveData> GetAircraftWatchLiveData(int groupId, int fboId, string tailNumber)
        {
            var liveDataMinDateTime = DateTime.UtcNow.AddMinutes(-10);

            var aircraftWatchLiveData = await _context.AirportWatchLiveData.Where(x => x.TailNumber == tailNumber && x.AircraftPositionDateTimeUtc >= liveDataMinDateTime).FirstOrDefaultAsync();

            var customerAircrafts = await _customerAircraftsEntityService.Where(x => x.TailNumber == tailNumber && x.GroupId == groupId).FirstOrDefaultAsync();

            CustomerInfoByGroup customerInfoByGroup = null;

            if (customerAircrafts != null)
            {
                customerInfoByGroup = await _customerInfoByGroupEntityService
                    .Where(x => x.CustomerId == customerAircrafts.CustomerId && x.GroupId == groupId)
                    .Include(x => x.Customer)
                    .FirstOrDefaultAsync();
            }


            var aircaft = await _degaContext.AirCrafts.Where(x => x.AircraftId == (customerAircrafts == null ? 0 : customerAircrafts.AircraftId)).FirstOrDefaultAsync();

            var fboairports = await _context.Fboairports.Where(x => x.Fboid == fboId).FirstOrDefaultAsync();

            CompanyPricingLog pricingLogs = null;
            if (customerInfoByGroup != null && customerInfoByGroup?.Customer?.FuelerlinxId > 0)
            {
                pricingLogs = await _context.CompanyPricingLog
                    .Where(x => x.CompanyId == customerInfoByGroup.Customer.FuelerlinxId.Value)
                    .OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            }

            PriceLookupResponse customerPricing = null;
            if (customerInfoByGroup?.Oid > 0 && customerAircrafts?.Oid > 0)
            {
                customerPricing = await _fboPricesService.GetFuelPricesForCustomer(
                    new PriceLookupRequest()
                    {
                        CustomerInfoByGroupId = customerInfoByGroup.Oid,
                        DepartureType = Core.Enums.ApplicableTaxFlights.DomesticOnly,
                        FBOID = fboId,
                        FlightTypeClassification = Core.Enums.FlightTypeClassifications.Private,
                        GroupID = groupId,
                        ICAO = fboairports.Icao,
                        TailNumber = tailNumber
                    });
            }

            return new AircraftWatchLiveData()
            {
                CustomerInfoBygGroupId = customerInfoByGroup?.Oid,
                TailNumber = aircraftWatchLiveData?.TailNumber,
                AtcFlightNumber = aircraftWatchLiveData?.AtcFlightNumber,
                AircraftTypeCode = aircraftWatchLiveData?.AircraftTypeCode,
                IsAircraftOnGround = (aircraftWatchLiveData?.IsAircraftOnGround).GetValueOrDefault(),
                Company = customerInfoByGroup?.Customer?.Company,
                AircraftMakeModel = aircaft?.Make + " " + aircaft?.Model,
                LastQuote = pricingLogs?.CreatedDate.ToString(),
                CurrentPricing = customerPricing?.PricingList?.FirstOrDefault()?.AllInPrice.GetValueOrDefault().ToString(),
                AircraftICAO = aircaft?.AFSAircraft?.FirstOrDefault()?.Icao
            };
        }
        
        public async Task<List<AirportWatchLiveDataDto>> GetAirportWatchLiveDataRefactored(int groupId, int fboId, Geolocation.Coordinate coordinate)
        {
            List<AirportWatchLiveDataDto> filteredResult = new List<AirportWatchLiveDataDto>();

            if (fboId == 0) return filteredResult;

            var fbo = await _FboService.GetSingleBySpec(new FboByIdSpecification(fboId));

            if (fbo == null) return filteredResult;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(coordinate, _distance, DistanceUnit.Miles);

            var airportWatchLiveDataMinimumDateTime = DateTime.UtcNow.AddMinutes(-2);

            var fuelOrders = await _FuelReqService.GetUpcomingDirectAndContractOrders(groupId, fboId, true);

            var customerAircrafts = await _customerAircraftsEntityService.GetListBySpec(new CustomerAircraftsByGroupSpecification(groupId));

            var airportWatchLiveData =
                await _AirportWatchLiveDataService.GetListbySpec(
                    new AirportWatchLiveDataSpecification(airportWatchLiveDataMinimumDateTime, DateTime.UtcNow));

            var airportWatchDataWithCustomers = (from awhd in airportWatchLiveData
                                                 join ca in customerAircrafts on awhd.TailNumber equals ca.TailNumber
                     into leftJoinedCustomers
                 from ca in leftJoinedCustomers.DefaultIfEmpty()
                 select new { awhd, ca })
                .Where(x => x.awhd.AircraftPositionDateTimeUtc >= airportWatchLiveDataMinimumDateTime)
                .Where(x =>
                !(x.awhd.Latitude < boundaries.MinLatitude || x.awhd.Latitude > boundaries.MaxLatitude || x.awhd.Longitude < boundaries.MinLongitude ||
                x.awhd.Longitude > boundaries.MaxLongitude))
                .OrderBy(x => x.awhd.AircraftPositionDateTimeUtc)
                .ThenBy(x => x.awhd.AircraftHexCode)
                .ThenBy(x => x.awhd.AtcFlightNumber)
                .ThenBy(x => x.awhd.GpsAltitude);


            filteredResult = (from fr in airportWatchDataWithCustomers
                              join fo in fuelOrders on fr.awhd.TailNumber equals (fo.TailNumber) into fos
                              from fo in fos.DefaultIfEmpty()
                              where GeoCalculator.GetDistance(coordinate.Latitude, coordinate.Longitude, fr.awhd.Latitude, fr.awhd.Longitude, 1, DistanceUnit.Miles) <= _distance
                              select new AirportWatchLiveDataDto
                              {
                                  Oid = fr.awhd.Oid,
                                  Longitude = fr.awhd.Longitude,
                                  Latitude = fr.awhd.Latitude,
                                  GpsAltitude = fr.awhd.GpsAltitude,
                                  GroundSpeedKts = fr.awhd.GroundSpeedKts,
                                  IsAircraftOnGround = fr.awhd.IsAircraftOnGround,
                                  TrackingDegree = fr.awhd.TrackingDegree,
                                  VerticalSpeedKts = fr.awhd.VerticalSpeedKts,
                                  TransponderCode = fr.awhd.TransponderCode,
                                  AtcFlightNumber = fr.awhd.AtcFlightNumber,
                                  AircraftHexCode = fr.awhd.AircraftHexCode,
                                  BoxName = fr.awhd.BoxName,
                                  BoxTransmissionDateTimeUtc = fr.awhd.BoxTransmissionDateTimeUtc,
                                  AircraftPositionDateTimeUtc = fr.awhd.AircraftPositionDateTimeUtc,
                                  AircraftTypeCode = fr.awhd.AircraftTypeCode,
                                  AltitudeInStandardPressure = fr.awhd.AltitudeInStandardPressure,
                                  FuelOrder = fo,
                                  IsInNetwork = fr.ca.IsInNetwork(),
                                  IsOutOfNetwork = fr.ca.IsOutOfNetwork(),
                                  IsActiveFuelRelease = fo.IsActiveFuelRelease(),
                                  IsFuelerLinxClient = fr.ca.IsFuelerLinxClient(),
                                  IsFuelerLinxCustomer = fr.ca.isFuelerLinxCustomer(),
                                  TailNumber = fr.awhd.TailNumber
                              })
                              .ToList();

            AddDemoDataToAirportWatchResult(filteredResult, fboId);
            await SetAircraftICAO(filteredResult);

            return filteredResult;
        }

        private async Task SetAircraftICAO(List<AirportWatchLiveDataDto> result)
        {
            List<int> aircraftIds = result.Where(x => x.AircraftId != null).Select(x => x.AircraftId.Value).Distinct().ToList();
            List<AFSAircraft> afsAircrafts = await _AFSAircraftEntityService.GetListBySpec(new AFSAircraftSpecification(aircraftIds));
            foreach (var airportWatchLiveData in result)
            {
                AFSAircraft afsAircraft = afsAircrafts.FirstOrDefault(x => x.DegaAircraftID == airportWatchLiveData.AircraftId);
                if (afsAircraft != null)
                {
                    airportWatchLiveData.AircraftICAO = afsAircraft.Icao;
                }
            }
        }

        public async Task<List<AirportWatchHistoricalDataResponse>> GetArrivalsDepartures(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            //Only retrieve arrival and departure occurrences.  Remove all parking occurrences.
            var historicalData = await GetAircraftsHistoricalDataAssociatedWithFboRefactored(groupId, fboId, request);

            if (historicalData != null && historicalData.Count > 0)
            {
                var icao = historicalData.FirstOrDefault().AirportICAO;

                List<Geolocation.Coordinate> coordinates = new List<Geolocation.Coordinate>();
                var allFboGeoFenceClusters = await _airportFboGeofenceClustersService.GetAllClusters();
                var fbo = await _FboService.GetSingleBySpec(new FboByIdSpecification(fboId));
                var fboGeoFenceCluster = allFboGeoFenceClusters.Where(a => a.Icao == icao && a.AcukwikFBOHandlerID == fbo.AcukwikFBOHandlerId).FirstOrDefault();

                if (fboGeoFenceCluster != null)
                {
                    var fboClusterCoordinates = await _airportFboGeofenceClustersService.GetClusterCoordinatesByClusterId(fboGeoFenceCluster.Oid);
                    foreach (var clusterCoordinate in fboClusterCoordinates)
                    {
                        Geolocation.Coordinate coordinate = new Geolocation.Coordinate();
                        coordinate.Latitude = clusterCoordinate.Latitude;
                        coordinate.Longitude = clusterCoordinate.Longitude;
                        coordinates.Add(coordinate);
                    }
                }

                var customerVisitsData = historicalData
                    .GroupBy(ah => new { ah.CustomerId, ah.AirportICAO, ah.AircraftHexCode, ah.AtcFlightNumber })
                    .Select(g =>
                    {
                        var latest = g
                            .OrderByDescending(ah => ah.AircraftPositionDateTimeUtc).First();

                        var pastVisits = g
                           .Where(ah => ah.AircraftStatus == AircraftStatusType.Parking).ToList();

                        var visitsToMyFbo = new List<FboHistoricalDataModel>();
                        if (coordinates.Count > 0)
                            visitsToMyFbo = pastVisits.Where(p => FBOLinx.Core.Utilities.Geography.LocationHelper.IsPointInPolygon(new Geolocation.Coordinate(p.Latitude, p.Longitude), coordinates.ToArray())).ToList();

                        return new AirportWatchHistoricalDataResponse
                        {
                            CustomerInfoByGroupID = latest.CustomerInfoByGroupID,
                            CompanyId = latest.CustomerId,
                            Company = latest.Company,
                            DateTime = latest.AircraftPositionDateTimeUtc,
                            TailNumber = latest.TailNumber,
                            FlightNumber = latest.AtcFlightNumber,
                            HexCode = latest.AircraftHexCode,
                            AircraftType = string.IsNullOrEmpty(latest.Make) ? null : latest.Make + " / " + latest.Model,
                            Status = latest.AircraftStatusDescription,
                            PastVisits = pastVisits.Count(),
                            AirportIcao = latest.AirportICAO,
                            AircraftTypeCode = latest.AircraftTypeCode,
                            VisitsToMyFbo = visitsToMyFbo.Count(),
                            PercentOfVisits = visitsToMyFbo.Count > 0 ? (double)(visitsToMyFbo.Count() / (double)pastVisits.Count()) : 0
                        };
                    })
                    .ToList();

                historicalData?.RemoveAll(x => x.AircraftStatus == AircraftStatusType.Parking);

                var result = (from h in historicalData
                              join cv in customerVisitsData on new { h.CustomerId, h.AirportICAO, h.AircraftHexCode, h.AtcFlightNumber } equals new { CustomerId = cv.CompanyId, AirportICAO = cv.AirportIcao, AircraftHexCode = cv.HexCode, AtcFlightNumber = cv.FlightNumber }
                              into leftJoinedCV
                              from cv in leftJoinedCV.DefaultIfEmpty()
                              select new AirportWatchHistoricalDataResponse
                              {
                                  CustomerInfoByGroupID = h.CustomerInfoByGroupID,
                                  CompanyId = h.CustomerId,
                                  Company = h.Company,
                                  DateTime = h.AircraftPositionDateTimeUtc,
                                  TailNumber = h.TailNumber,
                                  FlightNumber = h.AtcFlightNumber,
                                  HexCode = h.AircraftHexCode,
                                  AircraftType = string.IsNullOrEmpty(h.Make) ? null : h.Make + " / " + h.Model,
                                  Status = h.AircraftStatusDescription,
                                  AirportIcao = h.AirportICAO,
                                  AircraftTypeCode = h.AircraftTypeCode,
                                  PastVisits = cv == null ? null : cv.PastVisits,
                                  VisitsToMyFbo = cv == null ? null : cv.VisitsToMyFbo,
                                  PercentOfVisits = cv == null ? null : cv.PercentOfVisits
                              }).ToList();

                return result;
            }

            return new List<AirportWatchHistoricalDataResponse>();
        }
        public async Task<List<AirportWatchHistoricalDataResponse>> GetArrivalsDeparturesRefactored(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            //Only retrieve arrival and departure occurrences.  Remove all parking occurrences.
            var historicalData = await GetAircraftsHistoricalDataAssociatedWithFboRefactored(groupId, fboId, request);

            if (historicalData == null || historicalData.Count() == 0) return new List<AirportWatchHistoricalDataResponse>();

            var icao = historicalData.FirstOrDefault().AirportICAO;

            List<Geolocation.Coordinate> coordinates = new List<Geolocation.Coordinate>();
            var allFboGeoFenceClusters = await _airportFboGeofenceClustersService.GetAllClusters();
            var fbo = await _FboService.GetSingleBySpec(new FboByIdSpecification(fboId));
            var fboGeoFenceCluster = allFboGeoFenceClusters.Where(a => a.Icao == icao && a.AcukwikFBOHandlerID == fbo.AcukwikFBOHandlerId).FirstOrDefault();

            if (fboGeoFenceCluster != null)
            {
                var fboClusterCoordinates = await _airportFboGeofenceClustersService.GetClusterCoordinatesByClusterIdIQueryable(fboGeoFenceCluster.Oid);
                foreach (var clusterCoordinate in fboClusterCoordinates)
                {
                    Geolocation.Coordinate coordinate = new Geolocation.Coordinate();
                    coordinate.Latitude = clusterCoordinate.Latitude;
                    coordinate.Longitude = clusterCoordinate.Longitude;
                    coordinates.Add(coordinate);
                }
            }
            var customerVisitsData = new List<AirportWatchHistoricalDataResponse>();

            customerVisitsData = historicalData
               .GroupBy(ah => new { ah.CustomerId, ah.AirportICAO, ah.AircraftHexCode, ah.AtcFlightNumber })
               .Select(g =>
               {
                   var latest = g
                       .OrderByDescending(ah => ah.AircraftPositionDateTimeUtc).First();

                   var pastVisits = g
                       .Where(ah => ah.AircraftStatus == AircraftStatusType.Parking);

                   var visitsToMyFboCount = (coordinates.Count > 0) ? pastVisits.Where(p => FBOLinx.Core.Utilities.Geography.LocationHelper.IsPointInPolygon(new Geolocation.Coordinate(p.Latitude, p.Longitude), coordinates.ToArray())).Count() : 0;


                   return new AirportWatchHistoricalDataResponse
                   {
                       CustomerInfoByGroupID = latest.CustomerInfoByGroupID,
                       CompanyId = latest.CustomerId,
                       Company = latest.Company,
                       DateTime = latest.AircraftPositionDateTimeUtc,
                       TailNumber = latest.TailNumber,
                       FlightNumber = latest.AtcFlightNumber,
                       HexCode = latest.AircraftHexCode,
                       AircraftType = string.IsNullOrEmpty(latest.Make) ? null : latest.Make + " / " + latest.Model,
                       Status = latest.AircraftStatusDescription,
                       PastVisits = pastVisits.Count(),
                       AirportIcao = latest.AirportICAO,
                       AircraftTypeCode = latest.AircraftTypeCode,
                       VisitsToMyFbo = visitsToMyFboCount,
                       PercentOfVisits = visitsToMyFboCount > 0 ? (double)(visitsToMyFboCount / (double)pastVisits.Count()) : 0
                   };
               })
               .ToList();

            historicalData?.RemoveAll(x => x.AircraftStatus == AircraftStatusType.Parking);

            var result = (from h in historicalData
                      join cv in customerVisitsData on new { h.CustomerId, h.AirportICAO, h.AircraftHexCode, h.AtcFlightNumber } equals new { CustomerId = cv.CompanyId, AirportICAO = cv.AirportIcao, AircraftHexCode = cv.HexCode, AtcFlightNumber = cv.FlightNumber }
                      into leftJoinedCV
                      from cv in leftJoinedCV.DefaultIfEmpty()
                      select new AirportWatchHistoricalDataResponse
                      {
                          CustomerInfoByGroupID = h.CustomerInfoByGroupID,
                          CompanyId = h.CustomerId,
                          Company = h.Company,
                          DateTime = h.AircraftPositionDateTimeUtc,
                          TailNumber = h.TailNumber,
                          FlightNumber = h.AtcFlightNumber,
                          HexCode = h.AircraftHexCode,
                          AircraftType = string.IsNullOrEmpty(h.Make) ? null : h.Make + " / " + h.Model,
                          Status = h.AircraftStatusDescription,
                          AirportIcao = h.AirportICAO,
                          AircraftTypeCode = h.AircraftTypeCode,
                          PastVisits = cv == null ? null : cv.PastVisits,
                          VisitsToMyFbo = cv == null ? null : cv.VisitsToMyFbo,
                          PercentOfVisits = cv == null ? null : cv.PercentOfVisits
                      }).ToList();

            return result;
        }
        public async Task<List<AirportWatchHistoricalDataResponse>> GetVisits(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            var historicalData = await GetAircraftsHistoricalDataAssociatedWithFboRefactored(groupId, fboId, request);

            var noCustomerData = historicalData
                .Where(h => h.Company == null)
                .Select(h => new AirportWatchHistoricalDataResponse
                {
                    CustomerInfoByGroupID = h.CustomerInfoByGroupID,
                    CompanyId = h.CustomerId,
                    Company = h.Company,
                    DateTime = h.AircraftPositionDateTimeUtc,
                    TailNumber = h.TailNumber,
                    FlightNumber = h.AtcFlightNumber,
                    HexCode = h.AircraftHexCode,
                    AircraftType = string.IsNullOrEmpty(h.Make) ? null : h.Make + " / " + h.Model,
                    Status = h.AircraftStatusDescription,
                    AirportIcao = h.AirportICAO,
                    AircraftTypeCode = h.AircraftTypeCode
                })
                .ToList();

            var customerData = historicalData
                .Where(h => h.Company != null)
                .GroupBy(ah => new { ah.CustomerId, ah.AirportICAO, ah.AircraftHexCode, ah.AtcFlightNumber })
                .Select(g =>
                {
                    var latest = g
                        .OrderByDescending(ah => ah.AircraftPositionDateTimeUtc).First();

                    var pastVisits = g
                        .Where(ah => ah.AircraftStatus == AircraftStatusType.Landing);

                    return new AirportWatchHistoricalDataResponse
                    {
                        CustomerInfoByGroupID = latest.CustomerInfoByGroupID,
                        CompanyId = latest.CustomerId,
                        Company = latest.Company,
                        DateTime = latest.AircraftPositionDateTimeUtc,
                        TailNumber = latest.TailNumber,
                        FlightNumber = latest.AtcFlightNumber,
                        HexCode = latest.AircraftHexCode,
                        AircraftType = string.IsNullOrEmpty(latest.Make) ? null : latest.Make + " / " + latest.Model,
                        Status = latest.AircraftStatusDescription,
                        PastVisits = pastVisits.Count(),
                        AirportIcao = latest.AirportICAO,
                        AircraftTypeCode = latest.AircraftTypeCode
                    };
                })
                .ToList();

            return noCustomerData.Concat(customerData).OrderByDescending(h => h.DateTime).ToList();
        }

        public async Task ProcessAirportWatchData(List<AirportWatchLiveDataDto> data, bool isTesting = false)
        {
            _LiveDataToUpdate = new List<AirportWatchLiveDataDto>();
            _LiveDataToInsert = new List<AirportWatchLiveDataDto>();
            _LiveDataToDelete = new List<AirportWatchLiveDataDto>();
            _HistoricalDataToUpdate = new List<AirportWatchHistoricalDataDto>();
            _HistoricalDataToInsert = new List<AirportWatchHistoricalDataDto>();



            data = data.OrderByDescending(row => row.AircraftPositionDateTimeUtc)
               .GroupBy(row => new { row.AircraftHexCode, row.BoxName })
               .Select(grouped => grouped.First()).ToList();


            //await using var fboLinxContext = _ServiceProvider.GetService<FboLinxContext>();
            //await using var degaContext = _ServiceProvider.GetService<DegaContext>();

            // TEST DATA
            //data.Clear();
            //var testData = Newtonsoft.Json.JsonConvert.DeserializeObject<AirportWatchLiveDataDto>("{\"BoxTransmissionDateTimeUtc\":\"2022-07-22T20:12:03\",\"AtcFlightNumber\":\"N118AT\",\"AltitudeInStandardPressure\":0,\"GroundSpeedKts\":7,\"TrackingDegree\":17.0,\"Latitude\":33.57487,\"Longitude\":-117.12913,\"VerticalSpeedKts\":0,\"TransponderCode\":1200,\"BoxName\":\"krbk_a01\",\"AircraftPositionDateTimeUtc\":\"2022-07-22T20:12:03\",\"AircraftTypeCode\":\"A1\",\"GpsAltitude\":-75,\"IsAircraftOnGround\":true,\"FuelOrder\":null,\"IsFuelerLinxCustomer\":null,\"AircraftHexCode\":\"A049FD\",\"TailNumber\":null,\"Oid\":0}");
            //var newAircraftWatchLiveData = new AirportWatchLiveDataDto();
            //newAircraftWatchLiveData = (AirportWatchLiveDataDto)testData;
            //data.Add(newAircraftWatchLiveData);
            //testData = Newtonsoft.Json.JsonConvert.DeserializeObject<AirportWatchLiveDataDto>("{\"BoxTransmissionDateTimeUtc\":\"2022-07-22T20:16:48\",\"AtcFlightNumber\":\"N118AT\",\"AltitudeInStandardPressure\":2100,\"GroundSpeedKts\":89,\"TrackingDegree\":195.0,\"Latitude\":33.56058,\"Longitude\":-117.13237,\"VerticalSpeedKts\":1664,\"TransponderCode\":1200,\"BoxName\":\"krbk_a01\",\"AircraftPositionDateTimeUtc\":\"2022-07-22T20:16:48\",\"AircraftTypeCode\":\"A1\",\"GpsAltitude\":2100,\"IsAircraftOnGround\":false,\"FuelOrder\":null,\"IsFuelerLinxCustomer\":null,\"AircraftHexCode\":\"A049FD\",\"TailNumber\":null,\"Oid\":0}");
            //newAircraftWatchLiveData = new AirportWatchLiveDataDto();
            //newAircraftWatchLiveData = (AirportWatchLiveDataDto)testData;
            //data.Add(newAircraftWatchLiveData);

            //Grab distinct aircraft for this set of data
            //var distinctAircraftHexCodes =
            //    data.Where(x => !string.IsNullOrEmpty(x.AircraftHexCode)).Select(x => x.AircraftHexCode).Distinct().ToList();
            var distinctHexCodes = data.Where(x => !string.IsNullOrEmpty(x.AircraftHexCode))
                .Select(x => x.AircraftHexCode).Distinct().ToList();

            //Preload the collection of past records from the last 7 days to use in the loop
            var oldAirportWatchLiveDataCollection = await GetAirportWatchLiveDataFromDatabase(distinctHexCodes, DateTime.UtcNow.AddHours(-1));
            
            // TEST DATA
            //testData = Newtonsoft.Json.JsonConvert.DeserializeObject<AirportWatchLiveDataDto>("{\"BoxTransmissionDateTimeUtc\":\"2022-07-22T20:11:51\",\"AtcFlightNumber\":\"N118AT\",\"AltitudeInStandardPressure\":1300,\"GroundSpeedKts\":2,\"TrackingDegree\":298.0,\"Latitude\":33.57458,\"Longitude\":-117.12912,\"VerticalSpeedKts\":0,\"TransponderCode\":1200,\"BoxName\":\"krbk_a01\",\"AircraftPositionDateTimeUtc\":\"2022-07-22T20:11:51\",\"AircraftTypeCode\":\"A1\",\"GpsAltitude\":1225,\"IsAircraftOnGround\":false,\"FuelOrder\":null,\"IsFuelerLinxCustomer\":null,\"AircraftHexCode\":\"A049FD\",\"TailNumber\":\"N118AT\",\"Oid\":29998562}");
            //newAircraftWatchLiveData = new AirportWatchLiveDataDto();
            //newAircraftWatchLiveData = (AirportWatchLiveDataDto)testData;
            //oldAirportWatchLiveDataCollection.Add(newAircraftWatchLiveData);

            var oldAirportWatchHistoricalDataCollection = await GetAirportWatchHistoricalDataFromDatabase(distinctHexCodes, DateTime.UtcNow.AddHours(-4));

            var airportWatchDistinctBoxes = await _AirportWatchDistinctBoxesService.GetAllAirportWatchDistinctBoxes();

            foreach (var record in data)
            {
                var aircraftOldAirportWatchLiveDataCollection = oldAirportWatchLiveDataCollection
                    .Where(aw => aw.AircraftHexCode == record.AircraftHexCode).OrderByDescending(a => a.BoxTransmissionDateTimeUtc).ToList();

                var oldAirportWatchLiveData = aircraftOldAirportWatchLiveDataCollection.FirstOrDefault(a => !_LiveDataToUpdate.Any(d => d.Oid == a.Oid) && !_LiveDataToDelete.Any(l => l.Oid == a.Oid));

                var oldAirportWatchHistoricalData = oldAirportWatchHistoricalDataCollection
                    .Where(aw => aw.AircraftHexCode == record.AircraftHexCode)
                    .OrderByDescending(aw => aw.AircraftPositionDateTimeUtc)
                    .FirstOrDefault();

                var airportWatchHistoricalData = AirportWatchHistoricalDataDto.Cast(record);

                double distance = 0;
                double airportLatitude = 0;
                double airportLongitude = 0;
                var airportWatchDistinctBox = airportWatchDistinctBoxes.Where(a => a.BoxName == record.BoxName).FirstOrDefault();
                if (airportWatchDistinctBox != null && airportWatchDistinctBox.Latitude != "" && airportWatchDistinctBox.Longitude != "")
                {
                    airportLatitude = LocationHelper.GetLatitudeGeoLocationFromGPS(airportWatchDistinctBox.Latitude);
                    airportLongitude = LocationHelper.GetLongitudeGeoLocationFromGPS(airportWatchDistinctBox.Longitude);

                    distance = new Coordinates(airportLatitude, airportLongitude).DistanceTo(
                     new Coordinates(record.Latitude, record.Longitude),
                     UnitOfLength.NauticalMiles
                 );
                }
                else
                    return;

                if (distance > 350)
                    return;

                //Record historical status

                //Compare our last "live" record with the new one to determine if the aircraft is taking off or landing

                //if (record.BoxName == "krbk_a01")
                //{
                //_LoggingService.LogError("krbk_a01", Newtonsoft.Json.JsonConvert.SerializeObject(record), LogLevel.Info, LogColorCode.Blue);
                //}

                //Next check if the last live record we have for the aircraft had a different IsAircraftOnGround state than what we see now, if the aircraft was in range of the box

                if (oldAirportWatchLiveData != null &&
                oldAirportWatchLiveData.IsAircraftOnGround != record.IsAircraftOnGround && oldAirportWatchLiveData.AircraftPositionDateTimeUtc > DateTime.UtcNow.AddMinutes(-5))
                {
                    _HistoricalDataToInsert.Add(airportWatchHistoricalData);
                }
                //Finally go through the conditions that make this a valid parking occurrence
                else
                {
                    AddPossibleParkingOccurrence(oldAirportWatchHistoricalData, airportWatchHistoricalData);
                }


                //Record live-view data and new flight/tail combinations
                if (oldAirportWatchLiveData == null)
                {
                    var existingLiveDataList = _LiveDataToInsert.Where(l => l.AircraftHexCode == record.AircraftHexCode).ToList();
                    
                    if (existingLiveDataList.Count > 0)
                    {
                        foreach (var existingLiveData in existingLiveDataList)
                        {
                            if (existingLiveData.BoxName != record.BoxName)
                            {
                                var distance2 = new Coordinates(airportLatitude, airportLongitude).DistanceTo(
                                     new Coordinates(existingLiveData.Latitude, existingLiveData.Longitude),
                                     UnitOfLength.NauticalMiles
                                );

                                if (distance2 < distance)
                                {
                                    _LiveDataToInsert.Add(record);
                                    _LiveDataToInsert.Remove(existingLiveData);
                                }
                            }
                        }
                    }
                    else
                        _LiveDataToInsert.Add(record);
                }
                else
                {
                    //Capture the tail from the previous record before copying the new information to prevent needing to lookup the tail again
                    record.TailNumber = oldAirportWatchLiveData.TailNumber;
                    record.Oid = oldAirportWatchLiveData.Oid;
                    _LiveDataToUpdate.Add(record);
                    aircraftOldAirportWatchLiveDataCollection.Remove(oldAirportWatchLiveData);

                    if (aircraftOldAirportWatchLiveDataCollection.Count > 1)
                    {
                        _LiveDataToDelete.AddRange(aircraftOldAirportWatchLiveDataCollection);
                    }
                }
            }

            await PrepareRecordsForDatabase();

            if (!isTesting)
                await CommitChanges();
        }

        private async Task PrepareRecordsForDatabase()
        {
            //[#2ht0dek] Reverting back changes for this branch and upgrading EFCore and BulkExtensions to test performance
            //Set the nearest airport for all records that will be recorded for historical statuses
            foreach (var airportWatchHistoricalDataDto in _HistoricalDataToInsert)
            {
                var nearestAirport = await _AirportService.GetNearestAirportPosition(airportWatchHistoricalDataDto.Latitude, airportWatchHistoricalDataDto.Longitude);
                airportWatchHistoricalDataDto.AirportICAO = nearestAirport?.GetProperAirportIdentifier();
            }

            foreach (var airportWatchHistoricalDataDto in _HistoricalDataToUpdate)
            {
                var nearestAirport = await _AirportService.GetNearestAirportPosition(airportWatchHistoricalDataDto.Latitude, airportWatchHistoricalDataDto.Longitude);
                airportWatchHistoricalDataDto.AirportICAO = nearestAirport?.GetProperAirportIdentifier();
            }

            _HistoricalDataToInsert = _HistoricalDataToInsert.Where(record => {
                if (string.IsNullOrEmpty(record.AirportICAO) || string.IsNullOrEmpty(record.BoxName)) return false;
                //return record.BoxName.ToLower().StartsWith(record.AirportICAO.ToLower());
                return true;
            }).ToList();
            _HistoricalDataToUpdate = _HistoricalDataToUpdate.Where(record => {
                if (string.IsNullOrEmpty(record.AirportICAO) || string.IsNullOrEmpty(record.BoxName)) return false;
                //return record.BoxName.ToLower().StartsWith(record.AirportICAO.ToLower());
                return true;
            }).ToList();

            await SetTailNumber(_HistoricalDataToInsert);
            await SetTailNumber(_LiveDataToInsert);
            //[#2ht0dek] Commenting out changes made in this branch to see if performance is restored with an upgrade to EFCore and BulkExtensions

            ////Set the nearest airport for all records that will be recorded for historical statuses
            //_HistoricalDataToInsert.ForEach(x =>
            //{
            //    x.AirportICAO = GetNearestICAO(airportPositions, x.Latitude, x.Longitude);
            //});
            //_HistoricalDataToUpdate.ForEach(x =>
            //{
            //    x.AirportICAO = GetNearestICAO(airportPositions, x.Latitude, x.Longitude);
            //});

            //_LiveDataToUpdate = _LiveDataToUpdate.OrderByDescending(x => x.AircraftPositionDateTimeUtc)
            //    .GroupBy(x => x.Id)
            //    .Select(x => x.First())
            //    .ToList();

            //_LiveDataToDelete = _LiveDataToDelete.OrderByDescending(x => x.AircraftPositionDateTimeUtc).GroupBy(x => x.Id)
            //    .Select(x => x.First()).ToList();

            //_HistoricalDataToUpdate = _HistoricalDataToUpdate.OrderByDescending(x => x.AircraftPositionDateTimeUtc)
            //    .GroupBy(x => x.Id)
            //    .Select(x => x.First())
            //    .Where(x =>
            //    {
            //        if (string.IsNullOrEmpty(x?.AirportICAO) || string.IsNullOrEmpty(x?.BoxName)) return false;
            //        return x.BoxName.ToLower().StartsWith(x.AirportICAO.ToLower());
            //    })
            //    .ToList();

            //_HistoricalDataToInsert = _HistoricalDataToInsert.Where(record =>
            //{
            //    if (string.IsNullOrEmpty(record.AirportICAO) || string.IsNullOrEmpty(record.BoxName)) return false;
            //    return record.BoxName.ToLower().StartsWith(record.AirportICAO.ToLower());
            //}).ToList();

            //await SetTailNumber(_HistoricalDataToInsert);
            //await SetTailNumber(_LiveDataToInsert);

            ////Merge the insert data for historical information into the update collection so we can do a BulkInsertUpdate operation
            //_HistoricalDataToUpdate.AddRange(_HistoricalDataToInsert);
        }

        public async Task<List<FboHistoricalDataModel>> GetHistoricalDataAssociatedWithGroupOrFboRefactored(int groupId, int? fboId, AirportWatchHistoricalDataRequest request)
        {
            var result = await _AirportWatchHistoricalDataService.GetHistoricalDataWithCustomerAndAircraftInfo(groupId,
                fboId, request.StartDateTime.GetValueOrDefault(), request.EndDateTime.GetValueOrDefault(), request.FlightOrTailNumbers);

            return result;
        }
        
        public async Task<List<FboHistoricalDataModel>> GetAircraftsHistoricalDataAssociatedWithFboRefactored(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            var historicalData = await GetHistoricalDataAssociatedWithGroupOrFboRefactored(groupId, fboId, request);
            return historicalData;
        }

        public async Task<List<AcukwikAirport>> GetAirportsWithAntennaData()
        {
            try
            {
                //Fetch all airports with antenna data from the last two hours
                var pastWeekDateTime = DateTime.UtcNow.Add(new TimeSpan(-1, 0, 0, 0));
                var allAirportWatchDistinctBoxes = await _AirportWatchDistinctBoxesService.GetAllAirportWatchDistinctBoxes();
                var distinctBoxAirports = allAirportWatchDistinctBoxes.Where(x => !string.IsNullOrEmpty(x.AirportICAO)).Select(x => x.AirportICAO.ToUpper()).ToList();

                //Fetch distinct airports from clusters or that were added manually
                var distinctAirportIdentifiersWithClusters = await _airportFboGeofenceClustersService.GetDistinctAirportIdentifiersWithClusters();
                distinctBoxAirports.AddRange(distinctAirportIdentifiersWithClusters);

                var airports = await _AirportService.GetAirportsByAirportIdentifier(distinctBoxAirports);
                return airports;
            }
            catch (System.Exception exception)
            {
                Debug.WriteLine("Error in AirportWatchService.GetAirportsWithAntennaData: " + exception.Message);
                return new List<AcukwikAirport>();
            }
        }

        public async Task<List<AirportWatchAntennaStatusGrid>> GetAntennaStatusData()
        {
            try
            {
                var distinctBoxes = await GetDistinctAntennaBoxes();

                var fbos = await (from f in _context.Fbos where f.GroupId > 1 select new { Fbo = f.Fbo, AntennaName = f.AntennaName }).ToListAsync();
                foreach (AirportWatchAntennaStatusGrid distinctBox in distinctBoxes)
                {
                    var fbo = fbos.Where(f => f.AntennaName != null && f.AntennaName.ToLower().Trim() == distinctBox.BoxName.ToLower().Trim()).FirstOrDefault();
                    if (fbo != null && fbo.Fbo != null)
                        distinctBox.FbolinxAccount = fbo.Fbo;
                }

                return distinctBoxes.OrderBy(d => d.BoxName).ToList();

            }
            catch (System.Exception exception)
            {
                Debug.WriteLine("Error in AirportWatchService.GetAntennaStatusData: " + exception.Message);
                return new List<AirportWatchAntennaStatusGrid>();
            }
        }

        public async Task<List<string>> GetDistinctUnassignedAntennaBoxes(string fboAntenna)
        {
            var distinctBoxes = await GetDistinctAntennaBoxes();
            var fboAntennas = await (from f in _context.Fbos where f.GroupId > 1 select f.AntennaName).ToListAsync();
            var distinctUnassignedBoxes = distinctBoxes.Where(d => !fboAntennas.Contains(d.BoxName)).Select(f => f.BoxName).ToList();

            if (fboAntenna != "none")
                distinctUnassignedBoxes.Add(fboAntenna);

            return distinctUnassignedBoxes.OrderBy(d => d).ToList();
        }

        public async Task<List<AirportWatchHistoricalDataDto>> GetParkingOccurencesByAirport(string icao,
            DateTime startDateTimeUtc, DateTime endDateTimeUtc)
        {
            try
            {
                var historicalData = await _AirportWatchHistoricalDataService.GetListbySpec(
                    new AirportWatchHistoricalDataByIcaoSpecification(icao, startDateTimeUtc, endDateTimeUtc));
                    

                var occurrences = historicalData.Where(x =>
                    x.AircraftStatus == AircraftStatusType.Parking).ToList();
                return occurrences;
            }
            catch (System.Exception exception)
            {
                Debug.WriteLine("Error in AirportWatchService.GetParkingOccurencesByAirport: " + exception.Message);
                return new List<AirportWatchHistoricalDataDto>();
            }
        }

        public async Task<List<AircraftLocation>> GetAircraftLocations(int fuelerlinxCustomerId)
        {
            var aircrafts = await _customerAircraftsEntityService.GetAircraftLocations(fuelerlinxCustomerId);
            var aircraftCoordinates = await _AirportWatchLiveDataEntityService.GetListBySpec(new AirportWatchLiveDataByTailNumberSpecification(aircrafts.Select(x => x.TailNumber).ToList(), DateTime.UtcNow.AddDays(-7)));
            foreach (IGrouping<string, AirportWatchLiveData> grouping in aircraftCoordinates.GroupBy(x => x.TailNumber))
            {
                var latestAircraftCoordinate = grouping.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                if (latestAircraftCoordinate != null)
                {
                    var aircraft = aircrafts.FirstOrDefault(x => x.TailNumber == latestAircraftCoordinate.TailNumber);
                    if (aircraft != null)
                    {
                        aircraft.Latitude = latestAircraftCoordinate.Latitude;
                        aircraft.Longitude = latestAircraftCoordinate.Longitude;
                    }
                }
            }

            var missingAircrafts = aircrafts.Where(x => x.Latitude == null && x.Longitude == null).ToList();
            if (missingAircrafts.Any())
            {
                var latestSWIMRecords = await _SWIMFlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(missingAircrafts.Select(x => x.TailNumber).ToList(), DateTime.UtcNow.AddDays(-7), false));
                if (latestSWIMRecords.Any())
                {
                    foreach (IGrouping<string, SWIMFlightLeg> grouping in latestSWIMRecords.GroupBy(x => x.AircraftIdentification))
                    {
                        var latestSWIMFlightRecord = grouping.OrderByDescending(x => x.Oid).FirstOrDefault();
                        if (latestSWIMFlightRecord != null)
                        {
                            var aircraft = aircrafts.FirstOrDefault(x => x.TailNumber == latestSWIMFlightRecord.AircraftIdentification);
                            if (aircraft != null)
                            {
                                aircraft.Latitude = latestSWIMFlightRecord.Latitude;
                                aircraft.Longitude = latestSWIMFlightRecord.Longitude;
                            }
                        }
                    }
                }
            }
            
            return aircrafts;
        }
        
        public async Task GetAirportWatchTestData()
        {
            var pastTenMinutes = DateTime.UtcNow.Add(new TimeSpan(0, -10, 0));

            var aircraftWatchLiveData = await _AirportWatchLiveDataService.GetListbySpec(new AirportWatchLiveDataSpecification(pastTenMinutes, DateTime.UtcNow));
            await ProcessAirportWatchData(aircraftWatchLiveData, true);
        }

        private async Task<List<AirportWatchAntennaStatusGrid>> GetDistinctAntennaBoxes()
        {
            var allDistinctBoxes = await _AirportWatchDistinctBoxesService.GetAllAirportWatchDistinctBoxes();
            var distinctBoxes = new List<AirportWatchAntennaStatusGrid>();

            var pastHalfHour = DateTime.UtcNow.Add(new TimeSpan(0, -30, 0));

            foreach (var distinctBox in allDistinctBoxes)
            {
                var box = new AirportWatchAntennaStatusGrid()
                {
                    BoxName = distinctBox.BoxName,
                    Status = distinctBox.LastLiveDateTime == null || distinctBox.LastLiveDateTime < pastHalfHour ? "Not Active" : "Active",
                    LastUpdateRaw = distinctBox.LastLiveDateTime == null || distinctBox.LastLiveDateTime < pastHalfHour ? "" : distinctBox.LastLiveDateTime.ToString(),
                    LastUpdateCurated = distinctBox.LastHistoricDateTime == null ? "" : distinctBox.LastHistoricDateTime.ToString()
                };
                distinctBoxes.Add(box);
            }

            return distinctBoxes;
        }

        private async Task SetTailNumber(IEnumerable<IBaseAirportWatchModel> airportWatchRecords)
        {
            //TODO: Setup an AircraftHexTailMappingService to handle this
            List<string> aircraftHexCodesToInsert = airportWatchRecords.Select(x => x.AircraftHexCode).Distinct().ToList();
            List<AircraftHexTailMapping> hexTailMappings = await _degaContext.AircraftHexTailMapping.Where(x => aircraftHexCodesToInsert.Contains(x.AircraftHexCode)).AsNoTracking().ToListAsync();
            foreach (IBaseAirportWatchModel airportWatchRecord in airportWatchRecords.ToList())
            {
                airportWatchRecord.TailNumber = hexTailMappings.FirstOrDefault(mapping => mapping.AircraftHexCode == airportWatchRecord.AircraftHexCode)?.TailNumber;
            }
        }

        private void AddDemoDataToAirportWatchResult(List<AirportWatchLiveDataDto> result, int fboId)
        {
            if (_demoData == null || _demoData.Value == null || _demoData.Value.FlightWatch == null)
                return;
            if (_demoData.Value.FlightWatch.FuelOrder.Fboid != fboId)
                return;
            _demoData.Value.FlightWatch.AircraftPositionDateTimeUtc = DateTime.UtcNow.AddSeconds(-5);
            _demoData.Value.FlightWatch.BoxTransmissionDateTimeUtc = DateTime.UtcNow.AddSeconds(-5);
            _demoData.Value.FlightWatch.FuelOrder.Eta = DateTime.UtcNow.AddMinutes(25);
            _demoData.Value.FlightWatch.FuelOrder.Etd = DateTime.UtcNow.AddHours(2);
            result.Add(_demoData.Value.FlightWatch);
        }

        private void AddPossibleParkingOccurrence(AirportWatchHistoricalDataDto oldAirportWatchHistoricalData, AirportWatchHistoricalDataDto airportWatchHistoricalData)
        {
            // Parking occurrences
            //Don't check for parking if we don't know what the aircraft was doing previously
            if (oldAirportWatchHistoricalData == null)
                return;

            // If neither the old record nor the new one are on the ground then it can't be a parking
            if (!oldAirportWatchHistoricalData.IsAircraftOnGround || !airportWatchHistoricalData.IsAircraftOnGround)
                return;

            //First confirm the last record we are comparing with was a landing or a parking
            if (oldAirportWatchHistoricalData.AircraftStatus != AircraftStatusType.Landing &&
                oldAirportWatchHistoricalData.AircraftStatus != AircraftStatusType.Parking)
                return;

            //If the aircraft has not moved then do not update the parking record - we want to keep the old record when it first stopped moving
            if (Math.Abs(airportWatchHistoricalData.Latitude - oldAirportWatchHistoricalData.Latitude) <= 0.000001 ||
                Math.Abs(airportWatchHistoricalData.Longitude - oldAirportWatchHistoricalData.Longitude) <= 0.000001)
                return;
            //Last record was over 10 minutes ago and the aircraft hasn't moved since that point - keep this old record and don't update
            if (oldAirportWatchHistoricalData.AircraftPositionDateTimeUtc < DateTime.UtcNow.AddMinutes(-10))
                return;
            //The aircraft has moved since landing - this should be an updated parking record
            airportWatchHistoricalData.AircraftStatus = AircraftStatusType.Parking;
            var tailNumber = oldAirportWatchHistoricalData.TailNumber;

            if (oldAirportWatchHistoricalData.AircraftStatus == AircraftStatusType.Parking)
            {
                airportWatchHistoricalData.TailNumber = tailNumber;
                airportWatchHistoricalData.Oid = oldAirportWatchHistoricalData.Oid;
                _HistoricalDataToUpdate.Add(airportWatchHistoricalData);
            }
            else
            {
                _HistoricalDataToInsert.Add(airportWatchHistoricalData);
            }

        }

        private async Task CommitChanges()
        {
            var bulkConfig = new BulkConfig() {WithHoldlock = false, BatchSize = 5000};
            
            await _AirportWatchLiveDataService.BulkInsert(_LiveDataToInsert, bulkConfig);
            await _AirportWatchLiveDataService.BulkUpdate(_LiveDataToUpdate, bulkConfig);
            await _AirportWatchLiveDataService.BulkDeleteAsync(_LiveDataToDelete, bulkConfig);
            await _AirportWatchHistoricalDataService.BulkInsert(_HistoricalDataToInsert, bulkConfig);
            await _AirportWatchHistoricalDataService.BulkUpdate(_HistoricalDataToUpdate, bulkConfig);
        }

        private async Task<List<AirportWatchLiveDataDto>> GetAirportWatchLiveDataFromDatabase(List<string> aircraftHexCodes, DateTime aircraftPositionDateTime)
        {
            var result = await _AirportWatchLiveDataService.GetListbySpec(
                new AirportWatchLiveDataByHexCodeSpecification(aircraftHexCodes, aircraftPositionDateTime));
            return result;
        }

        private async Task<List<AirportWatchHistoricalDataDto>> GetAirportWatchHistoricalDataFromDatabase(List<string> aircraftHexCodes, DateTime aircraftPositionDateTime)
        {
            var result = await _AirportWatchHistoricalDataService.GetListbySpec(new AirportWatchHistoricalDataByHexCodeSpecification(aircraftHexCodes, aircraftPositionDateTime));

            return result;
        }
    }
    
}
