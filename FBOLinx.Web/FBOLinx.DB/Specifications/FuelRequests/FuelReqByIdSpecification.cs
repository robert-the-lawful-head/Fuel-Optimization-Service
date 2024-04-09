using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.FuelRequests
{
    public sealed class FuelReqByIdSpecification : Specification<DB.Models.FuelReq>
    {
        public FuelReqByIdSpecification(int oid) : base(x => x.Oid == oid)
        {
        }
    }
}
