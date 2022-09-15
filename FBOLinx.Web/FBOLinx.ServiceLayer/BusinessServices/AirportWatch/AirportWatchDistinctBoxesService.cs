using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications.AirportWatchDistinctBoxes;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface IAirportWatchDistinctBoxesService : IBaseDTOService<AirportWatchDistinctBoxesDTO, DB.Models.AirportWatchDistinctBoxes>
    {
        Task<List<AirportWatchDistinctBoxesDTO>> GetAllAirportWatchDistinctBoxes();
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
    }
}
