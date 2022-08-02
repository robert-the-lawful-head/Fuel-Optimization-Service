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
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;
using FBOLinx.ServiceLayer.BusinessServices.Mail;

namespace FBOLinx.ServiceLayer.BusinessServices.Fbo
{
    public interface IFboService : IBaseDTOService<FbosDto, Fbos>
    {
        Task<DateTime> GetAirportLocalDateTimeByUtcFboId(DateTime utcDateTime, int fboId);
        Task<DateTime> GetAirportLocalDateTimeByFboId(int fboId);
        Task<string> GetAirportTimeZoneByFboId(int fboId);
        Task<List<Fbos>> GetFbosByIcaos(string icaos);
        Task<List<string>> GetToEmailsForEngagementEmails(int fboId);
        Task NotifyFboNoPrices(List<string> toEmails, string fbo, string customerName);
    }
    public class FboService : BaseDTOService<FbosDto, DB.Models.Fbos, FboLinxContext>, IFboService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private IMailService _MailService;
        public FboService(IFboEntityService fboEntityService, FboLinxContext context, DegaContext degaContext, IMailService mailService) : base(fboEntityService)
        {
            _context = context;
            _degaContext = degaContext;
            _MailService = mailService;
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

        public async Task<List<Fbos>> GetFbosByIcaos(string icaos)
        {
            var fbos = await (from f in _context.Fbos
                              join fa in _context.Fboairports on f.Oid equals fa.Fboid
                              where icaos.Contains(fa.Icao) && f.GroupId > 1 && f.Active == true
                              select f).ToListAsync();
            return fbos;
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
    }
}
