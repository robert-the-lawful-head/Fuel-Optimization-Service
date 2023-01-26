using FBOLinx.Core.Utilities.Extensions;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Mail
{
    public interface IMailService
    {
        Task<bool> SendAsync(FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail.FBOLinxMailMessage msg);
        bool IsValidEmailRecipient(string emailAddress);
    }

    public class MailService : IMailService
    {
        private readonly FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations.MailSettings _MailSettings;

        public MailService(IOptions<FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations.MailSettings> mailSettings)
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

            if (msg.SendGridEngagementTemplateData != null)
            {
                AddEngagementEmailData(msg, ref sendGridMessage);
            }

            if (msg.SendGridGroupCustomerPricingTemplateData != null)
            {
                AddGroupCustomerPricingEmailData(msg, ref sendGridMessage);
            }

            if (msg.SendGridMissedQuoteTemplateData != null)
            {
                AddMissedQuoteEmailData(msg, ref sendGridMessage);
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

            foreach (var pricesForProduct in message.SendGridDistributionTemplateData.pricesForProducts)
            {
                var pricesAttachment = new SendGrid.Helpers.Mail.Attachment();
                pricesAttachment.Disposition = "inline";
                pricesAttachment.Content = pricesForProduct.imageBase64;
                pricesAttachment.Filename = pricesForProduct.cId.Replace("cid:","") + ".png";
                pricesAttachment.Type = "image/png";
                pricesAttachment.ContentId = pricesForProduct.cId.Replace("cid:", "");
                sendGridMessageWithTemplate.AddAttachment(pricesAttachment);
            }

            foreach (var attachment in message.AttachmentsCollection)
            {
                var sendGridAttachment = new SendGrid.Helpers.Mail.Attachment();
                sendGridAttachment.Filename = attachment.FileName;
                sendGridAttachment.Type = attachment.ContentType;
                sendGridAttachment.Content = attachment.FileData;
                sendGridAttachment.Disposition = "attachment";
                sendGridMessageWithTemplate.AddAttachment(sendGridAttachment);
            }

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

            sendGridMessageWithTemplate.TemplateId = "d-53e251adc804497d8b2239f75de64aa4";
        }

        private void AddGroupCustomerPricingEmailData(FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail.FBOLinxMailMessage message, ref SendGridMessage sendGridMessageWithTemplate)
        {
            sendGridMessageWithTemplate.SetTemplateData(message.SendGridGroupCustomerPricingTemplateData);

            var pricesAttachment = new SendGrid.Helpers.Mail.Attachment();
            pricesAttachment.Disposition = "attachment";
            pricesAttachment.Content = message.AttachmentsCollection[0].FileData;
            pricesAttachment.Filename = "Prices.csv";
            pricesAttachment.Type = "text/csv";
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
                sendGridMessageWithTemplate.TemplateId = "d-ed86e8cb93a143a8861cd11bbcca2525";
            }
            else
                sendGridMessageWithTemplate.TemplateId = "d-5b2281c7d8df4c62a5219b7528151d2c";
        }

        private void AddEngagementEmailData(FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail.FBOLinxMailMessage message, ref SendGridMessage sendGridMessageWithTemplate)
        {
            sendGridMessageWithTemplate.SetTemplateData(message.SendGridEngagementTemplateData);

            if (string.IsNullOrEmpty(message.SendGridEngagementTemplateData.customerName))
                sendGridMessageWithTemplate.TemplateId = "d-bd3e32cbb21a4c60bf9753bcf70b2527";  //templateid for fuel price expiration
            else
                sendGridMessageWithTemplate.TemplateId = "d-038c5d66d8034610af790492a8e184b8";  //templateid for no ramp fees
        }

        private void AddMissedQuoteEmailData(FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail.FBOLinxMailMessage message, ref SendGridMessage sendGridMessageWithTemplate)
        {
            sendGridMessageWithTemplate.SetTemplateData(message.SendGridMissedQuoteTemplateData);
            sendGridMessageWithTemplate.TemplateId = "d-d367c9ed538e4a52aaf34ac042aaa3fd";
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
