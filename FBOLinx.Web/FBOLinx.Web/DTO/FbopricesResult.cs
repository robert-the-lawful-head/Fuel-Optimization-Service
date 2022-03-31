using System;

namespace FBOLinx.Web.DTO
{
    public class FbopricesResult
    {
        public int Oid { get; set; }
        public int Fboid { get; set; }
        public string Product { get; set; }
        public double? Price { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? TimeStamp { get; set; }
        public double? SalesTax { get; set; }
        public string Currency { get; set; }
        public double? TempJet { get; set; }
        public double? TempAvg { get; set; }
        public int? TempId { get; set; }
        public DateTime? TempDateFrom { get; set; }
        public DateTime? TempDateTo { get; set; }
        public string GenericProduct
        {
            get
            {
                if (Product.Contains("Cost"))
                    return "Cost";
                if (Product.Contains("Retail"))
                    return "Retail";
                return "";
            }
        }
    }
}
