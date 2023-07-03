using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface IAirportWatchHistoricalParkingService : IBaseDTOService<AirportWatchHistoricalParkingDto,
        DB.Models.AirportWatchHistoricalParking>
    {

    }

    public class AirportWatchHistoricalParkingService : BaseDTOService<AirportWatchHistoricalParkingDto, DB.Models.AirportWatchHistoricalParking, FboLinxContext>, IAirportWatchHistoricalParkingService
    {
        public AirportWatchHistoricalParkingService(IAirportWatchHistoricalParkingEntityService entityService) : base(entityService)
        {
        }
    }
}
