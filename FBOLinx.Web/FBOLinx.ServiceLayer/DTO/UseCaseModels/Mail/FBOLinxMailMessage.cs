using System;
using System.Collections.Generic;
using System.Net.Mail;
using FBOLinx.Core.Utilities.Extensions;
using FBOLinx.Core.Utilities.Mail;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail
{
    public class FBOLinxMailMessage : System.Net.Mail.MailMessage
    {
        #region Members
        #endregion

        #region Properties
        public string InlineAttachmentBase64String { get; set; }
        public List<FileAttachment> AttachmentsCollection { get; set; } = new List<FileAttachment>();
        public LogoDetails Logo { get; set; }
        public SendGridDistributionTemplateData SendGridDistributionTemplateData { get; set; }
        public SendGridEngagementTemplateData SendGridEngagementTemplateData { get; set; }
        public SendGridMissedQuoteTemplateData SendGridMissedQuoteTemplateData { get; set; }
        public SendGridGroupCustomerPricingTemplateData SendGridGroupCustomerPricingTemplateData { get; set; }
        public SendGridAutomatedFuelOrderNotificationTemplateData SendGridAutomatedFuelOrderNotificationTemplateData { get; set; }

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
