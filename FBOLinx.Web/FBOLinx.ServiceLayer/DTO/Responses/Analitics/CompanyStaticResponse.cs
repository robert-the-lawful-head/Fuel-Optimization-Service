namespace FBOLinx.ServiceLayer.DTO.Responses.Analitics
{
    public class CompanyStaticResponse
    {
        public int Oid { get; set; }
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public int CompanyQuotesTotal { get; set; }
        public int DirectOrders { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal ConversionRateTotal { get; set; }
        public int TotalOrders { get; set; }
        public int AirportOrders { get; set; }
        public decimal CustomerBusiness { get; set; }
        public string LastPullDate { get; set; }
        public string AirportICAO { get; set; }
        public int AirportVisits { get; set; }
        public int VisitsToFbo { get; set; }
        public double PercentVisits { get; set; }

    }
}
