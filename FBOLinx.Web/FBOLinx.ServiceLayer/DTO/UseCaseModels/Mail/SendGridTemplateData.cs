using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail
{
    public class SendGridTemplateData
    {
        [JsonProperty("recipientCompanyName")]
        public string recipientCompanyName { get; set; }

        [JsonProperty("templateEmailBodyMessage")]
        public string templateEmailBodyMessage { get; set; }

        [JsonProperty("fboName")]
        public string fboName { get; set; }

        [JsonProperty("fboICAOCode")]
        public string fboICAOCode { get; set; }

        [JsonProperty("fboAddress")]
        public string fboAddress { get; set; }

        [JsonProperty("fboCity")]
        public string fboCity { get; set; }

        [JsonProperty("fboState")]
        public string fboState { get; set; }

        [JsonProperty("fboZip")]
        public string fboZip { get; set; }

        [JsonProperty("templateNotesMessage")]
        public string templateNotesMessage { get; set; }

        [JsonProperty("Subject")]
        public string Subject { get; set; }
        [JsonProperty("expiration")]
        public string expiration { get; set; }

        public string currentPostedRetail { get; set; }
    }
}
