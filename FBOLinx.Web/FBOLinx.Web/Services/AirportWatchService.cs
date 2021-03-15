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

namespace FBOLinx.Web.Services
{
    public class AirportWatchService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private readonly AircraftService _aircraftService;
        private readonly FboService _fboService;
        private List<AirportWatchLiveData> _LiveDataToUpdate;
        private List<AirportWatchLiveData> _LiveDataToInsert;
        private List<AirportWatchHistoricalData> _HistoricalDataToUpdate;
        private List<AirportWatchHistoricalData> _HistoricalDataToInsert;
        private List<AirportWatchAircraftTailNumber> _TailNumberDataToInsert;

        public AirportWatchService(FboLinxContext context, DegaContext degaContext,  AircraftService aircraftService, FboService fboService)
        {
            _context = context;
            _degaContext = degaContext;
            _aircraftService = aircraftService;
            _fboService = fboService;
        }

        public async Task<List<AirportWatchLiveData>> GetAirportWatchLiveData()
        {
            List<AirportWatchLiveData> filteredResult = new List<AirportWatchLiveData>();
            var timelimit = DateTime.UtcNow.AddMinutes(-2);

            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                    filteredResult = await _context.AirportWatchLiveData
                        .Where(x => x.AircraftPositionDateTimeUtc >= timelimit)
                        .OrderBy(x => x.AircraftPositionDateTimeUtc)
                        .ThenBy(x => x.AircraftHexCode)
                        .ThenBy(x => x.AtcFlightNumber)
                        .ThenBy(x => x.GpsAltitude)
                        .ToListAsync();
                    scope.Complete();
            }
            return filteredResult;
        }

        public async Task<List<AirportWatchHistoricalDataResponse>> GetArrivalsDepartures(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            var historicalData = await GetHistoricalDataAssociatedWithFbo(groupId, fboId, request);

            var aircraftHistoricalData = (from h in historicalData
                                          join a in _aircraftService.GetAllAircraftsOnlyAsQueryable() on h.AircraftId equals a.AircraftId
                                          into leftJoinedAircrafts
                                          from a in leftJoinedAircrafts.DefaultIfEmpty()
                                          orderby h.AircraftPositionDateTimeUtc descending
                                          select new
                                          {
                                              h.CustomerId,
                                              h.Company,
                                              h.AircraftPositionDateTimeUtc,
                                              h.TailNumber,
                                              h.AtcFlightNumber,
                                              h.AircraftHexCode,
                                              h.AircraftTypeCode,
                                              a?.Model,
                                              h.AircraftStatus,
                                              h.AirportICAO,
                                              h.CustomerInfoByGroupID,
                                          }).ToList();

            var customersData = aircraftHistoricalData
                .Select(h => new AirportWatchHistoricalDataResponse
                {
                    CompanyId = h.CustomerInfoByGroupID,
                    Company = h.Company,
                    DateTime = h.AircraftPositionDateTimeUtc,
                    TailNumber = h.TailNumber,
                    FlightNumber = h.AtcFlightNumber,
                    HexCode = h.AircraftHexCode,
                    AircraftType = h.Model,
                    AircraftTypeCode = h.AircraftTypeCode,
                    Status = h.AircraftStatus,
                })
                .ToList();

            return customersData;
        }

        public async Task<List<AirportWatchHistoricalDataResponse>> GetVisits(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            var historicalData = await GetHistoricalDataAssociatedWithFbo(groupId, fboId, request);

            var aircraftHistoricalData = (from h in historicalData
                                          join a in _aircraftService.GetAllAircraftsOnlyAsQueryable() on h.AircraftId equals a.AircraftId
                                          into leftJoinedAircrafts
                                          from a in leftJoinedAircrafts.DefaultIfEmpty()
                                          orderby h.AircraftPositionDateTimeUtc descending
                                          select new
                                          {
                                              h.CustomerId,
                                              h.Company,
                                              h.AircraftPositionDateTimeUtc,
                                              h.TailNumber,
                                              h.AtcFlightNumber,
                                              h.AircraftHexCode,
                                              h.AircraftTypeCode,
                                              a?.Model,
                                              h.AircraftStatus,
                                              h.AirportICAO,
                                              h.CustomerInfoByGroupID,
                                          }).ToList();

            var noCustomerData = aircraftHistoricalData
                .Where(h => h.Company == null)
                .Select(h => new AirportWatchHistoricalDataResponse
                {
                    Company = h.Company,
                    DateTime = h.AircraftPositionDateTimeUtc,
                    TailNumber = h.TailNumber,
                    FlightNumber = h.AtcFlightNumber,
                    HexCode = h.AircraftHexCode,
                    AircraftType = h.Model,
                    AircraftTypeCode = h.AircraftTypeCode,
                    Status = h.AircraftStatus,
                    CompanyId = h.CustomerInfoByGroupID
                })
                .ToList();

            var customerData = aircraftHistoricalData
                .Where(h => h.Company != null)
                .GroupBy(ah => new { ah.CustomerId, ah.AirportICAO, ah.AircraftHexCode, ah.AtcFlightNumber })
                .Select(g =>
                {
                    var latest = g
                        .OrderByDescending(ah => ah.AircraftPositionDateTimeUtc).First();

                    var pastVisits = g
                        .Where(ah => ah.AircraftStatus == AirportWatchHistoricalData.AircraftStatusType.Landing)
                        .Count();

                    return new AirportWatchHistoricalDataResponse
                    {
                        Company = latest.Company,
                        DateTime = latest.AircraftPositionDateTimeUtc,
                        TailNumber = latest.TailNumber,
                        FlightNumber = latest.AtcFlightNumber,
                        HexCode = latest.AircraftHexCode,
                        AircraftType = latest.Model,
                        AircraftTypeCode = latest.AircraftTypeCode,
                        Status = latest.AircraftStatus,
                        PastVisits = pastVisits,
                        CompanyId = latest.CustomerInfoByGroupID
                    };
                })
                .ToList();

            return noCustomerData.Concat(customerData).ToList();
        }

        public async Task ProcessAirportWatchData(List<AirportWatchLiveData> data)
        {
            _LiveDataToUpdate = new List<AirportWatchLiveData>();
            _LiveDataToInsert = new List<AirportWatchLiveData>();
            _HistoricalDataToUpdate = new List<AirportWatchHistoricalData>();
            _HistoricalDataToInsert = new List<AirportWatchHistoricalData>();
            _TailNumberDataToInsert = new List<AirportWatchAircraftTailNumber>();

            var airportPositions = await GetAirportPositions();
            
            //Grab distinct aircraft for this set of data
            var distinctAircraftHexCodes =
                data.Where(x => !string.IsNullOrEmpty(x.AircraftHexCode)).Select(x => x.AircraftHexCode).ToList().Distinct();
            var distinctFlightNumbers = data.Where(x => !string.IsNullOrEmpty(x.AtcFlightNumber))
                .Select(x => x.AtcFlightNumber).ToList().Distinct();

            //Preload the collection of past records from the last 7 days to use in the loop
            var oldAirportWatchLiveDataCollection = await _context.AirportWatchLiveData.Where(x =>
                distinctAircraftHexCodes.Any(hexCode => hexCode == x.AircraftHexCode) 
                && distinctFlightNumbers.Any(flightNumber => flightNumber == x.AtcFlightNumber)
                && x.AircraftPositionDateTimeUtc > DateTime.UtcNow.AddDays(-7)).ToListAsync();

            var oldAirportWatchHistoricalDataCollection = await _context.AirportWatchHistoricalData.Where(x =>
                distinctAircraftHexCodes.Any(hexCode => hexCode == x.AircraftHexCode)
                && distinctFlightNumbers.Any(flightNumber => flightNumber == x.AtcFlightNumber)
                && x.AircraftPositionDateTimeUtc > DateTime.UtcNow.AddDays(-7)).ToListAsync();

            foreach (var record in data)
            {
                var oldAirportWatchLiveData = oldAirportWatchLiveDataCollection
                    .FirstOrDefault(aw => aw.AircraftHexCode == record.AircraftHexCode && aw.AtcFlightNumber == record.AtcFlightNumber);

                var oldAirportWatchHistoricalData = oldAirportWatchHistoricalDataCollection
                    .Where(aw => aw.AircraftHexCode == record.AircraftHexCode && aw.AtcFlightNumber == record.AtcFlightNumber)
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
                    _TailNumberDataToInsert.Add(new AirportWatchAircraftTailNumber
                    {
                        AircraftHexCode = record.AircraftHexCode,
                        AtcFlightNumber = record.AtcFlightNumber,
                    });
                    _LiveDataToInsert.Add(record);
                }
                else
                {
                    AirportWatchLiveData.CopyEntity(oldAirportWatchLiveData, record);
                    _LiveDataToUpdate.Add(oldAirportWatchLiveData);
                }
            }

            //Set the nearest airport for all records that will be recorded for historical statuses
            _HistoricalDataToInsert.ForEach(x =>
            {
                x.AirportICAO = GetNearestICAO(airportPositions, x.Latitude, x.Longitude);
            });
            _HistoricalDataToUpdate.ForEach(x =>
            {
                x.AirportICAO = GetNearestICAO(airportPositions, x.Latitude, x.Longitude);
            });
            
            await CommitChanges();
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
            if (oldAirportWatchHistoricalData.AircraftStatus != AirportWatchHistoricalData.AircraftStatusType.Landing &&
                oldAirportWatchHistoricalData.AircraftStatus != AirportWatchHistoricalData.AircraftStatusType.Parking)
                return;

            //If the aircraft has not moved then do not update the parking record - we want to keep the old record when it first stopped moving
            if (Math.Abs(airportWatchHistoricalData.Latitude - oldAirportWatchHistoricalData.Latitude) <= 0.000001 ||
                Math.Abs(airportWatchHistoricalData.Longitude - oldAirportWatchHistoricalData.Longitude) <= 0.000001)
                return;
            //Last record was over 10 minutes ago and the aircraft hasn't moved since that point - keep this old record and don't update
            if (oldAirportWatchHistoricalData.AircraftPositionDateTimeUtc < DateTime.UtcNow.AddMinutes(-10))
                return;
            //The aircraft has moved since landing - this should be an updated parking record
            airportWatchHistoricalData.AircraftStatus = AirportWatchHistoricalData.AircraftStatusType.Parking;

            if (oldAirportWatchHistoricalData.AircraftStatus == AirportWatchHistoricalData.AircraftStatusType.Parking)
            {
                AirportWatchHistoricalData.CopyEntity(oldAirportWatchHistoricalData, airportWatchHistoricalData);
                _HistoricalDataToUpdate.Add(oldAirportWatchHistoricalData);
            }
            else
            {
                _HistoricalDataToInsert.Add(airportWatchHistoricalData);
            }

        }

        private async Task CommitChanges()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            if (_LiveDataToInsert != null)
                await _context.BulkInsertAsync(_LiveDataToInsert);
            if (_LiveDataToUpdate != null)
                await _context.BulkUpdateAsync(_LiveDataToUpdate);
            if (_HistoricalDataToInsert != null)
                await _context.BulkInsertAsync(_HistoricalDataToInsert);
            if (_HistoricalDataToUpdate != null)
                await _context.BulkUpdateAsync(_HistoricalDataToUpdate); 
            if (_TailNumberDataToInsert != null)
                await _context.BulkInsertAsync(_TailNumberDataToInsert);
            await _context.AirportWatchChangeTracker.AddAsync(new AirportWatchChangeTracker()
            {
                DateTimeAppliedUtc = DateTime.UtcNow,
                HistoricalDataRecords = (_HistoricalDataToInsert?.Count ?? 0) + (_HistoricalDataToUpdate?.Count ?? 0),
                LiveDataRecords = (_LiveDataToInsert?.Count ?? 0) + (_LiveDataToUpdate?.Count ?? 0),
                TailNumberRecords = _TailNumberDataToInsert?.Count ?? 0
            });
            await transaction.CommitAsync();
        }
        
        private async Task<List<AirportPosition>> GetAirportPositions()
        {
            var airports = (await _degaContext.AcukwikAirports
                            .ToListAsync()
                            )
                            .Select(a =>
                            {
                                var (alat, alng) = GetGeoLocationFromGPS(a.Latitude, a.Longitude);

                                return new AirportPosition
                                {
                                    Latitude = alat,
                                    Longitude = alng,
                                    Icao = a.Icao,
                                };
                            })
                            .ToList();

            return airports;
        }

        private string GetNearestICAO(List<AirportPosition> airportPositions, double latitude, double longitude)
        {
            double minDistance = -1;
            string nearestICAO = null;
            foreach (var airport in airportPositions)
            {
                double distance = GeoCalculator.GetDistance(latitude, longitude, airport.Latitude, airport.Longitude, 1);

                if (minDistance == -1 || distance < minDistance)
                {
                    minDistance = distance;
                    nearestICAO = airport.Icao;
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

        private async Task<List<FboHistoricalDataModel>> GetHistoricalDataAssociatedWithFbo(int groupId, int fboId, AirportWatchHistoricalDataRequest request)
        {
            var fboIcao = await _fboService.GetFBOIcao(fboId);

            var historicalData = await(from awhd in _context.AirportWatchHistoricalData
                                       join awat in _context.AirportWatchAircraftTailNumber on new { awhd.AircraftHexCode, awhd.AtcFlightNumber } equals new { awat.AircraftHexCode, awat.AtcFlightNumber }
                                       join ca in (
                                           from ca in _context.CustomerAircrafts
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
                                           }
                                       ) on awat.AtcFlightNumber equals ca.TailNumber
                                       into leftJoinedCustomers
                                       from ca in leftJoinedCustomers.DefaultIfEmpty()
                                       where
                                           awhd.AirportICAO == fboIcao &&
                                           (request.StartDateTime == null || awhd.AircraftPositionDateTimeUtc >= request.StartDateTime.Value.ToUniversalTime()) &&
                                           (request.EndDateTime == null || awhd.AircraftPositionDateTimeUtc <= request.EndDateTime.Value.ToUniversalTime().AddDays(1))
                                       group awhd by new
                                       {
                                           AirportWatchHistoricalDataID = awhd.Oid,
                                           awhd.AircraftHexCode,
                                           awhd.AtcFlightNumber,
                                           awhd.AircraftPositionDateTimeUtc,
                                           awhd.AircraftStatus,
                                           awhd.AirportICAO,
                                           awhd.AircraftTypeCode,
                                           ca.Company,
                                           ca.CustomerId,
                                           ca.TailNumber,
                                           ca.AircraftId,
                                           ca.CustomerInfoByGroupID,
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
                                       }).ToListAsync();
            
            return historicalData;
        }
    }

    class FboHistoricalDataModel
    {
        public int AirportWatchHistoricalDataID { get; set; }
        public string AircraftHexCode { get; set; }
        public string AtcFlightNumber { get; set; }
        public DateTime AircraftPositionDateTimeUtc { get; set; }
        public AirportWatchHistoricalData.AircraftStatusType AircraftStatus { get; set; }
        public string AircraftTypeCode { get; set; }
        public string Company { get; set; }
        public int CustomerId { get; set; }
        public string TailNumber { get; set; }
        public int AircraftId { get; set; }
        public string AirportICAO { get; set; }
        public int CustomerInfoByGroupID { get; set; }
    }
}
