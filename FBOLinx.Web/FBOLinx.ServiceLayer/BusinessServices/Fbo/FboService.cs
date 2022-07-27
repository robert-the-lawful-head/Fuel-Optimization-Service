using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Fbo
{
    public interface IFboService : IBaseDTOService<FbosDto, Fbos>
    {
        Task<DateTime> GetAirportLocalDateTimeByUtcFboId(DateTime utcDateTime, int fboId);
        Task<DateTime> GetAirportLocalDateTimeByFboId(int fboId);
        Task<string> GetAirportTimeZoneByFboId(int fboId);
    }
    public class FboService : BaseDTOService<FbosDto, DB.Models.Fbos, FboLinxContext>, IFboService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        public FboService(IFboEntityService fboEntityService, FboLinxContext context, DegaContext degaContext) : base(fboEntityService)
        {
            _context = context;
            _degaContext = degaContext;
        }
        public async Task<DateTime> GetAirportLocalDateTimeByUtcFboId(DateTime utcDateTime, int fboId)
        {
            var fboAcukwikId = await (from f in _context.Fbos.Where(f => f.Oid == fboId) select f.AcukwikFBOHandlerId).FirstOrDefaultAsync();

            var acukwikAirport = await (from afh in _degaContext.AcukwikFbohandlerDetail
                                        join aa in _degaContext.AcukwikAirports on afh.AirportId equals aa.Oid
                                        where afh.HandlerId == fboAcukwikId
                                        select aa).FirstOrDefaultAsync();

            if (acukwikAirport == null)
                return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

            var result = Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTime(utcDateTime, acukwikAirport.IntlTimeZone, acukwikAirport.DaylightSavingsYn == "Y" ? true : false);
            result = DateTime.SpecifyKind(result, DateTimeKind.Unspecified);
            return result;
        }

        public async Task<DateTime> GetAirportLocalDateTimeByFboId(int fboId)
        {
            var fboAcukwikId = await (from f in _context.Fbos.Where(f => f.Oid == fboId) select f.AcukwikFBOHandlerId).FirstOrDefaultAsync();

            var acukwikAirport = await (from afh in _degaContext.AcukwikFbohandlerDetail
                                        join aa in _degaContext.AcukwikAirports on afh.AirportId equals aa.Oid
                                        where afh.HandlerId == fboAcukwikId
                                        select aa).FirstOrDefaultAsync();

            if (acukwikAirport == null)
                return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

            var result = Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTimeNow(acukwikAirport.IntlTimeZone, acukwikAirport.DaylightSavingsYn == "Y" ? true : false);
            result = DateTime.SpecifyKind(result, DateTimeKind.Unspecified);
            return result;
        }

        public async Task<string> GetAirportTimeZoneByFboId(int fboId)
        {
            var fboAcukwikId = await (from f in _context.Fbos.Where(f => f.Oid == fboId) select f.AcukwikFBOHandlerId).FirstOrDefaultAsync();

            var acukwikAirport = await (from afh in _degaContext.AcukwikFbohandlerDetail
                                        join aa in _degaContext.AcukwikAirports on afh.AirportId equals aa.Oid
                                        where afh.HandlerId == fboAcukwikId
                                        select aa).FirstOrDefaultAsync();

            if (acukwikAirport == null)
                return "";

            var timeZone = Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTimeZone(acukwikAirport.IntlTimeZone, acukwikAirport.AirportCity);

            if (timeZone == "")
                timeZone = "UTC" + acukwikAirport.IntlTimeZone;

            return timeZone;
        }
    }
}
