using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
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

        public async Task<List<AirportWatchHistoricalDataResponse>> GetHistoricalData(int groupId)
        {
            var allAircrafts = await _aircraftService.GetAllAircrafts();

            var customersData = await (from awat in _context.AirportWatchAircraftTailNumber
                                       join ca in _context.CustomerAircrafts on awat.AtcFlightNumber equals ca.TailNumber
                                       join cig in _context.CustomerInfoByGroup on new { ca.CustomerId, GroupId = ca.GroupId ?? 0 } equals new { cig.CustomerId, cig.GroupId }
                                       where ca.GroupId == groupId
                                       select new
                                       {
                                           awat.AircraftHexCode,
                                           cig.Company,
                                           ca.TailNumber,
                                           ca.AircraftId,
                                       }).ToListAsync();

            var historicalData = await _context.AirportWatchHistoricalData.ToListAsync();

            var result = (from awhd in historicalData
                          join ca in customersData on awhd.AircraftHexCode equals ca.AircraftHexCode
                          join a in allAircrafts on ca.AircraftId equals a.AircraftId
                          select new AirportWatchHistoricalDataResponse
                          {
                              Company = ca.Company,
                              DateTime = awhd.AircraftPositionDateTimeUtc,
                              TailNumber = ca.TailNumber,
                              FlightNumber = awhd.AtcFlightNumber,
                              HexCode = awhd.AircraftHexCode,
                              AircraftType = a.Model,
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
                if (oldAirportWatchHistoricalData == null || oldAirportWatchLiveData.IsAircraftOnGround != record.IsAircraftOnGround)
                {
                    _context.AirportWatchHistoricalData.Add(airportWatchHistoricalData);
                } else
                {
                    // Parking occurrences
                    if (oldAirportWatchHistoricalData.IsAircraftOnGround == true &&
                        record.IsAircraftOnGround == true &&
                        record.AircraftPositionDateTimeUtc >= oldAirportWatchHistoricalData.AircraftPositionDateTimeUtc.AddMinutes(5))
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
