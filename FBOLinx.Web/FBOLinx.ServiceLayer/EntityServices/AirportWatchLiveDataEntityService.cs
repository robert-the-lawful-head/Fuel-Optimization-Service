using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AirportWatchLiveDataEntityService : Repository<AirportWatchLiveData, FboLinxContext>
    {
        public AirportWatchLiveDataEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
