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
        [JsonProperty("fboName")]
        public string fboName { get; set; }

        [JsonProperty("flightDepartment")]
        public string flightDepartment { get; set; }

        [JsonProperty("tailNumber")]
        public string tailNumber { get; set; }

        [JsonProperty("airportICAO")]
        public string airportICAO { get; set; }

        [JsonProperty("arrivalDate")]
        public string arrivalDate { get; set; }

        [JsonProperty("departureDate")]
        public string departureDate { get; set; }

        [JsonProperty("services")]
        public List<ServicesForSendGrid> services { get; set; }

        [JsonProperty("flightDepartmentInfo")]
        public List<FlightDepartmentInfoForSendGrid> flightDepartmentInfo { get; set; }
    }

    public partial class ServicesForSendGrid
    {
        public string service { get; set; }
    }

    public partial class FlightDepartmentInfoForSendGrid
    {
        public string info { get; set; }
    }
}
