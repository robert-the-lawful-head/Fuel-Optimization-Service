using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegDataRepository : Repository<SWIMFlightLegData, DegaContext>
    {
        public SWIMFlightLegDataRepository(DegaContext context) : base(context)
        {
        }
    }
}
