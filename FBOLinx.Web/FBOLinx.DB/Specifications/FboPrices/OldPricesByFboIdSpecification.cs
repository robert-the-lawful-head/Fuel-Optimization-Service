using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class OldPricesByFboIdSpecification : Specification<Models.Fboprices>
    {
        public OldPricesByFboIdSpecification(int fboId) : base(f => f.EffectiveTo <= DateTime.UtcNow && f.Fboid == fboId && (f.Expired == null || f.Expired != true))
        {

        }
    }
}
