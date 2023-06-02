using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO.SWIM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using Mapster;

namespace FBOLinx.ServiceLayer.BusinessServices.SWIMS
{
    public interface ISWIMFlightLegDataService : IBaseDTOService<SWIMFlightLegDataDTO, DB.Models.SWIMFlightLegData>
    {
        Task<List<SWIMFlightLegDataDTO>> GetSwimFlightLegDataBySwimFlightLegIds(
            List<long> swimFlightLegIds, DateTime? minMessageDateTimeUtc = null);
    }

    public class SWIMFlightLegDataService : BaseDTOService<SWIMFlightLegDataDTO, DB.Models.SWIMFlightLegData, DegaContext>, ISWIMFlightLegDataService
    {
        SWIMFlightLegDataEntityService _SwimFlightLegEntityService;
        public SWIMFlightLegDataService(SWIMFlightLegDataEntityService entityService) : base(entityService)
        {
            _SwimFlightLegEntityService = entityService;
        }

        public async Task<List<SWIMFlightLegDataDTO>> GetSwimFlightLegDataBySwimFlightLegIds(
            List<long> swimFlightLegIds, DateTime? minMessageDateTimeUtc = null)
        {
            var result = await _SwimFlightLegEntityService.GetSwimFlightLegData(swimFlightLegIds);
            return result == null ? default(List<SWIMFlightLegDataDTO>) : result.Adapt<List<SWIMFlightLegDataDTO>>();
        }
    }
}
