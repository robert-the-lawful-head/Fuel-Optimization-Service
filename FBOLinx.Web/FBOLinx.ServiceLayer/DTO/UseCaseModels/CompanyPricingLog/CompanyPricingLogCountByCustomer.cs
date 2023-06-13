using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.CompanyPricingLog
{
    public class CompanyPricingLogCountByCustomer
    {
        public int CustomerId { get; set; }
        public int QuoteCount { get; set; }
        public DateTime? LastQuoteDate { get; set; }
    }
}
