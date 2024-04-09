using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerMarginsGridViewModel
    {
        public int PriceTierId { get; set; }
        public int TemplateId { get; set; }
        public double? Amount { get; set; }
        public int Oid { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }

        public int? discountType { get; set; }
        public double? MaxEntered { get; set; }
    }
}
