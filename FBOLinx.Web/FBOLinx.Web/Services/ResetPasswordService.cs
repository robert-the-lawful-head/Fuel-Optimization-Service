using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FBOLinx.Web.Configurations;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using SendGrid.Helpers.Mail;

namespace FBOLinx.Web.Services
{
    public class ResetPasswordService
    {
        private FBOLinx.Web.Configurations.MailSettings _MailSettings;
        private FboLinxContext _Context;
        private IFileProvider _FileProvider;
        private IHttpContextAccessor _HttpContextAccessor;

        public ResetPasswordService(FBOLinx.Web.Configurations.MailSettings mailSettings, FboLinxContext context, IFileProvider fileProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _HttpContextAccessor = httpContextAccessor;
            _FileProvider = fileProvider;
            _Context = context;
            _MailSettings = mailSettings;
        }

        #region Public Methods
        public async Task SendResetPasswordEmailAsync(User user, string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress) || !(_MailSettings.IsValidEmailRecipient(emailAddress)))
                return;

            MailMessage mailMessage = new MailMessage();
            string body = GetResetPasswordEmailTemplate();
            body = body.Replace("%USERNAME%", user.Username);
            body = body.Replace("%RESETPASSWORDLINK%", _HttpContextAccessor.HttpContext.Request.Scheme + "://" + _HttpContextAccessor.HttpContext.Request.Host + "/#/extra-layout/authtoken/" + user.Token);
            mailMessage.From = new MailAddress("donotreply@fbolinx.com");
            mailMessage.To.Add(new MailAddress(emailAddress));
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Reset Your Password";

            //Convert to a SendGrid message and use their API to send it
            Services.MailService mailService = new MailService(_MailSettings);
            var sendGridMessage = mailMessage.GetSendGridMessage();
            await mailService.SendAsync(sendGridMessage);
        }
        #endregion

        #region Private Methods
        private string GetResetPasswordEmailTemplate()
        {
            return FileLocater.GetTemplatesFileContent(_FileProvider, "ResetPassword",
                "ResetPasswordBody.html");
        }
        #endregion
    }
}
