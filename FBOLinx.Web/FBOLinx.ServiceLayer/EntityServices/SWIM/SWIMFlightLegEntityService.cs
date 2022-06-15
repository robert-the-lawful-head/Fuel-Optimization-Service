using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegEntityService : DegaBaseEntityService<SWIMFlightLegs, SWIMFlightLegDTO, int>, IEntityService<SWIMFlightLegs, SWIMFlightLegDTO, int>
    {
        public SWIMFlightLegEntityService(DegaContext context) : base(context)
        {
        }
    }
}
