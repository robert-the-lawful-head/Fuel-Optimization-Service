using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications
{
    public class IntegrationStatusSpecification : Specification<Models.IntegrationStatus>
    {
        public IntegrationStatusSpecification(int integrationPartnerId, int fboId) : base(x => x.IntegrationPartnerId == integrationPartnerId && x.FboId == fboId)
        {
        }
    }
}
