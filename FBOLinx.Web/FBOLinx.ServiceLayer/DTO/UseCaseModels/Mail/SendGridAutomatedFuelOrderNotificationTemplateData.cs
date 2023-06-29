using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail
{
    public class SendGridAutomatedFuelOrderNotificationTemplateData
    {
        [JsonProperty("aircraftTailNumber")]
        public string aircraftTailNumber { get; set; }

        [JsonProperty("fboName")]
        public string fboName { get; set; }

        [JsonProperty("flightDepartment")]
        public string flightDepartment { get; set; }

        [JsonProperty("aircraftMakeModel")]
        public string aircraftMakeModel { get; set; }

        [JsonProperty("airportICAO")]
        public string airportICAO { get; set; }

        [JsonProperty("arrivalDate")]
        public string arrivalDate { get; set; }

        [JsonProperty("departureDate")]
        public string departureDate { get; set; }

        [JsonProperty("fuelVolume")]
        public string fuelVolume { get; set; }

        [JsonProperty("fuelVendor")]
        public string fuelVendor { get; set; }

        [JsonProperty("customOrderNotes")]
        public string customOrderNotes { get; set; }
        [JsonProperty("buttonUrl")]
        public string buttonUrl { get; set; }
        [JsonProperty("paymentMethod")]
        public string paymentMethod { get; set; }
        [JsonProperty("services")]
        public List<string> services { get; set; }
    }
}
