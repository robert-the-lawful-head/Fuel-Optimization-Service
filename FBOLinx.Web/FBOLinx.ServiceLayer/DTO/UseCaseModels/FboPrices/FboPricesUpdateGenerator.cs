using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.FboPrices
{
    public class FboPricesUpdateGenerator
    {
        public int OidCost { get; set; }
        public int OidPap { get; set; }
        public int Fboid { get; set; }
        public string Product { get; set; }
        public double? PriceCost { get; set; }
        public double? PricePap { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveFromUtc { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int Source { get; set; }
        public bool IsStaged { get; set; } = false;
        public bool IsLive { get; set; } = false;
    }
}
