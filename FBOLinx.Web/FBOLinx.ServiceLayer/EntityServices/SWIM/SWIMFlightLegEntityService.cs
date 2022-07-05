using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegEntityService : Repository<SWIMFlightLeg, DegaContext>
    {
        public SWIMFlightLegEntityService(DegaContext context) : base(context)
        {
        }
    }
}
