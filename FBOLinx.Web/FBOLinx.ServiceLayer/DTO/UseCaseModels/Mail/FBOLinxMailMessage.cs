using System;
using System.Collections.Generic;
using System.Net.Mail;
using FBOLinx.Core.Utilities.Extensions;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail
{
    public class FBOLinxMailMessage : System.Net.Mail.MailMessage
    {
        #region Members
        #endregion

        #region Properties
        public string AttachmentBase64String { get; set; }
        public LogoDetails Logo { get; set; }
        public SendGridDistributionTemplateData SendGridDistributionTemplateData { get; set; }
        public SendGridEngagementTemplateData SendGridEngagementTemplate { get; set; }
        public SendGridGroupCustomerPricingTemplateData SendGridGroupCustomerPricingTemplateData { get; set; }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods
        private string[] GetSeparatedEmails(string email)
        {
            email = email.Replace(" .", ".").Replace(". ", ".")
                .Replace(" @", "@").Replace("@ ", "@")
                .Replace(" ", ";").Replace(",", ";");
            return email.Split(';');
        }
        #endregion
    }
}
