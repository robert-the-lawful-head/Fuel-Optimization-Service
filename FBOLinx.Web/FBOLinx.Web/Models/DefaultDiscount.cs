using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class DefaultDiscount
    {
        public int Oid { get; set; }
        public double? Margin { get; set; }
        public short? MarginType { get; set; }
        public int Fboid { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string Notes { get; set; }
    }
}
