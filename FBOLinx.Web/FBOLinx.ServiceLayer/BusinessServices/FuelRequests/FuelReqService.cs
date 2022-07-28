using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelRequests
{
    public interface IFuelReqService : IBaseDTOService<FuelReqDto, FuelReq>
    {
        Task<List<FuelReq>> GetRecentFuelRequestsForFbo(int fboId);
    }

    public class FuelReqService : BaseDTOService<FuelReqDto, DB.Models.FuelReq, FboLinxContext>, IFuelReqService
    {
        private FuelReqEntityService _FuelReqEntityService;

        public FuelReqService(FuelReqEntityService fuelReqEntityService) : base(fuelReqEntityService)
        {
            _FuelReqEntityService = fuelReqEntityService;
        }

        public async Task<List<FuelReq>> GetRecentFuelRequestsForFbo(int fboId)
        {
            var startDate = DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0));
            var endDate = DateTime.UtcNow.Add(new TimeSpan(3, 0, 0, 0));
            var requests =
                await _FuelReqEntityService.GetListBySpec(new FuelReqByFboAndDateSpecification(fboId, startDate, endDate));

            return requests;
        }
    }
}
