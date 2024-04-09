using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations
{
    public class MailSettings
    {
        public string MailUserName { get; set; }
        public string MailPassword { get; set; }
        public int MailPort { get; set; }
        public string MailHost { get; set; }
        public bool MailEnableSSL { get; set; }
        public bool MailNetworkDelivery { get; set; }
        public string SendGridAPIKey { get; set; }
        public List<string> LimitedEmailDomains { get; set; }

        public bool IsValidEmailRecipient(string emailAddress)
        {
            var emailAddressValidator = new EmailAddressAttribute();
            if (string.IsNullOrEmpty(emailAddress))
                return false;
            if (!emailAddressValidator.IsValid(emailAddress))
                return false;
            if (LimitedEmailDomains == null || LimitedEmailDomains.Count == 0)
                return true;
            foreach (string emailDomain in LimitedEmailDomains)
            {
                if (emailAddress.ToLower().Contains(emailDomain))
                    return true;
            }
            return false;
        }
    }
}
