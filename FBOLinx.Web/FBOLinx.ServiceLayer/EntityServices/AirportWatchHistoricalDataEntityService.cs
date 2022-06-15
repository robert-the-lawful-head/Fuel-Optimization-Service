using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AirportWatchHistoricalDataEntityService : FBOLinxBaseEntityService<AirportWatchHistoricalData, AirportWatchHistoricalDataDto, int>, IEntityService<AirportWatchHistoricalData, AirportWatchHistoricalDataDto, int>
    {
        public AirportWatchHistoricalDataEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
