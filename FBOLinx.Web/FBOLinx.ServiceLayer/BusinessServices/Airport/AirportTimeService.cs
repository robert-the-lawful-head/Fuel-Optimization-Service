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
        Task<DateTime> GetAirportLocalDateTime(string icao, DateTime? utcDateTime = null);
        Task<string> GetAirportTimeZone(string icao);
    }

    public class AirportTimeService : IAirportTimeService
    {
        private FuelerLinxApiService _FuelerLinxApiService;
        private List<GeneralAirportInformation> _AirportGeneralInformationList;

        public AirportTimeService(FuelerLinxApiService fuelerLinxApiService)
        {
            _FuelerLinxApiService = fuelerLinxApiService;
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
            icao = icao?.ToUpper();

            if (_AirportGeneralInformationList == null)
                _AirportGeneralInformationList = await _FuelerLinxApiService.GetAllAirportGeneralInformation();

            return _AirportGeneralInformationList.FirstOrDefault(x => x.ProperAirportIdentifier.ToUpper() == icao);
        }
    }
}
