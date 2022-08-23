using System;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class MissedQuoteLogDTO : FBOLinxBaseEntityModel<int>
    {
        public int? FboId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CustomerId { get; set; }
        public bool? Emailed { get; set; }
        public string CreatedDateString { get; set; }
        public string Debugs { get; set; }
    }
}