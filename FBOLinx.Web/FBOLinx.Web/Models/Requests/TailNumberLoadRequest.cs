using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class TailNumberLoadRequest
    {
        [Required]
        public string TailNumber { get; set; }
        public int PricingTemplateID { get; set; }
        public string ICAO { get; set; }
        [Required]
        public int FBOID { get; set; }
        [Required]
        public int GroupID { get; set; }
        [Required]
        public Enums.FlightTypeClassifications FlightTypeClassification { get; set; }     
        [Required]
        public Enums.ApplicableTaxFlights DepartureType { get; set; }
    }
}
