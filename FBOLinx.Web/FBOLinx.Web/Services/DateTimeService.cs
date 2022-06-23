using FBOLinx.Core.Utilities.DatesAndTimes;
using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class DateTimeService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        public DateTimeService(FboLinxContext context, DegaContext degaContext)
        {
            _context = context;
            _degaContext = degaContext;
        }

        public async Task<DateTime> ConvertLocalTimeToUtc(int fboId, DateTime localTime)
        {
            var fboAcukwikId = await (from f in _context.Fbos where f.Oid == fboId select f.AcukwikFBOHandlerId).FirstOrDefaultAsync();

            var acukwikAirport = await (from afh in _degaContext.AcukwikFbohandlerDetail
                                       join aa in _degaContext.AcukwikAirports on afh.AirportId equals aa.Id
                                       where afh.HandlerId == fboAcukwikId
                                       select aa).FirstOrDefaultAsync();

            if (acukwikAirport == null)
                return DateTime.UtcNow;

            return DateTimeHelper.GetUtcTime(localTime, acukwikAirport.IntlTimeZone, acukwikAirport.DaylightSavingsYn == "Y" ? true : false);
        }

        public DateTime GetNextTuesdayDate(DateTime date)
        {
            return DateTimeHelper.GetNextTuesdayDate(date);
        }
    }
}
