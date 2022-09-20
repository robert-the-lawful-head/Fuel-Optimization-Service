using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;
using FBOLinx.DB.Projections.AirportWatch;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public class AirportWatchHistoricalDataByIcaoSpecification : Specification<AirportWatchHistoricalData, AirportWatchHistoricalDataSimplifiedProjection>
    {
        public AirportWatchHistoricalDataByIcaoSpecification(string icao, DateTime startDateTimeUtc, DateTime endDateTimeUtc, List<string> tailNumbers = null) : base(awhd =>
            (awhd.AirportICAO == icao)
        && (awhd.AircraftPositionDateTimeUtc >= startDateTimeUtc)
            && (awhd.AircraftPositionDateTimeUtc <= endDateTimeUtc)
            && (tailNumbers == null || tailNumbers.Contains(awhd.TailNumber)),
            x => new AirportWatchHistoricalDataSimplifiedProjection()
            {
                Oid = x.Oid,
                AircraftHexCode = x.AircraftHexCode,
                AtcFlightNumber = x.AtcFlightNumber,
                AircraftPositionDateTimeUtc = x.AircraftPositionDateTimeUtc,
                AircraftStatus = x.AircraftStatus,
                AirportICAO = x.AirportICAO,
                AircraftTypeCode = x.AircraftTypeCode,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                TailNumber = x.TailNumber
            })
        {
        }
    }
}
