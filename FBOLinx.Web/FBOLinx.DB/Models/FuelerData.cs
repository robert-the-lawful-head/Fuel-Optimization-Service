using System;

namespace FBOLinx.DB.Models
{
    public partial class FuelerData
    {
        public int Oid { get; set; }
        public int? CustId { get; set; }
        public int? FuelerId { get; set; }
        public bool Active { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? OffDate { get; set; }
        public DateTime? ChgDate { get; set; }
        public int? CompanyId { get; set; }
        public string QbvendorName { get; set; }
        public double? DiscrepancyMaxPriceDif { get; set; }
    }
}
