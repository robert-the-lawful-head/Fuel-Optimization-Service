using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using Fuelerlinx.SDK;

namespace FBOLinx.ServiceLayer.BusinessServices.Airport
{
    public interface IAirportTimeService
    {
        Task<DateTime> GetAirportLocalDateTime(int fboId, DateTime? utcDateTime = null);
        Task<DateTime> GetAirportLocalDateTime(string icao, DateTime? utcDateTime = null);
        Task<string> GetAirportTimeZone(string icao);
    }

    public class AirportTimeService : IAirportTimeService
    {
        private IAirportService _AirportService;

        public AirportTimeService(IAirportService airportService)
        {
            _AirportService = airportService;
        }

        public async Task<DateTime> GetAirportLocalDateTime(int fboId, DateTime? utcDateTime = null)
        {
            var airportPosition = await _AirportService.GetAirportPositionForFbo(fboId);
            if (airportPosition == null)
                return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            return await GetAirportLocalDateTime(airportPosition.Icao, utcDateTime);
        }

        public async Task<DateTime> GetAirportLocalDateTime(string icao, DateTime? utcDateTime = null)
        {
            var airport = await GetAirportGeneralInformation(icao);
            
            if (airport == null)
                return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

            DateTime? result = null;
            if (!utcDateTime.HasValue)
                result = Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTimeNow(airport.IntlTimeZone,
                    airport.RespectDaylightSavings);
            else
                result = Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTime(utcDateTime.Value,
                    airport.IntlTimeZone, airport.RespectDaylightSavings);
            result = DateTime.SpecifyKind(result.GetValueOrDefault(), DateTimeKind.Unspecified);
            return result.GetValueOrDefault();
        }

        public async Task<string> GetAirportTimeZone(string icao)
        {
            var airport = await GetAirportGeneralInformation(icao);

            if (airport == null)
                return "";

            var timeZone = Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTimeZone(airport.IntlTimeZone, airport.AirportCity);

            if (timeZone == "")
                timeZone = "UTC" + airport.IntlTimeZone;

            return timeZone;
        }

        private async Task<GeneralAirportInformation> GetAirportGeneralInformation(string icao)
        {
            return await _AirportService.GetGeneralAirportInformation(icao);
        }
    }
}
