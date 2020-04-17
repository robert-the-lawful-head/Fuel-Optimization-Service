using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class FBOGroupPriceUpdateVM
    {
        public int FboId { get; set; }
        public string FboName { get; set; }
        public decimal? Retail { get; set; }
        public decimal? Cost { get; set; }
        public DateTime EffectiveTo { get; set; }
    }
}
