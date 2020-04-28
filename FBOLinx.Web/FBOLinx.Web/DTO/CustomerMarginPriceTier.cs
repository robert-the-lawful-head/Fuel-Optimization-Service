using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.DTO
{
    public class CustomerMarginPriceTier
    {
        public double Min { get; set; }
        public int TemplateId { get; set; }
        public double? Amount { get; set; }
    }
}
