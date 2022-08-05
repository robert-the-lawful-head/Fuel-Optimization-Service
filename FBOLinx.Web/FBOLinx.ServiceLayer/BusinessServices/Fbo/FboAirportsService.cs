using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.DB.Specifications.Fbo;

namespace FBOLinx.ServiceLayer.BusinessServices.Fbo
{
    public interface IFboAirportsService : IBaseDTOService<FboAirportsDTO, Fboairports>
    {
        Task<FboAirportsDTO> GetFboAirportsByFboId(int fboId);
    }
    public class FboAirportsService : BaseDTOService<FboAirportsDTO, DB.Models.Fboairports, FboLinxContext>, IFboAirportsService
    {
        private IFboAirportsEntityService _fboAirportsEntityService;
        public FboAirportsService(IFboAirportsEntityService fboAirportsEntityService) : base(fboAirportsEntityService)
        {
            _fboAirportsEntityService = fboAirportsEntityService;
        }

        public async Task<FboAirportsDTO> GetFboAirportsByFboId(int fboId)
        {
            var fboAirport = await _fboAirportsEntityService.GetFboAirportByFboId(fboId);
            return fboAirport;
        }
    }
}
