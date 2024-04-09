using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail
{
    public class SendGridFuelRequestUpdateOrCancellationTemplateData
    {
        [JsonProperty("buttonUrl")]
        public string buttonUrl { get; set; }
        [JsonProperty("requestStatus")]
        public string requestStatus { get; set; }
    }
}
