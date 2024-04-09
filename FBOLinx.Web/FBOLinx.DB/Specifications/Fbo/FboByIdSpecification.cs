using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class FboByIdSpecification : Specification<Models.Fbos>
    {
        public FboByIdSpecification(int id) : base(x => x.Oid == id)
        {
            AddInclude(x => x.FboAirport);
            AddInclude(x => x.Preferences);
        }
    }
}
