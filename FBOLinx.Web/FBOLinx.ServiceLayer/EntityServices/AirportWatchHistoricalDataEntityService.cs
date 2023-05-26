using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AirportWatchHistoricalDataEntityService : Repository<AirportWatchHistoricalData, FboLinxContext>
    {
        public AirportWatchHistoricalDataEntityService(FboLinxContext context) : base(context)
        {
        }

        public async Task<List<AirportWatchHistoricalData>> GetHistoricalData(DateTime aircraftPositionStateDateUtc, DateTime aircraftPositionEndDateUtc,
            List<string> airportIcaos = null, List<string> aircraftHexCodes = null, List<string> tailNumbers = null, List<string> atcFlightNumbers = null)
        {
            var query = GetHistoricalDataQueryable(aircraftPositionStateDateUtc, aircraftPositionEndDateUtc, airportIcaos, aircraftHexCodes, tailNumbers, atcFlightNumbers);
            return await query.ToListAsync();
        }

        private IQueryable<AirportWatchHistoricalData> GetHistoricalDataQueryable(DateTime aircraftPositionStateDateUtc, DateTime aircraftPositionEndDateUtc,
            List<string> airportIcaos = null, List<string> aircraftHexCodes = null, List<string> tailNumbers = null, List<string> atcFlightNumbers = null)
        {
            //We use a projected version of AirportWatchHistoricalData that omits a few of the unused columns.  
            //This is done because the indexes are also ignoring these columns so it significantly improves performance.
            var query = (from hd in context.AirportWatchHistoricalData
                    join icao in context.AsTable(airportIcaos) on hd.AirportICAO equals icao.Value
                        into icaoJoin
                    from icao in icaoJoin.DefaultIfEmpty()
                    join hexCode in context.AsTable(aircraftHexCodes) on hd.AircraftHexCode equals hexCode.Value
                        into hexCodeJoin
                    from hexCode in hexCodeJoin.DefaultIfEmpty()
                    join tailNumber in context.AsTable(tailNumbers) on hd.TailNumber equals tailNumber.Value
                        into tailNumberJoin
                    from tailNumber in tailNumberJoin.DefaultIfEmpty()
                    join atcFlightNumber in context.AsTable(atcFlightNumbers) on hd.AtcFlightNumber equals
                        atcFlightNumber.Value
                        into atcFlightNumberJoin
                    from atcFlightNumber in atcFlightNumberJoin.DefaultIfEmpty()

                    where hd.AircraftPositionDateTimeUtc >= aircraftPositionStateDateUtc
                          && hd.AircraftPositionDateTimeUtc <= aircraftPositionEndDateUtc
                          && (airportIcaos == null || !string.IsNullOrEmpty(icao.Value))
                          && (aircraftHexCodes == null || !string.IsNullOrEmpty(hexCode.Value))
                          && (tailNumbers == null || !string.IsNullOrEmpty(tailNumber.Value))
                          && (atcFlightNumbers == null || !string.IsNullOrEmpty(atcFlightNumber.Value))
                    select new AirportWatchHistoricalData()
                    {
                        Oid = 0,
                        AirportICAO = hd.AirportICAO,
                        AircraftHexCode = hd.AircraftHexCode,
                        TailNumber = hd.TailNumber,
                        AtcFlightNumber = hd.AtcFlightNumber,
                        AircraftPositionDateTimeUtc = hd.AircraftPositionDateTimeUtc,
                        AircraftStatus = hd.AircraftStatus,
                        AircraftTypeCode = hd.AircraftTypeCode,
                        BoxName = hd.BoxName,
                        //BoxTransmissionDateTimeUtc = hd.BoxTransmissionDateTimeUtc,
                        //GpsAltitude = hd.GpsAltitude,
                        //GroundSpeedKts = hd.GroundSpeedKts,
                        //IsAircraftOnGround = hd.IsAircraftOnGround,
                        Latitude = hd.Latitude,
                        Longitude = hd.Longitude,
                        //TrackingDegree = hd.TrackingDegree,
                        //TransponderCode = hd.TransponderCode,
                        //VerticalSpeedKts = hd.VerticalSpeedKts,
                        //AltitudeInStandardPressure = hd.AltitudeInStandardPressure
                    }
                );
            return query;
        }
    }
}
