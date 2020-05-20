using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class FbosalesTax
    {
        public int Fboid { get; set; }
        public double SalesTaxDecimal { get; set; }
        public double? SalesTax100Lldecimal { get; set; }
        public int Oid { get; set; }
    }
}
