using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class FbocustomerPricing
    {
        public int CustomerId { get; set; }
        public int Fboid { get; set; }
        public DateTime? LastUpdated { get; set; }
        public double? PriceMargin { get; set; }
        public int Oid { get; set; }
    }
}
