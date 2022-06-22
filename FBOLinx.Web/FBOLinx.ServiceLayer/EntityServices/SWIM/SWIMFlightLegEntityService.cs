using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegEntityService : DegaBaseEntityService<SWIMFlightLeg, SWIMFlightLegDTO, int>, IEntityService<SWIMFlightLeg, SWIMFlightLegDTO, int>
    {
        public SWIMFlightLegEntityService(DegaContext context) : base(context)
        {
        }
    }
}
