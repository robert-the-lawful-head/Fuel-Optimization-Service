using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AFSAircraftEntityService : Repository<AFSAircraft, DegaContext>
    {
        public AFSAircraftEntityService(DegaContext context) : base(context)
        {
        }
    }
}
