using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FBOLinx.Core.Utilities;
using FBOLinx.Core.Utilities.Extensions;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;

namespace FBOLinx.Web.Services
{
    public class ResetPasswordService
    {
        private IMailService _MailService;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private MailTemplateService _MailTemplateService;

        public ResetPasswordService(IMailService mailService, FboLinxContext context, IHttpContextAccessor httpContextAccessor, MailTemplateService mailTemplateService)
        {
            _MailTemplateService = mailTemplateService;
            _HttpContextAccessor = httpContextAccessor;
            _MailService = mailService;
        }

        #region Public Methods
        public async Task SendResetPasswordEmailAsync(string name, string emailAddress, string token)
        {
            if (string.IsNullOrEmpty(emailAddress) || !(_MailService.IsValidEmailRecipient(emailAddress)))
                return;

            FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
            string body = GetResetPasswordEmailTemplate();
            body = body.Replace("%USERNAME%", name);
            body = body.Replace("%RESETPASSWORDLINK%", _HttpContextAccessor.HttpContext.Request.Scheme + "://" + _HttpContextAccessor.HttpContext.Request.Host + "/reset-password?token=" + token);
            mailMessage.From = new MailAddress("donotreply@fbolinx.com");
            mailMessage.To.Add(new MailAddress(emailAddress));
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Reset Your Password";

            await _MailService.SendAsync(mailMessage);
        }
        #endregion

        #region Private Methods
        private string GetResetPasswordEmailTemplate()
        {
            return _MailTemplateService.GetTemplatesFileContent("ResetPassword",
                "ResetPasswordBody.html");
        }
        #endregion
    }
}
