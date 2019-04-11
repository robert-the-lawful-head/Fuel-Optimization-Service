using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FBOLinx.Web.Configurations;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FBOLinx.Web.Services
{
    public class MailService
    {
        private readonly FBOLinx.Web.Configurations.MailSettings _MailSettings;
        private Guid _SendToken; 

        public MailService(FBOLinx.Web.Configurations.MailSettings mailSettings)
        {
            _MailSettings = mailSettings;
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

        public async Task<Response> SendAsync(SendGridMessage msg)
        {
            var apiKey = _MailSettings.SendGridAPIKey;
            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(msg);
            return response;
        }

        #region Private Methods
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
