using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Responses.FuelPricing
{
    public class FuelPriceResponse
    {
        public string Icao { get; set; }
        public string Iata { get; set; }
        public int FboId { get; set; }
        public string Fbo { get; set; }
        public int GroupId { get; set; }
        public string Group { get; set; }
        public string Product { get; set; }
        public double MinVolume { get; set; }
        public string Notes { get; set; }
        public bool Default { get; set; }
        public double Price { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string TailNumberList { get; set; }
        public int CustomerId { get; set; }
        public int PricingTemplateId { get; set; }
        public string PricingTemplateName { get; set; }
        public string FuelDeskEmail { get; set; }
        public string CopyEmails { get; set; }
        public double BasePrice { get; set; }
    }
}
