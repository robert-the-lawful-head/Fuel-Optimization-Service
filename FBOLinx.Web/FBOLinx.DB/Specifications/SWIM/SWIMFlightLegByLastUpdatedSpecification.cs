using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.DB.Specifications.SWIM
{
    public class SWIMFlightLegByLastUpdatedSpecification : Specification<SWIMFlightLeg>
    {
        public SWIMFlightLegByLastUpdatedSpecification(DateTime minimumLastUpdatedDateTime, long minimumOid) : base(x => x.LastUpdated >= minimumLastUpdatedDateTime && x.Oid >= minimumOid)
        {
        }
    }
}
