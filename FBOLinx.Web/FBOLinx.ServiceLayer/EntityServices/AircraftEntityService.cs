using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AircraftEntityService: Repository<AirCrafts, DegaContext>
    {
        public AircraftEntityService(DegaContext context) : base(context)
        {
        }
    }
}
