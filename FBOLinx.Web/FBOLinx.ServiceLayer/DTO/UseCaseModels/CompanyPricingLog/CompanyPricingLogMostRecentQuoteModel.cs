using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.CompanyPricingLog
{
    public class CompanyPricingLogMostRecentQuoteModel
    {
        public string Icao { get; set; }
        public int FuelerLinxCompanyId { get; set; }
        public DateTime? MostRecentQuoteDateTime { get; set; }
    }
}
