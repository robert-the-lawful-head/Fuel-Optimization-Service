using System;
using FBOLinx.Core.Enums;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class RampFeesDto
    {
        public int Oid { get; set; }
        public double? Price { get; set; }
        public AircraftSizes? Size { get; set; }
        public double? Waived { get; set; }
        public int? Fboid { get; set; }
        public RampFeeCategories? CategoryType { get; set; }
        public int? CategoryMinValue { get; set; }
        public int? CategoryMaxValue { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string CategoryStringValue { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string SizeDescription { get; set; }
        public string CategoryDescription { get; set; }
        public string CategorizationDescription { get; set; }
    }
}