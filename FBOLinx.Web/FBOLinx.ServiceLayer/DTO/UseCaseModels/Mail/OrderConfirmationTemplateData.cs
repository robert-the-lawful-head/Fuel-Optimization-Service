using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail
{
    public class OrderConfirmationTemplateData
    {
        [JsonProperty("aircraftTailNumber")]
        public string aircraftTailNumber { get; set; }

        [JsonProperty("fboName")]
        public string fboName { get; set; }

        [JsonProperty("airportICAO")]
        public string airportICAO { get; set; }

        [JsonProperty("arrivalDate")]
        public string arrivalDate { get; set; }

        [JsonProperty("fuelVendor")]
        public string fuelVendor { get; set; }
        [JsonProperty("fuelerLinxId")]
        public string fuelerLinxId { get; set; }
    }
}
