using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class FboPricesByFboIdSpecification : Specification<Models.Fboprices>
    {
        public FboPricesByFboIdSpecification(int fboId) : base(x => x.Fboid == fboId)
        {

        }
    }
}
