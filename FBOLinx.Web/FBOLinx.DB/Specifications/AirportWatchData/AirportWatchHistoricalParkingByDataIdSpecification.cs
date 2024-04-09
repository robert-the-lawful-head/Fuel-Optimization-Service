using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public class AirportWatchHistoricalParkingByDataIdSpecification : Specification<Models.AirportWatchHistoricalParking>
    {
        public AirportWatchHistoricalParkingByDataIdSpecification(int airportWatchHistoricalDataId) : base(x => x.AirportWatchHistoricalDataId == airportWatchHistoricalDataId)
        {

        }
    }
}
