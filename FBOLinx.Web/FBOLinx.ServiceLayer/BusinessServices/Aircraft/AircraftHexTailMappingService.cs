using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{
    public interface IAircraftHexTailMappingService : IBaseDTOService<AircraftHexTailMappingDTO, AircraftHexTailMapping>
    {
        Task<List<AircraftHexTailMappingDTO>> GetAircraftHexTailMappingsForTails(List<string> tailNumbers);
    }

    public class AircraftHexTailMappingService :
        BaseDTOService<AircraftHexTailMappingDTO, AircraftHexTailMapping, DegaContext>, IAircraftHexTailMappingService
    {
        private IAircraftHexTailMappingEntityService _AircrafthexTailMappingEntityService;

        public AircraftHexTailMappingService(IAircraftHexTailMappingEntityService entityService) : base(entityService)
        {
            _AircrafthexTailMappingEntityService = entityService;
        }

        public async Task<List<AircraftHexTailMappingDTO>> GetAircraftHexTailMappingsForTails(List<string> tailNumbers)
        {
            return await _AircrafthexTailMappingEntityService.GetAircraftHexTailMappingsForTails(tailNumbers);
        }
    }
}
