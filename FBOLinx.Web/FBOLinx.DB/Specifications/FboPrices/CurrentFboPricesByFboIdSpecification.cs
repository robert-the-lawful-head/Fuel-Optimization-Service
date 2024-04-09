using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class CurrentFboPricesByFboIdSpecification : Specification<Models.Fboprices>
    {
        public CurrentFboPricesByFboIdSpecification(int fboId) : base(x => x.Fboid == fboId && x.EffectiveFrom <= DateTime.UtcNow && x.EffectiveTo > DateTime.UtcNow && x.Expired != true)
        {

        }
    }
}
