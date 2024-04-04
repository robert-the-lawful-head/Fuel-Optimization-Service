using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications.AirportWatchDistinctBoxes;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface IAirportWatchDistinctBoxesService : IBaseDTOService<AirportWatchDistinctBoxesDTO, DB.Models.AirportWatchDistinctBoxes>
    {
        Task<List<AirportWatchDistinctBoxesDTO>> GetAllAirportWatchDistinctBoxes();
        Task<AirportWatchDistinctBoxesDTO> GetBoxByName(string boxName);
        Task<List<AirportWatchDistinctBoxesDTO>> GetBoxByName(List<string> boxNames);
    }

    public class AirportWatchDistinctBoxesService : BaseDTOService<AirportWatchDistinctBoxesDTO, DB.Models.AirportWatchDistinctBoxes, FboLinxContext>, IAirportWatchDistinctBoxesService
    {
        public AirportWatchDistinctBoxesService(IAirportWatchDistinctBoxesEntityService entityService) : base(entityService)
        {
            _EntityService = entityService;
        }

        public async Task<List<AirportWatchDistinctBoxesDTO>> GetAllAirportWatchDistinctBoxes()
        {
            var airportWatchDistinctBoxes = await GetListbySpec(new AirportWatchDistinctBoxesSpecification(0, true));
            return airportWatchDistinctBoxes;
        }
        public async Task<AirportWatchDistinctBoxesDTO> GetBoxByName(string boxName)
        {
            return await GetSingleBySpec(new AirportWatchDistinctBoxesSpecification(boxName));
        }

        public async Task<List<AirportWatchDistinctBoxesDTO>> GetBoxByName(List<string> boxNameList)
        {
            return await GetListbySpec(new AirportWatchDistinctBoxesSpecification(boxNameList));
        }
    }
}
