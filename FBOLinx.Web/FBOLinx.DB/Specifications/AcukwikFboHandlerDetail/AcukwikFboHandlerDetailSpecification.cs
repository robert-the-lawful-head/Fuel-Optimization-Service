using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.Aircraft
{
    public sealed class AcukwikFboHandlerDetailSpecification : Specification<AcukwikFbohandlerDetail>
    {
        public AcukwikFboHandlerDetailSpecification(int handlerId) : base(x => x.HandlerId == handlerId)
        {
        }
    }
}
