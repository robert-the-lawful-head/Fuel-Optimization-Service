using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegDataEntityService : DegaBaseEntityService<SWIMFlightLegData, SWIMFlightLegDataDTO, int>, IEntityService<SWIMFlightLegData, SWIMFlightLegDataDTO, int>
    {
        public SWIMFlightLegDataEntityService(DegaContext context) : base(context)
        {
        }
    }
}
