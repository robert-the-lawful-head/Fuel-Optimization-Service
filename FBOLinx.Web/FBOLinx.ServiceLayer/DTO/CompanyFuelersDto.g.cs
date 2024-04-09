using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CompanyFuelersDto
    {
        public int Oid { get; set; }
        public int FuelerId { get; set; }
        public int CompanyId { get; set; }
        public bool? Active { get; set; }
        public DateTime? AddDate { get; set; }
    }
}