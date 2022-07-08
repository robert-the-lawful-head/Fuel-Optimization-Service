using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AcukwikAirportEntityService : Repository<AcukwikAirport, DegaContext>
    {
        public AcukwikAirportEntityService(DegaContext context) : base(context)
        {
        }
    }
}
