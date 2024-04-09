using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class NonExpiredPricesByFboIdSpecification : Specification<Models.Fboprices>
    {
        public NonExpiredPricesByFboIdSpecification(int fboId) : base(x => x.Fboid == fboId && x.Expired == null)
        {

        }
    }
}
