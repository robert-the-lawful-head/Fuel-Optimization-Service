using FBOLinx.Core.BaseModels.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.CustomerInfoByFbo
{
    public class CustomerInfoByFboByCustomerInfoByGroupIdFboIdSpecification : Specification<Models.CustomerInfoByFbo>
    {
        public CustomerInfoByFboByCustomerInfoByGroupIdFboIdSpecification(int customerInfoByGroupId, int fboId) : base(x => x.CustomerInfoByGroupId == customerInfoByGroupId && x.FboId == fboId)
        {
        }
    }
}
