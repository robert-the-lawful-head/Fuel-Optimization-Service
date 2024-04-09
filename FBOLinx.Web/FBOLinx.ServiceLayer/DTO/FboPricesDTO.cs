using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class FboPricesDTO : FBOLinxBaseEntityModel<int>
    {
        public int? Fboid { get; set; }
        public string Product { get; set; }
        public double? Price { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? Timestamp { get; set; }
        public double? SalesTax { get; set; }
        public string Currency { get; set; }
        public bool? Expired { get; set; }
        public int? Id { get; set; }
        [NotMapped]
        public string GenericProduct
        {
            get
            {
                if (Product.Contains("Cost"))
                    return "Cost";
                if (Product.Contains("Retail"))
                    return "Retail";
                return "";
            }
        }
        public FboPricesSource Source { get; set; } = FboPricesSource.FboLinx;
        public int? IntegrationPartnerId { get; set; }
    }
}
