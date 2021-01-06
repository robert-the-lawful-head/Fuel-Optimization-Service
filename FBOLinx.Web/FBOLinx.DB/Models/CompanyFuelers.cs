using System;

namespace FBOLinx.DB.Models
{
    public partial class CompanyFuelers
    {
        public int Oid { get; set; }
        public int FuelerId { get; set; }
        public int CompanyId { get; set; }
        public bool? Active { get; set; }
        public DateTime? AddDate { get; set; }
    }
}
