using System;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CompanyPricingLogDto
    {
        public int Oid { get; set; }
        public int CompanyId { get; set; }
        public string ICAO { get; set; }
        public DateTime CreatedDate { get; set; }
        public CustomersDto Customer { get; set; }
    }
}