using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class FuelerList
    {
        public int Oid { get; set; }
        public string FuelerNm { get; set; }
        public string FuelerType { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? OffDate { get; set; }
        public DateTime? ChgDate { get; set; }
        public string EMail { get; set; }
        public string ProcessNm { get; set; }
        public bool Inclusive { get; set; }
        public string WebLink { get; set; }
        public string FuelerPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ImagePath { get; set; }
        public bool PullFlag { get; set; }
        public string Ccemail { get; set; }
        public bool? International { get; set; }
        public bool? Enabled { get; set; }
        public int? FbolinxId { get; set; }
        public bool? Show { get; set; }
        public bool? IsWebBasedAutoRecon { get; set; }
        public bool? IsPricingAlwaysShown { get; set; }
        public string InternationalEmail { get; set; }
        public bool? UseOldService { get; set; }
        public string ServiceUrl { get; set; }
        public bool? IsMessagesEnabled { get; set; }
        public bool? IsVendorLinxEnabled { get; set; }
    }
}
