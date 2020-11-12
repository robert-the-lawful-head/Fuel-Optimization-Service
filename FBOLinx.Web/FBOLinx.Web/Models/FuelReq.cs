using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    public partial class FuelReq
    {
        public int Oid { get; set; }
        public int? CustomerId { get; set; }
        public string Icao { get; set; }
        public int? Fboid { get; set; }
        public int? CustomerAircraftId { get; set; }
        public DateTime? Eta { get; set; }
        public DateTime? Etd { get; set; }
        public string TimeStandard { get; set; }
        public bool? Cancelled { get; set; }
        public double? QuotedVolume { get; set; }
        public double? QuotedPpg { get; set; }
        public string Notes { get; set; }
        public DateTime? DateCreated { get; set; }
        public double? ActualVolume { get; set; }
        public double? ActualPpg { get; set; }
        public string Source { get; set; }
        public int? SourceId { get; set; }
        public string DispatchNotes { get; set; }
        public bool? Archived { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("FuelReqs")]
        public Customers Customer { get; set; }

        [ForeignKey("CustomerAircraftId")]
        [InverseProperty("FuelReqs")]
        public CustomerAircrafts CustomerAircraft { get; set; }

        [ForeignKey("Fboid")]
        [InverseProperty("FuelReqs")]
        public Fbos Fbo { get; set; }

        [InverseProperty("FuelReq")]
        public FuelReqPricingTemplate FuelReqPricingTemplate { get; set; }
    }
}
