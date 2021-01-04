using System;

namespace FBOLinx.DB.Models
{
    public partial class ContractFuelRelationships
    {
        public int Oid { get; set; }
        public int Fboid { get; set; }
        public int? FuelVendorId { get; set; }
        public int? MarginType { get; set; }
        public double? Margin { get; set; }
        public DateTime? DateAdded { get; set; }
        public int? TemplateId { get; set; }
        public bool AttentionNeeded { get; set; }
    }
}
