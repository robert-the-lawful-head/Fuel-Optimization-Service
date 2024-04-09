using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Group
{
    public class IntegrationUpdatePricingLogSpecification : Specification<Models.IntegrationUpdatePricingLog>
    {
        public IntegrationUpdatePricingLogSpecification(int fboId) : base(x => x.FboId == fboId)
        {
        }
    }
}
