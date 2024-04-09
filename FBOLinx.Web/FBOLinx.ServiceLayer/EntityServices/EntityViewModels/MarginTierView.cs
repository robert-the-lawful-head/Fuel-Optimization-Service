using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.EntityServices.EntityViewModels
{
    public  class MarginTierView
    {
        public int TemplateId { get; set; }
        public double MaxPriceTierValue { get; set; }
        public double MinPriceTierValue { get; set; }
    }
}
