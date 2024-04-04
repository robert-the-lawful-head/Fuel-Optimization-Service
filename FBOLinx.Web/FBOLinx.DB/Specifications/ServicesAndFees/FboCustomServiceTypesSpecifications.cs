using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models.ServicesAndFees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.ServicesAndFees
{
    public class FboCustomServiceTypesSpecifications : Specification<FboCustomServiceType>
    {
        public FboCustomServiceTypesSpecifications(int fboId) : base(x => x.FboId == fboId)
        {
        }
    }
}
