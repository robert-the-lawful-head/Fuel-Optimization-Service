using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.DTO
{
    public class VolumeScaleDiscountFboPrice
    {
        public int CustomerId { get; set; }
        public int Fboid { get; set; }
        public bool DefaultSettings { get; set; }
        public short MarginType { get; set; }
        public double Price { get; set; }
        public double Margin { get; set; }
        public double SalesTax { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
