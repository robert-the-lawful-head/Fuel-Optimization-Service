using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses.AirportWatch;
using Geolocation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using EFCore.BulkExtensions;
using FBOLinx.DB;
using System.Collections;
using System.Diagnostics;
using FBOLinx.Core.Enums;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using Fuelerlinx.SDK;
using Microsoft.Extensions.Options;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.Service.Mapping.Dto;
using Microsoft.Extensions.Caching.Memory;
using Mapster;
using FBOLinx.ServiceLayer.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.Web.Services
{
    public class AirportWatchService
    {
        private const int _distance = 250;
        private string _AllAirportsPositioningCacheKey = "AirportWatchService_AllAirportsPositioning";

        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private readonly AircraftService _aircraftService;
        private readonly FboService _fboService;
        private List<AirportWatchLiveData> _LiveDataToUpdate;
        private List<AirportWatchLiveData> _LiveDataToInsert;
        private List<AirportWatchLiveData> _LiveDataToDelete;
        private List<AirportWatchHistoricalData> _HistoricalDataToUpdate;
        private List<AirportWatchHistoricalData> _HistoricalDataToInsert;
        private FuelerLinxApiService _fuelerLinxApiService;
        private IOptions<DemoData> _demoData;
        private DBSCANService _dBSCANService;
        private readonly AirportFboGeofenceClustersService _airportFboGeofenceClustersService;
        private readonly FbopricesService _fboPricesService;
        private ICustomerAircraftEntityService _customerAircraftsEntityService;
        private ICustomerInfoByGroupEntityService _customerInfoByGroupEntityService;
        private IMemoryCache _MemoryCache;
        private IServiceProvider _ServiceProvider;

        public AirportWatchService(DBSCANService DBSCANService,
            FboLinxContext context, DegaContext degaContext, AircraftService aircraftService, FboService fboService, FuelerLinxApiService fuelerLinxApiService,
            IOptions<DemoData> demoData, AirportFboGeofenceClustersService airportFboGeofenceClustersService,
            FbopricesService fboPricesService, ICustomerAircraftEntityService customerAircraftsEntityService, ICustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            IMemoryCache memoryCache, IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
            _dBSCANService = DBSCANService;
            _demoData = demoData;
            _context = context;
            _degaContext = degaContext;
            _aircraftService = aircraftService;
            _fboService = fboService;
            _fuelerLinxApiService = fuelerLinxApiService;
            _airportFboGeofenceClustersService = airportFboGeofenceClustersService;
            _fboPricesService = fboPricesService;
            _customerAircraftsEntityService = customerAircraftsEntityService;
            _customerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _MemoryCache = memoryCache;
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

            Models.Responses.PriceLookupResponse customerPricing = null;
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
                LastQuote = pricingLogs == null ? "N/A" : pricingLogs.CreatedDate.ToString(),
                CurrentPricing = customerPricing?.PricingList?.FirstOrDefault()?.AllInPrice.GetValueOrDefault().ToString()
            };
        }
        public async Task<List<AirportWatchLiveData>> GetAirportWatchLiveData(int groupId, int fboId, Coordinate coordinate)
        {
            List<AirportWatchLiveData> filteredResult = new List<AirportWatchLiveData>();

            if (fboId == 0) return filteredResult;
            {
                var fbo = await (from f in _context.Fbos
                                 join fa in _context.Fboairports on f.Oid equals fa.Fboid
                                 where f.Oid == fboId
                                 select new { f, fa }).FirstOrDefaultAsync();

                if (fbo != null)
                {
                    CoordinateBoundaries boundaries = new CoordinateBoundaries(coordinate, _distance, DistanceUnit.Miles);
                    double minLatitude = boundaries.MinLatitude;
                    double maxLatitude = boundaries.MaxLatitude;
                    double minLongitude = boundaries.MinLongitude;
                    double maxLongitude = boundaries.MaxLongitude;

                    var timelimit = DateTime.UtcNow.AddMinutes(-2);

                    using (var scope = new TransactionScope(
                        TransactionScopeOption.Required,
                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                        TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var fuelOrders = await _context.FuelReq
                            .Where(x => x.Fboid == fboId && x.Eta > DateTime.UtcNow)
                            .Include(x => x.CustomerAircraft).ToListAsync();
                        fuelOrders.RemoveAll(x => x.Cancelled == true);

                        FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _fuelerLinxApiService.GetContractFuelRequests(new FBOLinxOrdersRequest()
                        { EndDateTime = DateTime.UtcNow.AddHours(12), StartDateTime = DateTime.UtcNow, Icao = fbo.fa.Icao, Fbo = fbo.f.Fbo });

                        foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result)
                        {
                            FuelReq fuelReq = new FuelReq();
                            fuelReq.Oid = 0;
                            fuelReq.ActualPpg = 0;
                            fuelReq.ActualVolume = transaction.InvoicedVolume.Amount;
                            fuelReq.Archived = transaction.Archived;
                            fuelReq.Cancelled = false;
                            fuelReq.DateCreated = transaction.CreationDate;
                            fuelReq.DispatchNotes = "";
                            fuelReq.Eta = transaction.ArrivalDateTime;
                            fuelReq.Etd = transaction.DepartureDateTime;
                            fuelReq.Icao = transaction.Icao;
                            fuelReq.Notes = "";
                            fuelReq.QuotedPpg = 0;
                            fuelReq.QuotedVolume = transaction.DispatchedVolume.Amount;
                            fuelReq.Source = transaction.FuelVendor;
                            fuelReq.SourceId = transaction.Id;
                            fuelReq.TimeStandard = transaction.TimeStandard.ToString() == "0" ? "Z" : "L";
                            fuelReq.Email = "";
                            fuelReq.PhoneNumber = "";
                            fuelReq.CustomerAircraft = new CustomerAircrafts() { TailNumber = transaction.TailNumber };
                            fuelOrders.Add(fuelReq);
                        }

                        filteredResult = await (from awhd in _context.AirportWatchLiveData
                                                join ca in (
                                                        from ca in _context.CustomerAircrafts
                                                        join cig in _context.CustomerInfoByGroup on new { ca.CustomerId, GroupId = ca.GroupId ?? 0 }
                                                            equals new { cig.CustomerId, cig.GroupId }
                                                        join cu in _context.Customers on cig.CustomerId equals cu.Oid
                                                        where ca.GroupId == groupId
                                                        select new
                                                        {
                                                            ca.GroupId,
                                                            ca.CustomerId,
                                                            ca.TailNumber,
                                                            ca.AircraftId,
                                                            cig.Company,
                                                            CustomerInfoByGroupID = cig.Oid,
                                                            cu.FuelerlinxId
                                                        }
                                                ) on awhd.TailNumber equals ca.TailNumber
                                                    into leftJoinedCustomers
                                                from ca in leftJoinedCustomers.DefaultIfEmpty()
                                                where awhd.AircraftPositionDateTimeUtc >= timelimit
                                                select new AirportWatchLiveData
                                                {
                                                    Oid = awhd.Oid,
                                                    Longitude = awhd.Longitude,
                                                    Latitude = awhd.Latitude,
                                                    GpsAltitude = awhd.GpsAltitude,
                                                    GroundSpeedKts = awhd.GroundSpeedKts,
                                                    IsAircraftOnGround = awhd.IsAircraftOnGround,
                                                    TrackingDegree = awhd.TrackingDegree,
                                                    VerticalSpeedKts = awhd.VerticalSpeedKts,
                                                    TransponderCode = awhd.TransponderCode,
                                                    AtcFlightNumber = awhd.AtcFlightNumber,
                                                    AircraftHexCode = awhd.AircraftHexCode,
                                                    BoxName = awhd.BoxName,
                                                    BoxTransmissionDateTimeUtc = awhd.BoxTransmissionDateTimeUtc,
                                                    AircraftPositionDateTimeUtc = awhd.AircraftPositionDateTimeUtc,
                                                    AircraftTypeCode = awhd.AircraftTypeCode,
                                                    AltitudeInStandardPressure = awhd.AltitudeInStandardPressure,
                                                    IsFuelerLinxCustomer = (ca.FuelerlinxId.HasValue && ca.FuelerlinxId.Value > 0),
                                                    TailNumber = awhd.TailNumber ?? ""
                                                })
                            .OrderBy(x => x.AircraftPositionDateTimeUtc)
                            .ThenBy(x => x.AircraftHexCode)
                            .ThenBy(x => x.AtcFlightNumber)
                            .ThenBy(x => x.GpsAltitude)
                            .ToListAsync(); ;

                        filteredResult.RemoveAll(x =>
                            x.Latitude < minLatitude || x.Latitude > maxLatitude || x.Longitude < minLongitude ||
                            x.Longitude > maxLongitude);

                        filteredResult = (from fr in filteredResult
                                          join fo in fuelOrders on fr.AtcFlightNumber equals (fo.CustomerAircraft == null ? "" : fo.CustomerAircraft.TailNumber) into fos
                                          from fo in fos.DefaultIfEmpty()
                                          where GeoCalculator.GetDistance(coordinate.Latitude, coordinate.Longitude, fr.Latitude, fr.Longitude, 1, Geolocation.DistanceUnit.Miles) <= _distance
                                          select new AirportWatchLiveData
                                          {
                                              Oid = fr.Oid,
                                              Longitude = fr.Longitude,
                                              Latitude = fr.Latitude,
                                              GpsAltitude = fr.GpsAltitude,
                                              GroundSpeedKts = fr.GroundSpeedKts,
                                              IsAircraftOnGround = fr.IsAircraftOnGround,
                                              TrackingDegree = fr.TrackingDegree,
                                              VerticalSpeedKts = fr.VerticalSpeedKts,
                                              TransponderCode = fr.TransponderCode,
                                              AtcFlightNumber = fr.AtcFlightNumber,
                                              AircraftHexCode = fr.AircraftHexCode,
                                              BoxName = fr.BoxName,
                                              BoxTransmissionDateTimeUtc = fr.BoxTransmissionDateTimeUtc,
                                              AircraftPositionDateTimeUtc = fr.AircraftPositionDateTimeUtc,
                                              AircraftTypeCode = fr.AircraftTypeCode,
                                              AltitudeInStandardPressure = fr.AltitudeInStandardPressure,
                                              FuelOrder = fo,
                                              IsFuelerLinxCustomer = fr.IsFuelerLinxCustomer,
                                              TailNumber = fr.TailNumber
                                          })
                                    .ToList();

                        //cAddDemoDataToAirportWatchResult(filteredResult, fboId);

                        scope.Complete();
                    }
                }
            }
            return filteredResult;
        }
        public async Task<List<AirportWatchLiveDataDto>> GetAirportWatchLiveDataRefactored(int groupId, int fboId, Coordinate coordinate)
        {
            List<AirportWatchLiveDataDto> filteredResult = new List<AirportWatchLiveDataDto>();

            if (fboId == 0) return filteredResult;

            var fbo = await _context.Fbos
                .Include(fbo => fbo.fboAirport)
                .Where(fbo => fbo.Oid == fboId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (fbo == null) return filteredResult;

            CoordinateBoundaries boundaries = new CoordinateBoundaries(coordinate, _distance, DistanceUnit.Miles);

            var timelimit = DateTime.UtcNow.AddMinutes(-2);

            var fuelOrders = await _context.FuelReq
                    .Where(x => x.Fboid == fboId && x.Eta > DateTime.UtcNow)
                    .Include(x => x.CustomerAircraft)
                    .Where(x => x.Cancelled != true)
                    .AsNoTracking()
                    .ToListAsync();

            FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _fuelerLinxApiService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = DateTime.UtcNow.AddHours(12), StartDateTime = DateTime.UtcNow, Icao = fbo.fboAirport.Icao, Fbo = fbo.Fbo });

            foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result)
            {
                fuelOrders.Add(new FuelReq() {
                    Oid = 0,
                    ActualPpg = 0,
                    ActualVolume = transaction.InvoicedVolume.Amount,
                    Archived = transaction.Archived,
                    Cancelled = false,
                    DateCreated = transaction.CreationDate,
                    DispatchNotes = "",
                    Eta = transaction.ArrivalDateTime,
                    Etd = transaction.DepartureDateTime,
                    Icao = transaction.Icao,
                    Notes = "",
                    QuotedPpg = 0,
                    QuotedVolume = transaction.DispatchedVolume.Amount,
                    Source = transaction.FuelVendor,
                    SourceId = transaction.Id,
                    TimeStandard = transaction.TimeStandard.ToString() == "0" ? "Z" : "L",
                    Email = "",
                    PhoneNumber = "",
                    CustomerAircraft = new CustomerAircrafts() { TailNumber = transaction.TailNumber },
                });
            }
            var customerAircrafts =
                _context.CustomerAircrafts
                .Include(ca => ca.Customer)
                .Include(ca => ca.Customer.CustomerInfoByGroup)
                .Where(ca => (ca.GroupId ?? 0) == ca.Customer.CustomerInfoByGroup.GroupId)
                .Where(ca => ca.GroupId == groupId)
                .AsNoTracking();

            var airportWatchDataWithCustomers = await
                (from awhd in _context.AirportWatchLiveData
                 join ca in customerAircrafts on awhd.TailNumber equals ca.TailNumber
                     into leftJoinedCustomers
                 from ca in leftJoinedCustomers.DefaultIfEmpty()
                 select new { awhd, ca })
                .Where(x => x.awhd.AircraftPositionDateTimeUtc >= timelimit)
                .Where(x =>
                !(x.awhd.Latitude < boundaries.MinLatitude || x.awhd.Latitude > boundaries.MaxLatitude || x.awhd.Longitude < boundaries.MinLongitude ||
                x.awhd.Longitude > boundaries.MaxLongitude))
                .OrderBy(x => x.awhd.AircraftPositionDateTimeUtc)
                .ThenBy(x => x.awhd.AircraftHexCode)
                .ThenBy(x => x.awhd.AtcFlightNumber)
                .ThenBy(x => x.awhd.GpsAltitude)
                .AsNoTracking()
                .ToListAsync();


            filteredResult = (from fr in airportWatchDataWithCustomers
                              join fo in fuelOrders on fr.awhd.AtcFlightNumber equals (fo.CustomerAircraft == null ? "" : fo.CustomerAircraft.TailNumber) into fos
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
                                  FuelOrder = fo.Adapt<FuelReqDto>(),
                                  IsInNetwork = (fr.ca?.Customer?.CompanyByGroup?.Oid > 0),
                                  IsFuelerLinxCustomer = (fr.ca?.Customer?.FuelerlinxId.GetValueOrDefault() > 0),
                                  TailNumber = fr.awhd.TailNumber
                              })
                                    .ToList();

            AddDemoDataToAirportWatchResult(filteredResult, fboId);

            return filteredResult;
        }
        public async Task<List<AirportWatchHistoricalDataResponse>> GetArrivalsDepartures(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            //Only retrieve arrival and departure occurrences.  Remove all parking occurrences.
            var historicalData = await GetAircraftsHistoricalDataAssociatedWithFbo(groupId, fboId, request);

            if (historicalData != null && historicalData.Count > 0)
            {
                var icao = historicalData.FirstOrDefault().AirportICAO;

                List<Coordinate> coordinates = new List<Coordinate>();
                var allFboGeoFenceClusters = await _airportFboGeofenceClustersService.GetAllClusters();
                var fbo = await _fboService.GetFbo(fboId);
                var fboGeoFenceCluster = allFboGeoFenceClusters.Where(a => a.Icao == icao && a.AcukwikFBOHandlerID == fbo.AcukwikFBOHandlerId).FirstOrDefault();

                if (fboGeoFenceCluster != null)
                {
                    var fboClusterCoordinates = await _airportFboGeofenceClustersService.GetClusterCoordinatesByClusterId(fboGeoFenceCluster.Oid);
                    foreach (var clusterCoordinate in fboClusterCoordinates)
                    {
                        Coordinate coordinate = new Coordinate();
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
                            visitsToMyFbo = pastVisits.Where(p => IsPointInPolygon(new Coordinate(p.Latitude, p.Longitude), coordinates.ToArray())).ToList();

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

                var result2 = (from h in historicalData
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

            if (historicalData == null && historicalData.Count() == 0) return new List<AirportWatchHistoricalDataResponse>();

            var icao = historicalData.FirstOrDefault().AirportICAO;

            List<Coordinate> coordinates = new List<Coordinate>();
            var allFboGeoFenceClusters = await _airportFboGeofenceClustersService.GetAllClusters();
            var fbo = await _fboService.GetFbo(fboId);
            var fboGeoFenceCluster = allFboGeoFenceClusters.Where(a => a.Icao == icao && a.AcukwikFBOHandlerID == fbo.AcukwikFBOHandlerId).FirstOrDefault();

            if (fboGeoFenceCluster != null)
            {
                var fboClusterCoordinates = await _airportFboGeofenceClustersService.GetClusterCoordinatesByClusterIdIQueryable(fboGeoFenceCluster.Oid);
                foreach (var clusterCoordinate in fboClusterCoordinates)
                {
                    Coordinate coordinate = new Coordinate();
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

                   var visitsToMyFboCount = (coordinates.Count > 0) ? pastVisits.Where(p => IsPointInPolygon(new Coordinate(p.Latitude, p.Longitude), coordinates.ToArray())).Count() : 0;


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

            var result = new List<AirportWatchHistoricalDataResponse>();

            result = (from h in historicalData
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
            var historicalData = await GetAircraftsHistoricalDataAssociatedWithFbo(groupId, fboId, request);

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


        public async Task ProcessAirportWatchData(List<AirportWatchLiveData> data, bool isTesting = false)
        {
            _LiveDataToUpdate = new List<AirportWatchLiveData>();
            _LiveDataToInsert = new List<AirportWatchLiveData>();
            _LiveDataToDelete = new List<AirportWatchLiveData>();
            _HistoricalDataToUpdate = new List<AirportWatchHistoricalData>();
            _HistoricalDataToInsert = new List<AirportWatchHistoricalData>();

            await using var fboLinxContext = _ServiceProvider.GetService<FboLinxContext>();
            await using var degaContext = _ServiceProvider.GetService<DegaContext>();

            var airportPositions = await GetAirportPositions(degaContext);

            //Grab distinct aircraft for this set of data
            var distinctAircraftHexCodes =
                data.Where(x => !string.IsNullOrEmpty(x.AircraftHexCode)).Select(x => x.AircraftHexCode).Distinct().ToList();

            //Preload the collection of past records from the last 7 days to use in the loop
            var oldAirportWatchLiveDataCollection = await GetAirportWatchLiveDataFromDatabase(fboLinxContext, distinctAircraftHexCodes, DateTime.UtcNow.AddHours(-1));

            var oldAirportWatchHistoricalDataCollection = await GetAirportWatchHistoricalDataFromDatabase(fboLinxContext, distinctAircraftHexCodes, DateTime.UtcNow.AddHours(-4));

            foreach (var record in data)
            {
                var aircraftOldAirportWatchLiveDataCollection = oldAirportWatchLiveDataCollection
                    .Where(aw => aw.AircraftHexCode == record.AircraftHexCode).OrderByDescending(a => a.BoxTransmissionDateTimeUtc).ToList();

                var oldAirportWatchLiveData = aircraftOldAirportWatchLiveDataCollection.FirstOrDefault(a => !_LiveDataToUpdate.Any(d => d.Oid == a.Oid) && !_LiveDataToDelete.Any(l => l.Oid == a.Oid));

                var oldAirportWatchHistoricalData = oldAirportWatchHistoricalDataCollection
                    .Where(aw => aw.AircraftHexCode == record.AircraftHexCode)
                    .OrderByDescending(aw => aw.AircraftPositionDateTimeUtc)
                    .FirstOrDefault();

                var airportWatchHistoricalData = AirportWatchHistoricalData.ConvertFromAirportWatchLiveData(record);

                //Record historical status

                //Compare our last "live" record with the new one to determine if the aircraft is taking off or landing

                //Next check if the last live record we have for the aircraft had a different IsAircraftOnGround state than what we see now
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
                    _LiveDataToInsert.Add(record);
                }
                else
                {
                    //Capture the tail from the previous record before copying the new information to prevent needing to lookup the tail again
                    var tailNumber = oldAirportWatchLiveData.TailNumber;
                    AirportWatchLiveData.CopyEntity(oldAirportWatchLiveData, record);
                    oldAirportWatchLiveData.TailNumber = tailNumber;
                    _LiveDataToUpdate.Add(oldAirportWatchLiveData);

                    if (aircraftOldAirportWatchLiveDataCollection.Count > 1)
                    {
                        aircraftOldAirportWatchLiveDataCollection.Remove(oldAirportWatchLiveData);
                        _LiveDataToDelete.AddRange(aircraftOldAirportWatchLiveDataCollection);
                    }
                }
            }

            await PrepareRecordsForDatabase(degaContext, airportPositions);

            if (!isTesting)
                await CommitChanges(fboLinxContext);
        }

        private async Task PrepareRecordsForDatabase(DegaContext degaContext, List<AirportPosition> airportPositions)
        {
            //[#2ht0dek] Reverting back changes for this branch and upgrading EFCore and BulkExtensions to test performance
            //Set the nearest airport for all records that will be recorded for historical statuses
            _HistoricalDataToInsert.ForEach(x =>
            {
                x.AirportICAO = GetNearestICAO(airportPositions, x.Latitude, x.Longitude);
            });
            _HistoricalDataToUpdate.ForEach(x =>
            {
                x.AirportICAO = GetNearestICAO(airportPositions, x.Latitude, x.Longitude);
            });

            _HistoricalDataToInsert = _HistoricalDataToInsert.Where(record => {
                if (string.IsNullOrEmpty(record.AirportICAO) || string.IsNullOrEmpty(record.BoxName)) return false;
                return record.BoxName.ToLower().StartsWith(record.AirportICAO.ToLower());
            }).ToList();
            _HistoricalDataToUpdate = _HistoricalDataToUpdate.Where(record => {
                if (string.IsNullOrEmpty(record.AirportICAO) || string.IsNullOrEmpty(record.BoxName)) return false;
                return record.BoxName.ToLower().StartsWith(record.AirportICAO.ToLower());
            }).ToList();

            await SetTailNumber(degaContext, _HistoricalDataToInsert);
            await SetTailNumber(degaContext, _LiveDataToInsert);
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

        public async Task<List<FboHistoricalDataModel>> GetHistoricalDataAssociatedWithGroupOrFbo(int groupId, int? fboId, AirportWatchHistoricalDataRequest request)
        {
            var fboIcao = fboId.HasValue ? await _fboService.GetFBOIcao(fboId.Value) : null;

            var historicalData = await (from awhd in _context.AirportWatchHistoricalData
                                        where
                                         (!fboId.HasValue || awhd.AirportICAO == fboIcao) &&
                                         (request.StartDateTime == null || awhd.AircraftPositionDateTimeUtc >= request.StartDateTime.Value.ToUniversalTime()) &&
                                         (request.EndDateTime == null || awhd.AircraftPositionDateTimeUtc <= request.EndDateTime.Value.ToUniversalTime().AddDays(1))
                                        select new
                                        {
                                            Oid = awhd.Oid,
                                            awhd.AircraftHexCode,
                                            awhd.AtcFlightNumber,
                                            awhd.AircraftPositionDateTimeUtc,
                                            awhd.AircraftStatus,
                                            awhd.AirportICAO,
                                            awhd.AircraftTypeCode,
                                            awhd.Latitude,
                                            awhd.Longitude,
                                            AirportWatchAircraftTailNumberFlightNumber = awhd.TailNumber,
                                            TailNumber = awhd.TailNumber
                                        }).ToListAsync();

            var customerAircraftsData = await (from ca in _context.CustomerAircrafts
                                               join cig in _context.CustomerInfoByGroup on new { ca.CustomerId, GroupId = ca.GroupId ?? 0 } equals new { cig.CustomerId, cig.GroupId }
                                               where ca.GroupId == groupId
                                               select new
                                               {
                                                   ca.GroupId,
                                                   ca.CustomerId,
                                                   ca.TailNumber,
                                                   ca.AircraftId,
                                                   cig.Company,
                                                   CustomerInfoByGroupID = cig.Oid,
                                               }).ToListAsync();

            var joinedHistoricalData = (from hd in historicalData
                                        join cad in customerAircraftsData
                                        on hd.AirportWatchAircraftTailNumberFlightNumber equals cad.TailNumber
                                        into leftJoinedCustomers
                                        from cad in leftJoinedCustomers.DefaultIfEmpty()
                                        group hd by new
                                        {
                                            AirportWatchHistoricalDataID = hd.Oid,
                                            hd.AircraftHexCode,
                                            hd.AtcFlightNumber,
                                            hd.AircraftPositionDateTimeUtc,
                                            hd.AircraftStatus,
                                            hd.AirportICAO,
                                            hd.AircraftTypeCode,
                                            hd.Latitude,
                                            hd.Longitude,
                                            Company = cad == null ? "" : cad.Company,
                                            CustomerId = cad == null ? 0 : cad.CustomerId,
                                            TailNumber = cad == null ? hd.TailNumber : cad.TailNumber,
                                            AircraftId = cad == null ? 0 : cad.AircraftId,
                                            CustomerInfoByGroupID = cad == null ? 0 : cad.CustomerInfoByGroupID,
                                        }
                                        into groupedResult
                                        select new FboHistoricalDataModel
                                        {
                                            AirportWatchHistoricalDataID = groupedResult.Key.AirportWatchHistoricalDataID,
                                            AircraftHexCode = groupedResult.Key.AircraftHexCode,
                                            AtcFlightNumber = groupedResult.Key.AtcFlightNumber,
                                            AircraftPositionDateTimeUtc = groupedResult.Key.AircraftPositionDateTimeUtc,
                                            AircraftStatus = groupedResult.Key.AircraftStatus,
                                            AircraftTypeCode = groupedResult.Key.AircraftTypeCode,
                                            Company = groupedResult.Key.Company,
                                            CustomerId = groupedResult.Key.CustomerId,
                                            TailNumber = groupedResult.Key.TailNumber,
                                            AircraftId = groupedResult.Key.AircraftId,
                                            AirportICAO = groupedResult.Key.AirportICAO,
                                            CustomerInfoByGroupID = groupedResult.Key.CustomerInfoByGroupID,
                                            Latitude = groupedResult.Key.Latitude,
                                            Longitude = groupedResult.Key.Longitude
                                        }).ToList();

            return joinedHistoricalData;
        }
        public async Task<List<FboHistoricalDataModel>> GetHistoricalDataAssociatedWithGroupOrFboRefactored(int groupId, int? fboId, AirportWatchHistoricalDataRequest request)
        {
            var fboIcao = fboId.HasValue ? await _fboService.GetFBOIcaoAsNoTracking(fboId.Value) : null;

            //Only select the indexed columns that are important for historical lookup.  Further data can be isolated by the ID of the historical record.
            var historicalData = await
                _context.AirportWatchHistoricalData
                    .Where(awhd => !fboId.HasValue || awhd.AirportICAO == fboIcao)
                    .Where(awhd => request.StartDateTime == null || awhd.AircraftPositionDateTimeUtc >= request.StartDateTime.Value.ToUniversalTime())
                    .Where(awhd => request.EndDateTime == null || awhd.AircraftPositionDateTimeUtc <= request.EndDateTime.Value.ToUniversalTime().AddDays(1))
                    .Select(awhd => new
                    {
                        Oid = awhd.Oid,
                        awhd.AircraftHexCode,
                        awhd.AtcFlightNumber,
                        awhd.AircraftPositionDateTimeUtc,
                        awhd.AircraftStatus,
                        awhd.AirportICAO,
                        awhd.AircraftTypeCode,
                        awhd.Latitude,
                        awhd.Longitude,
                        AirportWatchAircraftTailNumberFlightNumber = awhd.TailNumber,
                        TailNumber = awhd.TailNumber
                    })
                    .AsNoTracking()
                    .ToListAsync();

            //this is repeated query in  getwatchlivedata
            var customerAircraftsData =
                           _context.CustomerAircrafts
                           .Include(ca => ca.Customer)
                           .Include(ca => ca.Customer.CustomerInfoByGroup)
                           .Where(ca => (ca.GroupId ?? 0) == ca.Customer.CustomerInfoByGroup.GroupId)
                           .Where(ca => ca.GroupId == groupId)
                           .AsNoTracking();

            //this is repeated query in  getwatchlivedata
            var joinedHistoricalData = (from hd in historicalData
                                        join cad in customerAircraftsData
                                        on hd.TailNumber equals cad.TailNumber
                                        into leftJoinedCustomers
                                        from cad in leftJoinedCustomers.DefaultIfEmpty()
                                        group hd by new
                                        {
                                            AirportWatchHistoricalDataID = hd.Oid,
                                            hd.AircraftHexCode,
                                            hd.AtcFlightNumber,
                                            hd.AircraftPositionDateTimeUtc,
                                            hd.AircraftStatus,
                                            hd.AirportICAO,
                                            hd.AircraftTypeCode,
                                            hd.Latitude,
                                            hd.Longitude,
                                            Company = cad == null ? "" : cad.Customer.Company,
                                            CustomerId = cad == null ? 0 : cad.CustomerId,
                                            TailNumber = cad == null ? hd.TailNumber : cad.TailNumber,
                                            AircraftId = cad == null ? 0 : cad.AircraftId,
                                            CustomerInfoByGroupID = cad == null ? 0 : cad.Customer.CustomerInfoByGroup.Oid,
                                        }
                                        into groupedResult
                                        select new FboHistoricalDataModel
                                        {
                                            AirportWatchHistoricalDataID = groupedResult.Key.AirportWatchHistoricalDataID,
                                            AircraftHexCode = groupedResult.Key.AircraftHexCode,
                                            AtcFlightNumber = groupedResult.Key.AtcFlightNumber,
                                            AircraftPositionDateTimeUtc = groupedResult.Key.AircraftPositionDateTimeUtc,
                                            AircraftStatus = groupedResult.Key.AircraftStatus,
                                            AircraftTypeCode = groupedResult.Key.AircraftTypeCode,
                                            Company = groupedResult.Key.Company,
                                            CustomerId = groupedResult.Key.CustomerId,
                                            TailNumber = groupedResult.Key.TailNumber,
                                            AircraftId = groupedResult.Key.AircraftId,
                                            AirportICAO = groupedResult.Key.AirportICAO,
                                            CustomerInfoByGroupID = groupedResult.Key.CustomerInfoByGroupID,
                                            Latitude = groupedResult.Key.Latitude,
                                            Longitude = groupedResult.Key.Longitude
                                        }).ToList();

            return joinedHistoricalData;
        }

        public async Task<List<FboHistoricalDataModel>> GetAircraftsHistoricalDataAssociatedWithFbo(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var historicalData = await GetHistoricalDataAssociatedWithGroupOrFbo(groupId, fboId, request);
            watch.Stop();
            Console.WriteLine($"GetArrivalsDepartures Execution Time: {watch.ElapsedMilliseconds} ms");

            var watch2 = new System.Diagnostics.Stopwatch();
            watch2.Start();
            var historicalData2 = await GetHistoricalDataAssociatedWithGroupOrFboRefactored(groupId, fboId, request);
            watch2.Stop();
            Console.WriteLine($"GetArrivalsDepartures Execution Time: {watch2.ElapsedMilliseconds} ms");




            var result = (from h in historicalData
                          join a in _aircraftService.GetAllAircraftsOnlyAsQueryable() on h.AircraftId equals a.AircraftId
                          into leftJoinedAircrafts
                          from a in leftJoinedAircrafts.DefaultIfEmpty()
                          orderby h.AircraftPositionDateTimeUtc descending
                          select new FboHistoricalDataModel
                          {
                              CustomerId = h.CustomerId,
                              Company = h.Company,
                              AircraftPositionDateTimeUtc = h.AircraftPositionDateTimeUtc,
                              TailNumber = h.TailNumber,
                              AtcFlightNumber = h.AtcFlightNumber,
                              AircraftHexCode = h.AircraftHexCode,
                              AircraftTypeCode = h.AircraftTypeCode,
                              Make = a?.Make,
                              Model = a?.Model,
                              AircraftStatus = h.AircraftStatus,
                              AirportICAO = h.AirportICAO,
                              CustomerInfoByGroupID = h.CustomerInfoByGroupID,
                              Latitude = h.Latitude,
                              Longitude = h.Longitude
                          }).ToList();
            return result;
        }
        public async Task<List<FboHistoricalDataModel>> GetAircraftsHistoricalDataAssociatedWithFboRefactored(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            var historicalData = await GetHistoricalDataAssociatedWithGroupOrFboRefactored(groupId, fboId, request);

            var result = (from h in historicalData
                          join a in _aircraftService.GetAllAircraftsOnlyAsQueryable().AsNoTracking() on h.AircraftId equals a.AircraftId
                          into leftJoinedAircrafts
                          from a in leftJoinedAircrafts.DefaultIfEmpty()
                          orderby h.AircraftPositionDateTimeUtc descending
                          select new FboHistoricalDataModel
                          {
                              CustomerId = h.CustomerId,
                              Company = h.Company,
                              AircraftPositionDateTimeUtc = h.AircraftPositionDateTimeUtc,
                              TailNumber = h.TailNumber,
                              AtcFlightNumber = h.AtcFlightNumber,
                              AircraftHexCode = h.AircraftHexCode,
                              AircraftTypeCode = h.AircraftTypeCode,
                              Make = a?.Make,
                              Model = a?.Model,
                              AircraftStatus = h.AircraftStatus,
                              AirportICAO = h.AirportICAO,
                              CustomerInfoByGroupID = h.CustomerInfoByGroupID,
                              Latitude = h.Latitude,
                              Longitude = h.Longitude
                          });
            return result.ToList();
        }

        public async Task<List<AcukwikAirport>> GetAirportsWithAntennaData()
        {
            try
            {
                //Fetch all airports with antenna data from the last two hours
                var pastWeekDateTime = DateTime.UtcNow.Add(new TimeSpan(-1, 0, 0, 0));
                var distinctBoxes = await _context.AirportWatchHistoricalData
                    .Where(x => x.AircraftPositionDateTimeUtc > pastWeekDateTime && !string.IsNullOrEmpty(x.AirportICAO))
                    .Select(x => x.AirportICAO)
                    .Distinct()
                    .ToListAsync();
                distinctBoxes.RemoveAll(x => string.IsNullOrEmpty(x));
                distinctBoxes = distinctBoxes.Select(x => x.ToUpper()).ToList();

                //Fetch distinct airports from clusters or that were added manually
                var distinctAirportIdentifiersWithClusters = await _airportFboGeofenceClustersService.GetDistinctAirportIdentifiersWithClusters();
                distinctBoxes.AddRange(distinctAirportIdentifiersWithClusters);

                var airports = await _degaContext.AcukwikAirports.Where(x => distinctBoxes.Contains(x.Icao))
                    .Include(x => x.AcukwikFbohandlerDetailCollection).ToListAsync();
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

                return distinctBoxes;

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

        public async Task<List<AirportWatchHistoricalData>> GetParkingOccurencesByAirport(string icao,
            DateTime startDateTimeUtc, DateTime endDateTimeUtc)
        {
            try
            {
                var occurrences = await _context.AirportWatchHistoricalData.Where(x =>
                    x.AircraftStatus == AircraftStatusType.Parking
                    && x.BoxTransmissionDateTimeUtc >= startDateTimeUtc
                    && x.BoxTransmissionDateTimeUtc <= endDateTimeUtc
                    && x.AirportICAO == icao).ToListAsync();
                return occurrences;
            }
            catch (System.Exception exception)
            {
                Debug.WriteLine("Error in AirportWatchService.GetParkingOccurencesByAirport: " + exception.Message);
                return new List<AirportWatchHistoricalData>();
            }
        }

        public async Task GetAirportWatchTestData()
        {
            var pastTenMinutes = DateTime.UtcNow.Add(new TimeSpan(0, -90, 0)); //CHANGE BACK

            var aircraftWatchLiveData = await _context.AirportWatchLiveData.Where(x => x.AircraftPositionDateTimeUtc >= pastTenMinutes).ToListAsync();
            await ProcessAirportWatchData(aircraftWatchLiveData, true);
        }

        private async Task<List<AirportWatchAntennaStatusGrid>> GetDistinctAntennaBoxes()
        {
            var distinctHistoricalBoxes = await _context.AirportWatchHistoricalData.GroupBy(a => a.BoxName).Select(ah => new
            {
                BoxName = ah.Key,
                AircraftPositionDateTimeUtc = ah.Max(row => row.AircraftPositionDateTimeUtc)
            }).ToListAsync();
            distinctHistoricalBoxes.RemoveAll(x => string.IsNullOrEmpty(x.BoxName));

            var pastHalfHour = DateTime.UtcNow.Add(new TimeSpan(0, -30, 0));
            var distinctLiveBoxes = await _context.AirportWatchLiveData.Where(x => x.AircraftPositionDateTimeUtc > pastHalfHour).GroupBy(a => a.BoxName).Select(al => new
            {
                BoxName = al.Key,
                AircraftPositionDateTimeUtc = al.Max(row => row.AircraftPositionDateTimeUtc)
            }).OrderBy(b => b.BoxName).ToListAsync();
            distinctLiveBoxes.RemoveAll(x => string.IsNullOrEmpty(x.BoxName));

            var distinctBoxesHistorical = (from dh in distinctHistoricalBoxes
                                           join dl in distinctLiveBoxes on dh.BoxName equals dl.BoxName
                                           into leftJoinDistinctLiveBoxes
                                           from dl in leftJoinDistinctLiveBoxes.DefaultIfEmpty()
                                           select new AirportWatchAntennaStatusGrid
                                           {
                                               BoxName = dl == null ? dh.BoxName : dl.BoxName,
                                               Status = dl == null ? "Not Active" : "Active",
                                               LastUpdateRaw = dl == null ? "" : dl.AircraftPositionDateTimeUtc.ToString(),
                                               LastUpdateCurated = dh == null ? "" : dh.AircraftPositionDateTimeUtc.ToString()
                                           });

            var distinctBoxes = (from dl in distinctLiveBoxes
                                 join db in distinctBoxesHistorical on dl.BoxName equals db.BoxName
                                 into leftJoinDistinctBoxes
                                 from db in leftJoinDistinctBoxes.DefaultIfEmpty()
                                 select new AirportWatchAntennaStatusGrid
                                 {
                                     BoxName = dl == null ? db.BoxName : dl.BoxName,
                                     Status = dl == null ? "Not Active" : "Active",
                                     LastUpdateRaw = dl == null ? "" : dl.AircraftPositionDateTimeUtc.ToString(),
                                     LastUpdateCurated = db == null ? "" : db.LastUpdateCurated
                                 }).Union(distinctBoxesHistorical).GroupBy(d => d.BoxName).Select(grouped => grouped.First()).ToList();

            return distinctBoxes;
        }

        private async Task SetTailNumber(DegaContext context, IEnumerable<BaseAirportWatchData> airportWatchRecords)
        {
            IEnumerable<string> aircraftHexCodesToInsert = airportWatchRecords.Select(x => x.AircraftHexCode).Distinct();
            List<AircraftHexTailMapping> hexTailMappings = await context.AircraftHexTailMapping.Where(x => aircraftHexCodesToInsert.Contains(x.AircraftHexCode)).ToListAsync();
            foreach (BaseAirportWatchData airportWatchRecord in airportWatchRecords)
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

        private void AddPossibleParkingOccurrence(AirportWatchHistoricalData oldAirportWatchHistoricalData, AirportWatchHistoricalData airportWatchHistoricalData)
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
                AirportWatchHistoricalData.CopyEntity(oldAirportWatchHistoricalData, airportWatchHistoricalData);
                oldAirportWatchHistoricalData.TailNumber = tailNumber;
                _HistoricalDataToUpdate.Add(oldAirportWatchHistoricalData);
            }
            else
            {
                _HistoricalDataToInsert.Add(airportWatchHistoricalData);
            }

        }

        private async Task CommitChanges(FboLinxContext context)
        {
            var liveDataToInsertAndUpdate = _LiveDataToInsert;
            if (liveDataToInsertAndUpdate == null)
                liveDataToInsertAndUpdate = new List<AirportWatchLiveData>();
            liveDataToInsertAndUpdate.AddRange(_LiveDataToUpdate == null ? new List<AirportWatchLiveData>() : _LiveDataToUpdate);

            await using var transaction = await context.Database.BeginTransactionAsync();
            if (liveDataToInsertAndUpdate.Count > 0)
                await context.BulkInsertOrUpdateAsync(liveDataToInsertAndUpdate, config =>
                {
                    config.WithHoldlock = false;
                    config.BatchSize = 5000;
                });
            if (_LiveDataToDelete?.Count > 0)
                await context.BulkDeleteAsync(_LiveDataToDelete, config => config.WithHoldlock = false);
            if (_HistoricalDataToInsert?.Count > 0)
                await context.BulkInsertAsync(_HistoricalDataToInsert, config => config.WithHoldlock = false);
            if (_HistoricalDataToUpdate?.Count > 0)
                await context.BulkUpdateAsync(_HistoricalDataToUpdate, config => config.WithHoldlock = false);
            await transaction.CommitAsync();
        }

        private async Task<List<AirportPosition>> GetAirportPositions(DegaContext context)
        {
            var recordsFromCache = LookupAirportRecordsByFromCache();

            if (recordsFromCache == null)
            {
                var airports = (await (context.AcukwikAirports

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
                                        Faa = a.Faa
                                    };
                                })
                                .ToList();

                //Store in cache before returning
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
                _MemoryCache.Set(_AllAirportsPositioningCacheKey, airports, cacheEntryOptions);

                recordsFromCache = airports;
            }

            return recordsFromCache;
        }

        private List<AirportPosition> LookupAirportRecordsByFromCache()
        {
            List<AirportPosition> result = null;
            if (_MemoryCache.TryGetValue(_AllAirportsPositioningCacheKey, out result))
                return result;
            return result;
        }

        private string GetNearestICAO(List<AirportPosition> airportPositions, double latitude, double longitude)
        {
            double minDistance = -1;
            string nearestICAO = null;
            var closestAirports = airportPositions.Where(a => a.Latitude >= latitude - 1 && a.Latitude <= latitude + 1 && a.Longitude >= longitude - 1 && a.Longitude <= longitude + 1).ToList();
            foreach (var airport in closestAirports)
            {
                double distance = GeoCalculator.GetDistance(latitude, longitude, airport.Latitude, airport.Longitude, 5, DistanceUnit.Miles);

                if (minDistance == -1 || distance < minDistance)
                {
                    minDistance = distance;
                    var airportIcao = airport.Icao;
                    if (airportIcao == "")
                        airportIcao = airport.Faa == "" ? airport.Iata : airport.Faa;
                    nearestICAO = airportIcao;
                }
            }

            return nearestICAO;
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

        public bool IsPointInPolygon(Coordinate p, Coordinate[] polygon)
        {
            double minX = polygon[0].Latitude;
            double maxX = polygon[0].Latitude;
            double minY = polygon[0].Longitude;
            double maxY = polygon[0].Longitude;
            for (int i = 1; i < polygon.Length; i++)
            {
                Coordinate q = polygon[i];
                minX = Math.Min(q.Latitude, minX);
                maxX = Math.Max(q.Latitude, maxX);
                minY = Math.Min(q.Longitude, minY);
                maxY = Math.Max(q.Longitude, maxY);
            }
            if (p.Latitude < minX || p.Latitude > maxX || p.Longitude < minY || p.Longitude > maxY)
            {
                return false;
            }
            // https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if ((polygon[i].Longitude > p.Longitude) != (polygon[j].Longitude > p.Longitude) &&
                     p.Latitude < (polygon[j].Latitude - polygon[i].Latitude) * (p.Longitude - polygon[i].Longitude) / (polygon[j].Longitude - polygon[i].Longitude) + polygon[i].Latitude)
                {
                    inside = !inside;
                }
            }
            return inside;
        }

        private async Task<List<AirportWatchLiveData>> GetAirportWatchLiveDataFromDatabase(FboLinxContext context, List<string> distinctAircraftHexCodes, DateTime aircraftPositionDateTime)
        {
            var result = await context.AirportWatchLiveData
                .Where(x =>
                    distinctAircraftHexCodes.Any(hexCode => hexCode == x.AircraftHexCode)
                    && x.AircraftPositionDateTimeUtc >= aircraftPositionDateTime)
                .AsNoTracking()
                .ToListAsync();
            return result;
        }

        private async Task<List<AirportWatchHistoricalData>> GetAirportWatchHistoricalDataFromDatabase(FboLinxContext context, List<string> distinctAircraftHexCodes, DateTime aircraftPositionDateTime)
        {
            var result = await context.AirportWatchHistoricalData
                .Where(x =>
                    distinctAircraftHexCodes.Any(hexCode => hexCode == x.AircraftHexCode)
                    && x.AircraftPositionDateTimeUtc >= aircraftPositionDateTime)
                .AsNoTracking()
                .ToListAsync();
            return result;
        }
    }

    public class FboHistoricalDataModel
    {
        public int AirportWatchHistoricalDataID { get; set; }
        public string AircraftHexCode { get; set; }
        public string AtcFlightNumber { get; set; }
        public DateTime AircraftPositionDateTimeUtc { get; set; }
        public AircraftStatusType AircraftStatus { get; set; }
        public string AircraftTypeCode { get; set; }
        public string Company { get; set; }
        public int CustomerId { get; set; }
        public string TailNumber { get; set; }
        public int AircraftId { get; set; }
        public string AirportICAO { get; set; }
        public int CustomerInfoByGroupID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string AircraftStatusDescription
        {
            get
            {
                if (string.IsNullOrEmpty(AtcFlightNumber))
                    return "";
                return FBOLinx.Core.Utilities.Enum.GetDescription(AircraftStatus);
            }
        }
    }
}
