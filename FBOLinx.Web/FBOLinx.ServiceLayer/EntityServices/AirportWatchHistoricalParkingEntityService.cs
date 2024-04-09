using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IAirportWatchHistoricalParkingEntityService : IRepository<AirportWatchHistoricalParking, FboLinxContext>
    {
        
    }

    public class AirportWatchHistoricalParkingEntityService : Repository<AirportWatchHistoricalParking, FboLinxContext>,
        IAirportWatchHistoricalParkingEntityService
    {
        public AirportWatchHistoricalParkingEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
