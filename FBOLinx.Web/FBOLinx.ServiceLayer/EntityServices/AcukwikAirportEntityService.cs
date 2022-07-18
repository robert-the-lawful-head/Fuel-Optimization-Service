using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AcukwikAirportEntityService : DegaBaseEntityService<AcukwikAirport, AcukwikAirportDTO, int>, IEntityService<AcukwikAirport, AcukwikAirportDTO, int>
    {
        public AcukwikAirportEntityService(DegaContext context) : base(context)
        {
        }
    }
}
