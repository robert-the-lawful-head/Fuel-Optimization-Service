using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMUnrecognizedFlightLegEntityService : Repository<SWIMUnrecognizedFlightLeg, DegaContext>
    {
        public SWIMUnrecognizedFlightLegEntityService(DegaContext context) : base(context)
        {
        }
    }
}
