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
using FBOLinx.DB.Specifications.Fbo;

namespace FBOLinx.ServiceLayer.BusinessServices.Fbo
{
    public interface IFboService : IBaseDTOService<FbosDto, Fbos>
    {
        Task<DateTime> GetAirportLocalDateTimeByUtcFboId(DateTime utcDateTime, int fboId);
        Task<DateTime> GetAirportLocalDateTimeByFboId(int fboId);
        Task<string> GetAirportTimeZoneByFboId(int fboId);
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
        {
            _fboEntityService = fboEntityService;
            _context = context;
            _degaContext = degaContext;
            _MailService = mailService;
            _fboContactsEntityService = fboContactsEntityService;
            _AcukwikFbohandlerDetailEntityService = acukwikFbohandlerDetailEntityService;
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
    }
}
