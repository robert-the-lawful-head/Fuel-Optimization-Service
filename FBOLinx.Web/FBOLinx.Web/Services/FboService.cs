using FBOLinx.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using Geolocation;
using FBOLinx.Web.Models.Requests;
using System.IO;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.Services
{
    public class FboService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private readonly FilestorageContext _fileStorageContext;
        private readonly IServiceProvider _services;
        public FboService(FboLinxContext context, DegaContext degaContext, IServiceProvider services, FilestorageContext fileStorageContext)
        {
            _context = context;
            _degaContext = degaContext;
            _services = services;
            _fileStorageContext = fileStorageContext;
        }

        public async Task DoLegacyGroupTransition(int groupId)
        {
            var customerMargins = await (from cm in _context.CustomerMargins
                                         join pt in _context.PricingTemplate on new { cm.TemplateId, marginType = (short)1 } equals new { TemplateId = pt.Oid, marginType = (short)pt.MarginType }
                                         where cm.Amount < 0
                                         select cm).ToListAsync();
            foreach (var cm in customerMargins)
            {
                cm.Amount = -cm.Amount;
            }
            _context.CustomerMargins.UpdateRange(customerMargins);
            await _context.SaveChangesAsync();

            var deleteAircraftPricesTask = Task.Run(async () =>
            {
                using (var scope = _services.CreateScope())
                {
                    var groupTransitionService = scope.ServiceProvider.GetRequiredService<GroupTransitionService>();
                    await groupTransitionService.PerformLegacyGroupTransition(groupId);
                }
            });
        }

        public async Task<FBOLinx.DB.Models.Fbos> GetFbo(int fboId)
        {
            var result = await _context.Fbos.Where(x => x.Oid == fboId).Include(x => x.Group)
                .Include(x => x.fboAirport).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Coordinate> GetFBOLocation(int fboid)
        {
            var fboAirport = await _context.Fboairports.Where(fa => fa.Fboid == fboid).FirstOrDefaultAsync();
            if (fboAirport == null)
                return new Coordinate(0, 0);
            var airport = await _degaContext.AcukwikAirports.Where(a => a.Icao == fboAirport.Icao).FirstOrDefaultAsync();

            var latDirection = airport.Latitude.Substring(0, 1);
            var lngDirection = airport.Longitude.Substring(0, 1);

            double latitude = double.Parse(airport.Latitude.Substring(1, 2)) + double.Parse(airport.Latitude.Substring(4, 2)) / 60 + double.Parse(airport.Latitude[7..]) / 3600;
            double longitude = airport.Longitude.Length == 8 ?
                double.Parse(airport.Longitude.Substring(1, 2)) + double.Parse(airport.Longitude.Substring(4, 2)) / 60 + double.Parse(airport.Longitude[6..]) / 3600 :
                double.Parse(airport.Longitude.Substring(1, 3)) + double.Parse(airport.Longitude.Substring(5, 2)) / 60 + double.Parse(airport.Longitude[7..]) / 3600;

            if (latDirection != "N") latitude = -latitude;
            if (lngDirection != "E") longitude = -longitude;

            return new Coordinate(latitude, longitude);
        }

        public async Task<string> UploadLogo(FboLogoRequest fboLogoRequest)
        {
            var imageAsArray = Convert.FromBase64String(fboLogoRequest.FileData);

            var existingRecord = await _fileStorageContext.FboLinxImageFileData.Where(f => f.FboId == fboLogoRequest.FboId).ToListAsync();
            if (existingRecord.Count > 0)
            {
                existingRecord[0].FileData = imageAsArray;
                existingRecord[0].FileName = fboLogoRequest.FileName;
                existingRecord[0].ContentType = fboLogoRequest.ContentType;
                _fileStorageContext.FboLinxImageFileData.Update(existingRecord[0]);
            }
            else
            {
                FBOLinx.DB.Models.FboLinxImageFileData fboLinxImageFileData = new DB.Models.FboLinxImageFileData();
                fboLinxImageFileData.FileData = imageAsArray;
                fboLinxImageFileData.FileName = fboLogoRequest.FileName;
                fboLinxImageFileData.ContentType = fboLogoRequest.ContentType;
                fboLinxImageFileData.FboId = fboLogoRequest.FboId;
                _fileStorageContext.FboLinxImageFileData.Add(fboLinxImageFileData);
            }

            await _fileStorageContext.SaveChangesAsync();

            var imageBase64 = Convert.ToBase64String(imageAsArray, 0, imageAsArray.Length);
            return fboLogoRequest.ContentType + ";base64," + imageBase64;
        }

        public async Task<string> GetLogo(int fboId)
        {
            var existingRecord = await _fileStorageContext.FboLinxImageFileData.Where(f => f.FboId == fboId).ToListAsync();
            if (existingRecord.Count > 0)
            {
                var imageBase64 = Convert.ToBase64String(existingRecord[0].FileData, 0, existingRecord[0].FileData.Length);
                return existingRecord[0].ContentType + ";base64," + imageBase64;
            }

            return "";
        }

        public async Task DeleteLogo(int fboId)
        {
            var existingRecord = await _fileStorageContext.FboLinxImageFileData.Where(f => f.FboId == fboId).SingleOrDefaultAsync();
            if (existingRecord.Oid > 0)
            {
                _fileStorageContext.FboLinxImageFileData.Remove(existingRecord);
                await _fileStorageContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetFBOIcao(int fboId)
        {
            var fboAirport = await _context.Fboairports.Where(fa => fa.Fboid == fboId).FirstOrDefaultAsync();

            return fboAirport.Icao;
        }

        public async Task<DateTime> GetAirportLocalDateTimeByUtcFboId(DateTime utcDateTime, int fboId)
        {
            var fboAcukwikId = await (from f in _context.Fbos.Where(f => f.Oid == fboId) select f.AcukwikFBOHandlerId).FirstOrDefaultAsync();

            var acukwikAirport = await (from afh in _degaContext.AcukwikFbohandlerDetail
                                        join aa in _degaContext.AcukwikAirports on afh.AirportId equals aa.AirportId
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
                                        join aa in _degaContext.AcukwikAirports on afh.AirportId equals aa.AirportId
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
                                        join aa in _degaContext.AcukwikAirports on afh.AirportId equals aa.AirportId
                                        where afh.HandlerId == fboAcukwikId
                                        select aa).FirstOrDefaultAsync();

            if (acukwikAirport == null)
                return "";

            return Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTimeZone(acukwikAirport.IntlTimeZone, acukwikAirport.DaylightSavingsYn == "Y" ? true : false);
        }
    }
}
