using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.CustomerAircrafts
{
    public sealed class FboByAcukwikHandlerIdSpecification : Specification<Models.Fbos>
    {
        public FboByAcukwikHandlerIdSpecification(int acukwikHandlerId) : base(x => x.AcukwikFBOHandlerId == acukwikHandlerId)
        {
            AddInclude(x => x.FboAirport);
            AddInclude(x => x.Preferences);
        }
    }
}
