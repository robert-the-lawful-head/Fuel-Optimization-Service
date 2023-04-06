using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.CompanyPricingLog
{
    public class CompanyPricingLogCountByDateRange
    {
        public int GroupId { get; set; }
        public int FboId { get; set; }
        public int QuoteCount { get; set; }
    }
}