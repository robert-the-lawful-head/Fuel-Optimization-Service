using FBOLinx.DB;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class AircraftHexTailMappingEntityService : Repository<AircraftHexTailMapping, DegaContext>
    {
        public AircraftHexTailMappingEntityService(DegaContext context) : base(context)
        {
        }
    }
}
