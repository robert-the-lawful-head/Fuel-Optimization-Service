using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CompanyPricingLog
{
    public class CompanyPricingLogSpecification : Specification<Models.CompanyPricingLog>
    {
        public CompanyPricingLogSpecification(int fuelerlinxCompanyId, DateTime startDate, DateTime endDate) : base(x => x.CompanyId == fuelerlinxCompanyId && x.CreatedDate >= startDate && x.CreatedDate <= endDate)
        {
        }
    }
}
