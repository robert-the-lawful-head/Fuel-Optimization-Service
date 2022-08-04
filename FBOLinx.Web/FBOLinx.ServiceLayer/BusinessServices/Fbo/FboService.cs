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
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Requests.FBO;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.DB.Specifications.Fbo;
using Geolocation;
using Microsoft.Extensions.DependencyInjection;

namespace FBOLinx.ServiceLayer.BusinessServices.Fbo
{
    public interface IFboService : IBaseDTOService<FbosDto, Fbos>
    {
        Task<DateTime> GetAirportLocalDateTimeByUtcFboId(DateTime utcDateTime, int fboId);
        Task<DateTime> GetAirportLocalDateTimeByFboId(int fboId);
        Task<string> GetAirportTimeZoneByFboId(int fboId);
        Task<Fbos> GetFbo(int fboId);
        Task<Coordinate> GetFBOLocation(int fboid);
        Task DoLegacyGroupTransition(int groupId);
        Task<string> UploadLogo(FboLogoRequest fboLogoRequest);
        Task<string> GetLogo(int fboId);
        Task DeleteLogo(int fboId);
        Task<string> GetFBOIcao(int fboId);
        Task<List<string>> GetToEmailsForEngagementEmails(int fboId);
        Task<List<Fbos>> GetFbosByIcaos(string icaos);
        Task<List<FbosDTO>> GetFbosByIcaos(string icaos);
        Task<List<string>> GetToEmailsForEngagementEmails(int fboId);
        Task NotifyFboNoPrices(List<string> toEmails, string fbo, string customerName);
        Task<FbosDTO> GetFboByFboId(int fboId);
    }
    public class FboService : BaseDTOService<FbosDto, DB.Models.Fbos, FboLinxContext>, IFboService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private IMailService _MailService;
        private IFboEntityService _fboEntityService;
        private IFboContactsEntityService _fboContactsEntityService;
        private readonly IAcukwikFbohandlerDetailEntityService _AcukwikFbohandlerDetailEntityService;

        public FboService(IFboEntityService fboEntityService, FboLinxContext context, DegaContext degaContext, IMailService mailService, IFboContactsEntityService fboContactsEntityService,
            IAcukwikFbohandlerDetailEntityService acukwikFbohandlerDetailEntityService) : base(fboEntityService)
        private readonly FilestorageContext _fileStorageContext;
        private readonly IServiceProvider _services;

        public FboService(IFboEntityService fboEntityService, FboLinxContext context, DegaContext degaContext, IServiceProvider services, FilestorageContext fileStorageContext) : base(fboEntityService)
        {
            _fboEntityService = fboEntityService;
            _context = context;
            _degaContext = degaContext;
            _MailService = mailService;
            _fboContactsEntityService = fboContactsEntityService;
            _AcukwikFbohandlerDetailEntityService = acukwikFbohandlerDetailEntityService;
            _services = services;
            _fileStorageContext = fileStorageContext;
        }
        
        public async Task<DateTime> GetAirportLocalDateTimeByUtcFboId(DateTime utcDateTime, int fboId)
        {
            var fboAcukwikId = await _fboEntityService.GetFboAcukwikId(fboId);

            var acukwikAirport = await _AcukwikFbohandlerDetailEntityService.GetAcukwikAirport(fboAcukwikId);

            if (acukwikAirport == null)
                return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

            var result = Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTime(utcDateTime, acukwikAirport.IntlTimeZone, acukwikAirport.DaylightSavingsYn == "Y" ? true : false);
            result = DateTime.SpecifyKind(result, DateTimeKind.Unspecified);
            return result;
        }

        public async Task<Fbos> GetFbo(int fboId)
        {
            var result = await _context.Fbos.Where(x => x.Oid == fboId).Include(x => x.Group)
                .Include(x => x.FboAirport).FirstOrDefaultAsync();
            return result;
        }

        public async Task<DateTime> GetAirportLocalDateTimeByFboId(int fboId)
        {
            var fboAcukwikId = await _fboEntityService.GetFboAcukwikId(fboId);

            var acukwikAirport = await _AcukwikFbohandlerDetailEntityService.GetAcukwikAirport(fboAcukwikId);

            if (acukwikAirport == null)
                return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

            var result = Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTimeNow(acukwikAirport.IntlTimeZone, acukwikAirport.DaylightSavingsYn == "Y" ? true : false);
            result = DateTime.SpecifyKind(result, DateTimeKind.Unspecified);
            return result;
        }

