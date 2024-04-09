using FBOLinx.DB;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class FAAAircraftMakeModelEntityService : Repository<FAAAircraftMakeModelReference, DegaContext>
    {
        public FAAAircraftMakeModelEntityService(DegaContext context) : base(context)
        {
        }
    }
}
