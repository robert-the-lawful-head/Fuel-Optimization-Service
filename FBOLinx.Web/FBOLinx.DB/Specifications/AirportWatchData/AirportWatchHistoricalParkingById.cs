using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public class AirportWatchHistoricalParkingById : Specification<AirportWatchHistoricalParking>
    {
        public AirportWatchHistoricalParkingById(int id) : base(x => x.Oid == id)
        {
        }
    }
}
