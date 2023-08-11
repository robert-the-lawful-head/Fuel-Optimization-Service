using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Core.Utilities.Geography;
using FBOLinx.Service.Mapping.Dto;
using Mapster;
using System;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.Enums;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.ServiceLayer.BusinessServices.Favorites;
using Microsoft.Extensions.Hosting;
using SendGrid.Helpers.Mail;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : FBOLinxControllerBase
    {
        private IFboAircraftFavoritesService _fboAircraftFavoritesService;
        private IFboCompaniesFavoritesService _fboCompaniesFavoritesService;


        public FavoritesController(IFboAircraftFavoritesService fboAircraftFavoritesService, IFboCompaniesFavoritesService fboCompaniesFavoritesService, ILoggingService logger,
            IAirportTimeService airportTimeService) : base(logger)
        {
            _fboAircraftFavoritesService = fboAircraftFavoritesService;
            _fboCompaniesFavoritesService = fboCompaniesFavoritesService;
        }

        //POST api/favorites/fbo/{fboId}/CustomerInfoByGroup/{CustomerInfoByGroupId}
        [HttpPost("fbo/{fboId}/group/{CustomerInfoByGroupId}")]
        public async Task<ActionResult<FboFavoriteCompanies>> CreateCompanyFavorite(int fboid, int customerinfoBybroupId)
        {
            return Ok(await _fboCompaniesFavoritesService.AddCompanyFavorite(fboid,customerinfoBybroupId));
        }
        //POST api/favorites/fbo/{fboId}/aircraft/{aircraftId}
        [HttpPost("fbo/{fboId}/aircraft/{aircraftId}")]
        public async Task<ActionResult<FboFavoriteAircrafts>> CreateAoircraftFavorite(int fboid, int aircraftId)
        {
            return Ok(await _fboAircraftFavoritesService.AddAircraftFavorite(fboid, aircraftId));
        }
        //DELETE api/favorites/company/{Oid}        
        [HttpDelete("companies/{Oid}}")]
        public async Task<IActionResult> DeleteFavoriteCompany(int Oid)
        {
            await _fboCompaniesFavoritesService.DeleteCompanyFavorite(Oid);
            return NoContent();
        }
        //DELETE api/favorites/aircraft/{Oid}
        [HttpDelete("aircraft/{Oid}}")]
        public async Task<IActionResult> DeleteFavoriteAircraft(int Oid)
        {
            await _fboCompaniesFavoritesService.DeleteCompanyFavorite(Oid);
            return NoContent();
        }
    }
}