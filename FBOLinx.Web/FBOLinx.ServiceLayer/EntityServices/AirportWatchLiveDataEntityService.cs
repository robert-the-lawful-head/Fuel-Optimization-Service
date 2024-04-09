using System;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using Mapster;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AirportWatchLiveDataEntityService : Repository<AirportWatchLiveData, FboLinxContext>
    {
        public AirportWatchLiveDataEntityService(FboLinxContext context) : base(context)
        {
            
        }
    }
}
