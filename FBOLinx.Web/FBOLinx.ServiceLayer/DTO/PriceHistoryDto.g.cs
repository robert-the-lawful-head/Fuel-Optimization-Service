using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class PriceHistoryDto
    {
        public int? Fboid { get; set; }
        public string Fbo { get; set; }
        public DateTime? ValidUntil { get; set; }
        public int? CustomerId { get; set; }
        public string Company { get; set; }
        public short? MarginType { get; set; }
        public double? Margin { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public string Amount { get; set; }
        public string Icao { get; set; }
        public double? PostedRetail { get; set; }
        public double? TotalCost { get; set; }
        public double? _100llmargin { get; set; }
        public short? _100llmarginType { get; set; }
        public int? TemplateId { get; set; }
        public double? PostedRetail100Ll { get; set; }
        public double? TotalCost100Ll { get; set; }
        public int Oid { get; set; }
    }
}