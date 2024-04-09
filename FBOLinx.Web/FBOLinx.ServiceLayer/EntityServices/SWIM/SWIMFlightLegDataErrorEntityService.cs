using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegDataErrorEntityService : Repository<SWIMFlightLegDataError, DegaContext>
    {
        public SWIMFlightLegDataErrorEntityService(DegaContext context) : base(context)
        {
        }
    }
}
