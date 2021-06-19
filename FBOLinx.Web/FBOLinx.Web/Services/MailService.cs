using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FBOLinx.Core.Utilities.Extensions;
using FBOLinx.Web.Configurations;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FBOLinx.Web.Services
{
    public interface IMailService
    {
        Task<bool> SendAsync(FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail.FBOLinxMailMessage msg);
        bool IsValidEmailRecipient(string emailAddress);
    }

    public class MailService : IMailService
    {
        private readonly FBOLinx.Web.Configurations.MailSettings _MailSettings;

        public MailService(IOptions<FBOLinx.Web.Configurations.MailSettings> mailSettings)
        {
            _MailSettings = mailSettings.Value;
        }

        public async Task<bool> SendAsync(FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail.FBOLinxMailMessage msg)
        {
            var sendGridMessage = MailMessageExtensions.GetSendGridMessage(msg);

            if (msg.SendGridDistributionTemplateData != null)
            {
                AddDistributionEmailData(msg, ref sendGridMessage);
            }

            if (msg.SendGridEngagementTemplate != null)
            {
                AddEngagementEmailData(msg, ref sendGridMessage);
            }

            var apiKey = _MailSettings.SendGridAPIKey;
            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(sendGridMessage);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                return true;
            else
            {
                var mailError = await response.Body.ReadAsStringAsync();
                throw new System.Exception(mailError);
            }
        }

        public bool IsValidEmailRecipient(string emailAddress)
        {
            var emailAddressValidator = new EmailAddressAttribute();
            if (string.IsNullOrEmpty(emailAddress))
                return false;
            if (!emailAddressValidator.IsValid(emailAddress))
                return false;
            if (_MailSettings.LimitedEmailDomains == null || _MailSettings.LimitedEmailDomains.Count == 0)
                return true;
            foreach (string emailDomain in _MailSettings.LimitedEmailDomains)
            {
                if (emailAddress.ToLower().Contains(emailDomain))
                    return true;
            }
            return false;
        }

        //public void Send(MailMessage mailMessage)
        //{
        //    try
        //    {
        //        _SendToken = Guid.NewGuid();
        //        SmtpClient client = GenerateSMTP();
        //        client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        //        client.SendAsync(mailMessage, _SendToken);
        //    }
        //    catch (System.Exception exception)
        //    {

        //    }
        //}

        public static string GetFboLinxAddress(string address)
        {
            return address + "@fbolinx.com";
        }

        #region Private Methods
        private void AddDistributionEmailData(FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail.FBOLinxMailMessage message, ref SendGridMessage sendGridMessageWithTemplate)
        {
            sendGridMessageWithTemplate.SetTemplateData(message.SendGridDistributionTemplateData);

            var pricesAttachment = new SendGrid.Helpers.Mail.Attachment();
            pricesAttachment.Disposition = "inline";
            pricesAttachment.Content = message.AttachmentBase64String;
            pricesAttachment.Filename = "prices.png";
            pricesAttachment.Type = "image/png";
            pricesAttachment.ContentId = "Prices";
            sendGridMessageWithTemplate.AddAttachment(pricesAttachment);

            if (message.Logo != null)
            {
                var logoAttachment = new SendGrid.Helpers.Mail.Attachment();
                logoAttachment.Disposition = "inline";
                logoAttachment.Content = message.Logo.Base64String;
                logoAttachment.Filename = message.Logo.Filename;
                logoAttachment.Type = message.Logo.ContentType;
                logoAttachment.ContentId = "Logo";
                sendGridMessageWithTemplate.AddAttachment(logoAttachment);
            }

            sendGridMessageWithTemplate.TemplateId = "d-537f958228a6490b977e372ad8389b71";
        }

        private void AddEngagementEmailData(FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail.FBOLinxMailMessage message, ref SendGridMessage sendGridMessageWithTemplate)
        {
            sendGridMessageWithTemplate.SetTemplateData(message.SendGridEngagementTemplate);

            if (!string.IsNullOrEmpty(message.SendGridEngagementTemplate.customerName))
                sendGridMessageWithTemplate.TemplateId = "d-bd3e32cbb21a4c60bf9753bcf70b2527";  //templateid for fuel price expiration
            else
                sendGridMessageWithTemplate.TemplateId = "d-038c5d66d8034610af790492a8e184b8";  //templateid for no ramp fees
        }

        private SmtpClient GenerateSMTP()
        {
            // Creates a new SMTP client to send the above message
            SmtpClient client = new SmtpClient();
            if (_MailSettings.MailNetworkDelivery)
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
            }
            // Sets the login credentials for your email account
            client.Credentials = new System.Net.NetworkCredential(_MailSettings.MailUserName, _MailSettings.MailPassword);
            // Sets the port number for the fuelerlinx server
            client.Port = _MailSettings.MailPort;
            // Sets the smtp server to fuelerlinx
            client.Host = _MailSettings.MailHost;
            // Enables SSL
            client.EnableSsl = _MailSettings.MailEnableSSL;
            // This will send the message based on all the previous details
            return client;
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            string token = (string)e.UserState;
        }
        #endregion
    }
}
