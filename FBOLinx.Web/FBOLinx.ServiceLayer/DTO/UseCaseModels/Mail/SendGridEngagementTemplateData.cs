using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail
{
    public class SendGridEngagementTemplateData
    {
        public string customerName { get; set; } = null;
        public string fboName { get; set; }
        public string ICAO { get; set; } = null;
        public string subject { get; set; }
    }
}
