using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses.AirportWatch;
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
        private readonly AircraftService _aircraftService;

        public AirportWatchService(FboLinxContext context, AircraftService aircraftService)
        {
            _context = context;
            _aircraftService = aircraftService;
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

        public async Task<List<AirportWatchHistoricalDataResponse>> GetHistoricalData(int groupId, AirportWatchHistoricalDataRequest request)
        {
            var allAircrafts = await _aircraftService.GetAllAircrafts();

            var historicalData = await (from awhd in _context.AirportWatchHistoricalData
                                        join awat in _context.AirportWatchAircraftTailNumber on new { awhd.AircraftHexCode, awhd.AtcFlightNumber } equals new { awat.AircraftHexCode, awat.AtcFlightNumber }
                                        join ca in _context.CustomerAircrafts on awat.AtcFlightNumber equals ca.TailNumber
                                        join cig in _context.CustomerInfoByGroup on new { ca.CustomerId, GroupId = ca.GroupId ?? 0 } equals new { cig.CustomerId, cig.GroupId }
                                        where ca.GroupId == groupId &&
                                            awhd.AircraftPositionDateTimeUtc >= request.StartDateTime.ToUniversalTime() &&
                                            awhd.AircraftPositionDateTimeUtc <= request.EndDateTime.ToUniversalTime().AddDays(1)
                                        group awhd by new
                                        {
                                            AirportWatchHistoricalDataID = awhd.Oid,
                                            awat.AircraftHexCode,
                                            awat.AtcFlightNumber,
                                            awhd.AircraftPositionDateTimeUtc,
                                            awhd.AircraftStatus,
                                            cig.Company,
                                            ca.TailNumber,
                                            ca.AircraftId,
                                        }
                                        into groupedResult
                                        select new
                                        {
                                            groupedResult.Key.AirportWatchHistoricalDataID,
                                            groupedResult.Key.AircraftHexCode,
                                            groupedResult.Key.AtcFlightNumber,
                                            groupedResult.Key.AircraftPositionDateTimeUtc,
                                            groupedResult.Key.AircraftStatus,
                                            groupedResult.Key.Company,
                                            groupedResult.Key.TailNumber,
                                            groupedResult.Key.AircraftId,
                                        }).ToListAsync();

            var result = (from h in historicalData
                          join a in _aircraftService.GetAllAircraftsAsQueryable() on h.AircraftId equals a.AircraftId
                          select new AirportWatchHistoricalDataResponse
                          {
                              Company = h.Company,
                              DateTime = h.AircraftPositionDateTimeUtc,
                              TailNumber = h.TailNumber,
                              FlightNumber = h.AtcFlightNumber,
                              HexCode = h.AircraftHexCode,
                              AircraftType = a.Model,
                              Status = h.AircraftStatus,
                          }).ToList();
            return result;
        }

        public void ProcessAirportWatchData(List<AirportWatchLiveData> data)
        {
            foreach (var record in data)
            {
                var oldAirportWatchLiveData = _context.AirportWatchLiveData
                    .Where(aw => aw.AircraftHexCode == record.AircraftHexCode && aw.AtcFlightNumber == record.AtcFlightNumber)
                    .FirstOrDefault();

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
                if (oldAirportWatchHistoricalData == null || oldAirportWatchHistoricalData.IsAircraftOnGround != record.IsAircraftOnGround || oldAirportWatchHistoricalData.AircraftStatus == AirportWatchHistoricalData.AircraftStatusType.Parking)
                {
                    _context.AirportWatchHistoricalData.Add(airportWatchHistoricalData);
                }
                else
                {
                    // Parking occurrences
                    if (oldAirportWatchHistoricalData.IsAircraftOnGround == true &&
                        record.IsAircraftOnGround == true &&
                        record.AircraftPositionDateTimeUtc >= oldAirportWatchHistoricalData.AircraftPositionDateTimeUtc.AddMinutes(10))
                    {
                        oldAirportWatchHistoricalData.AircraftStatus = AirportWatchHistoricalData.AircraftStatusType.Parking;
                    }

                    AirportWatchHistoricalData.CopyEntity(oldAirportWatchHistoricalData, airportWatchHistoricalData);
                    _context.AirportWatchHistoricalData.Update(oldAirportWatchHistoricalData);
                }
            }

            _context.SaveChanges();
        }
    }
}
