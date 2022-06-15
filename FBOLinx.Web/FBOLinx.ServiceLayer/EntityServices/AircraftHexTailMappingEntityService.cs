using FBOLinx.DB;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class AircraftHexTailMappingEntityService : DegaBaseEntityService<AircraftHexTailMapping, AircraftHexTailMappingDTO, int>, IEntityService<AircraftHexTailMapping, AircraftHexTailMappingDTO, int>
    {
        public AircraftHexTailMappingEntityService(DegaContext context) : base(context)
        {
        }
    }
}
