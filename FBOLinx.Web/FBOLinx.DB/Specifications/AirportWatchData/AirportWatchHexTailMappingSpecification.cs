using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;
using FBOLinx.DB.Projections.AirportWatch;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public class AirportWatchHexTailMappingSpecification : Specification<AirportWatchLiveData, AirportWatchHexTailMapping>
    {
        public AirportWatchHexTailMappingSpecification(Expression<Func<AirportWatchLiveData, AirportWatchHexTailMapping>> projection, Expression<Func<AirportWatchLiveData, bool>> criteria) : base(projection, criteria)
        {
        }
    }
}
