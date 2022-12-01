using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.Fbo
{
    public sealed class NonExpiredPricesByFboIdGenericProductSpecification : Specification<Models.Fboprices>
    {
        public NonExpiredPricesByFboIdGenericProductSpecification(int fboId, string genericProduct) : base(x => x.Fboid == fboId && x.Expired == null && (x.Product == genericProduct + " Retail" || x.Product == genericProduct + " Cost"))
        {

        }
    }
}
