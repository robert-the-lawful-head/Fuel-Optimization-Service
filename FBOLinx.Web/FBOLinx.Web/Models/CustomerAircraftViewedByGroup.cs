using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class CustomerAircraftViewedByGroup
    {
        public int? CustomerAircraftId { get; set; }
        public int? GroupId { get; set; }
        public int Oid { get; set; }
    }
}
