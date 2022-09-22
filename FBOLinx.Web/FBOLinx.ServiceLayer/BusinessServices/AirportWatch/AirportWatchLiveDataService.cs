using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface
        IAirportWatchLiveDataService : IBaseDTOService<AirportWatchLiveDataDto, DB.Models.AirportWatchLiveData>
    {

    }

    public class AirportWatchLiveDataService : BaseDTOService<AirportWatchLiveDataDto, DB.Models.AirportWatchLiveData, FboLinxContext>, IAirportWatchLiveDataService
    {
        public AirportWatchLiveDataService(IRepository<DB.Models.AirportWatchLiveData, FboLinxContext> entityService) : base(entityService)
        {
        }
    }
}