        public async Task<string> GetAirportTimeZoneByFboId(int fboId)
        {
            var fboAcukwikId = await _fboEntityService.GetFboAcukwikId(fboId);

            var acukwikAirport = await _AcukwikFbohandlerDetailEntityService.GetAcukwikAirport(fboAcukwikId);

            if (acukwikAirport == null)
                return "";

            var timeZone = Core.Utilities.DatesAndTimes.DateTimeHelper.GetLocalTimeZone(acukwikAirport.IntlTimeZone, acukwikAirport.AirportCity);

            if (timeZone == "")
                timeZone = "UTC" + acukwikAirport.IntlTimeZone;

            return timeZone;
        }

        public async Task<List<FbosDTO>> GetFbosByIcaos(string icaos)
        {
            var fbos = await _fboEntityService.GetFbosByIcaos(icaos);

            return fbos;
        }

        public async Task<List<string>> GetToEmailsForEngagementEmails(int fboId)
        {
            List<string> toEmails = new List<string>();

            var fboInfo = await _fboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));

            if (!string.IsNullOrEmpty(fboInfo.FuelDeskEmail))
                toEmails.Add(fboInfo.FuelDeskEmail);

            var fboContacts = await _fboContactsEntityService.GetFboContactsByFboId(fboId);

            foreach (ContactsDTO fboContact in fboContacts)
            {
                if (fboContact.CopyAlerts.HasValue && fboContact.CopyAlerts.Value && !string.IsNullOrEmpty(fboContact.Email))
                    toEmails.Add(fboContact.Email);
            }

            return toEmails;
        }

        public async Task NotifyFboNoPrices(List<string> toEmails, string fbo, string customerName)
        {
            FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
            mailMessage.From = new MailAddress("donotreply@fbolinx.com");
            foreach (string email in toEmails)
            {
                if (_MailService.IsValidEmailRecipient(email))
                    mailMessage.To.Add(email);
            }

            var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridMissedQuoteTemplateData
            {
                fboName = fbo,
                customerName = customerName,
                subject = "Missed opportunity for " + customerName
            };

            mailMessage.SendGridMissedQuoteTemplateData = dynamicTemplateData;

            //Send email
            var result = await _MailService.SendAsync(mailMessage);
        }

        public async Task<FbosDTO> GetFboByFboId(int fboId)
        {
            var fbo = await _fboEntityService.GetFboByFboId(fboId);
            return fbo;
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

        public async Task<Fbos> GetFboAsNoTracing(int fboId)
        {
            var result = await _context.Fbos.Where(x => x.Oid == fboId).Include(x => x.Group)
                .Include(x => x.FboAirport).AsNoTracking().FirstOrDefaultAsync();
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
        
        public async Task<List<string>> GetToEmailsForEngagementEmails(int fboId)
        {
            List<string> toEmails = new List<string>();

            var fboInfo = await _context.Fbos.FindAsync(fboId);
            //var responseFbo = await _apiClient.GetAsync("fbos/" + fbo.Oid, conductorUser.Token);
            //var fboInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Fbos>(responseFbo);

            if (!string.IsNullOrEmpty(fboInfo.FuelDeskEmail))
                toEmails.Add(fboInfo.FuelDeskEmail);

            var fboContacts = await _context.Fbocontacts
                                .Include("Contact")
                                .Where(x => x.Fboid == fboId && !string.IsNullOrEmpty(x.Contact.Email))
                                .Select(f => new Contacts
                                {
                                    FirstName = f.Contact.FirstName,
                                    LastName = f.Contact.LastName,
                                    Title = f.Contact.Title,
                                    Oid = f.Oid,
                                    Email = f.Contact.Email,
                                    Primary = f.Contact.Primary,
                                    CopyAlerts = f.Contact.CopyAlerts,
                                    CopyOrders = f.Contact.CopyOrders
                                })
                                .ToListAsync();
            //var responseFboContacts = await _apiClient.GetAsync("fbocontacts/fbo/" + fbo.Oid, conductorUser.Token);
            //var fboContacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FboContactsViewModel>>(responseFboContacts);

            foreach (Contacts fboContact in fboContacts)
            {
                if (fboContact.CopyAlerts.GetValueOrDefault() && !string.IsNullOrEmpty(fboContact.Email))
                    toEmails.Add(fboContact.Email);
            }

            return toEmails;
        }

        public async Task<List<Fbos>> GetFbosByIcaos(string icaos)
        {
            var fbos = await (from f in _context.Fbos
                              join fa in _context.Fboairports on f.Oid equals fa.Fboid
                              where icaos.Contains(fa.Icao) && f.GroupId > 1 && f.Active == true
                              select f).ToListAsync();
            return fbos;
        }
    }
}
