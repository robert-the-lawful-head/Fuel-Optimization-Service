using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class FuelReqEntityService : Repository<FuelReq, FboLinxContext>
    {
        public FuelReqEntityService(FboLinxContext context) : base(context)
        {

        }
    }
}
