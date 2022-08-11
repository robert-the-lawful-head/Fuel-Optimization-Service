using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.ServiceLayer.DTO.Requests.FuelPricing
{
    public class PriceLookupRequest
    {
        public string TailNumber { get; set; }
        public int PricingTemplateID { get; set; }
        public string ICAO { get; set; }
        [Required]
        public int FBOID { get; set; }
        [Required]
        public int GroupID { get; set; }
        [Required]
        public FlightTypeClassifications FlightTypeClassification { get; set; }     
        [Required]
        public ApplicableTaxFlights DepartureType { get; set; }
        public List<FboFeesAndTaxes> ReplacementFeesAndTaxes { get; set; }
        public int CustomerInfoByGroupId { get; set; }
    }
}
