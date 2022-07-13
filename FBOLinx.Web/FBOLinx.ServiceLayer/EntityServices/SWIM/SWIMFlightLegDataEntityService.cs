using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegDataEntityService : Repository<SWIMFlightLegData, DegaContext>
    {
        public SWIMFlightLegDataEntityService(DegaContext context) : base(context)
        {
        }
    }
}
