using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class AllCurrentFboPricesSpecification : Specification<Models.Fboprices>
    {
        public AllCurrentFboPricesSpecification() : base(x => x.EffectiveFrom <= DateTime.UtcNow && x.EffectiveTo > DateTime.UtcNow && x.Expired != true)
        {

        }
    }
}
