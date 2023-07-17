using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.AcukwikAirport
{
    public class AcukwikAirportByOidSpecification : Specification<Models.AcukwikAirport>
    {
        public AcukwikAirportByOidSpecification(int oid) : base(x => x.Oid == oid)
        {
        }
    }
}