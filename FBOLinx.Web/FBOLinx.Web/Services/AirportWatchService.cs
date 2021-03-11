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

namespace FBOLinx.Web.Services
{
    public class AirportWatchService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private readonly AircraftService _aircraftService;
        private readonly FboService _fboService;

        public AirportWatchService(FboLinxContext context, DegaContext degaContext,  AircraftService aircraftService, FboService fboService)
        {
            _context = context;
            _degaContext = degaContext;
            _aircraftService = aircraftService;
            _fboService = fboService;
        }

        public async Task<List<AirportWatchLiveData>> GetAirportWatchLiveData()
        {
            var timelimit = DateTime.UtcNow.AddMinutes(-5);

            var filteredResult = await _context.AirportWatchLiveData
                .Where(x => x.AircraftPositionDateTimeUtc >= timelimit)
                .OrderBy(x => x.AircraftPositionDateTimeUtc)
                .ThenBy(x => x.AircraftHexCode)
                .ThenBy(x => x.AtcFlightNumber)
                .ThenBy(x => x.GpsAltitude)
                .ToListAsync();

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
            var airportPositions = await GetAirportPositions();

            foreach (var record in data)
            {
                var oldAirportWatchLiveData = await _context.AirportWatchLiveData
                    .Where(aw => aw.AircraftHexCode == record.AircraftHexCode && aw.AtcFlightNumber == record.AtcFlightNumber)
                    .FirstOrDefaultAsync();

                if (oldAirportWatchLiveData == null)
                {
                    _context.AirportWatchAircraftTailNumber.Add(new AirportWatchAircraftTailNumber
                    {
                        AircraftHexCode = record.AircraftHexCode,
                        AtcFlightNumber = record.AtcFlightNumber,
                    });
                    // Add the most recent records for the "live" view
                    _context.AirportWatchLiveData.Add(record);
                }
                else
                {
                    AirportWatchLiveData.CopyEntity(oldAirportWatchLiveData, record);
                    _context.AirportWatchLiveData.Update(oldAirportWatchLiveData);
                }

                var oldAirportWatchHistoricalData = _context.AirportWatchHistoricalData
                    .Where(aw => aw.AircraftHexCode == record.AircraftHexCode && aw.AtcFlightNumber == record.AtcFlightNumber)
                    .OrderByDescending(aw => aw.AircraftPositionDateTimeUtc)
                    .FirstOrDefault();

                var airportWatchHistoricalData = AirportWatchHistoricalData.ConvertFromAirportWatchLiveData(record);
                airportWatchHistoricalData.AirportICAO = GetNearestICAO(airportPositions, record.Latitude, record.Longitude);

                if (oldAirportWatchHistoricalData == null ||
                    oldAirportWatchHistoricalData.IsAircraftOnGround != record.IsAircraftOnGround)
                {
                    _context.AirportWatchHistoricalData.Add(airportWatchHistoricalData);
                }
                else if (!oldAirportWatchHistoricalData.IsAircraftOnGround && !record.IsAircraftOnGround)
                {
                    AirportWatchHistoricalData.CopyEntity(oldAirportWatchHistoricalData, airportWatchHistoricalData);
                    _context.AirportWatchHistoricalData.Update(oldAirportWatchHistoricalData);
                }
                else {
                    AddPossibleParkingOccurrence(oldAirportWatchHistoricalData, airportWatchHistoricalData);
                }
            }

            await _context.SaveChangesAsync();
        }

        private void AddPossibleParkingOccurrence(AirportWatchHistoricalData oldAirportWatchHistoricalData, AirportWatchHistoricalData airportWatchHistoricalData)
        {
            // Parking occurrences

            //First confirm the last record we are comparing with was a landing or a parking
            if (oldAirportWatchHistoricalData.AircraftStatus != AirportWatchHistoricalData.AircraftStatusType.Landing &&
                oldAirportWatchHistoricalData.AircraftStatus != AirportWatchHistoricalData.AircraftStatusType.Parking)
                return;
            // If neither the old record nor the new one are on the ground then it can't be a parking
            if (!oldAirportWatchHistoricalData.IsAircraftOnGround || !airportWatchHistoricalData.IsAircraftOnGround)
                return;

            //If the aircraft has not moved then do not update the parking record - we want to keep the old record when it first stopped moving
            if (Math.Abs(airportWatchHistoricalData.Latitude - oldAirportWatchHistoricalData.Latitude) <= 0.000001 ||
                Math.Abs(airportWatchHistoricalData.Longitude - oldAirportWatchHistoricalData.Longitude) <= 0.000001)
                return;
            //Last record was over 10 minutes ago and the aircraft hasn't moved since that point - keep this old record and don't update
            if (oldAirportWatchHistoricalData.AircraftPositionDateTimeUtc < DateTime.UtcNow.AddMinutes(-10))
                return;
            //The aircraft has moved since landing - this should be an updated parking record
            oldAirportWatchHistoricalData.AircraftStatus = AirportWatchHistoricalData.AircraftStatusType.Parking;
            AirportWatchHistoricalData.CopyEntity(oldAirportWatchHistoricalData, airportWatchHistoricalData);
            _context.AirportWatchHistoricalData.Update(oldAirportWatchHistoricalData);
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
