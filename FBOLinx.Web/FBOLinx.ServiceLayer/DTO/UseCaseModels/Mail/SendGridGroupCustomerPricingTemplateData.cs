using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail
{
    public class SendGridGroupCustomerPricingTemplateData
    {
        [JsonProperty("templateEmailBodyMessage")]
        public string templateEmailBodyMessage { get; set; }

        [JsonProperty("Subject")]
        public string Subject { get; set; }
    }
}
