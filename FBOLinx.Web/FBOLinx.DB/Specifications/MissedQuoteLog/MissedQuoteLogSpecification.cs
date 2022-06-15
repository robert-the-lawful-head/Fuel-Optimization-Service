using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications
{
    public class MissedQuoteLogSpecification : Specification<Models.MissedQuoteLog>
    {
        public MissedQuoteLogSpecification(int fboId, DateTime createdDate) : base(x => x.FboId == fboId && x.CreatedDate >= createdDate)
        {
        }
    }
}
