using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.FboAirport
{
    public class FboAirportSpecification : Specification<Models.Fboairports>
    {
        public FboAirportSpecification(int fboId) : base(x => x.Fboid == fboId)
        {
        }
    }
}
