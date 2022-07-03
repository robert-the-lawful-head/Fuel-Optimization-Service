using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegRepository : Repository<SWIMFlightLeg, DegaContext>
    {
        public SWIMFlightLegRepository(DegaContext context) : base(context)
        {
        }
    }
}
