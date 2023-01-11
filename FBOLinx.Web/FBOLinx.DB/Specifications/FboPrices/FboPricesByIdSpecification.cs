using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class FboPricesByIdSpecification : Specification<Models.Fboprices>
    {
        public FboPricesByIdSpecification(int id) : base(x => x.Oid == id)
        {
           
        }
    }
}
