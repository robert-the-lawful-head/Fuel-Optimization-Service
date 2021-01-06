using System;

namespace FBOLinx.DB.Models
{
    public partial class Jobs
    {
        public int Oid { get; set; }
        public int? GroupId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
