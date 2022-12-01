using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class OldFboPricesByProductSpecification : Specification<Models.Fboprices>
    {
        public OldFboPricesByProductSpecification(int fboId, string product) : base(x => x.Fboid == fboId && x.EffectiveFrom <= DateTime.UtcNow && x.Expired == null && x.Product == product)
        {

        }
    }
}
