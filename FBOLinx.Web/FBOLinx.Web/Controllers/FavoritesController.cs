using System.Threading.Tasks;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.ServiceLayer.BusinessServices.Favorites;

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

        //POST api/favorites/fbo/{fboId}/CustomerInfoByGroup/{customerInfoByGroupId}
        [HttpPost("fbo/{fboId}/group/{customerInfoByGroupId}")]
        public async Task<ActionResult<FboFavoriteCompany>> CreateCompanyFavorite(int fboid, int customerInfoByGroupId)
        {
            var result = await _fboCompaniesFavoritesService.AddCompanyFavorite(fboid, customerInfoByGroupId);
            return Ok(result);
        }
        //POST api/favorites/fbo/{fboId}/aircraft/{aircraftId}
        [HttpPost("fbo/{fboId}/aircraft/{aircraftId}")]
        public async Task<ActionResult<FboFavoriteAircraft>> CreateAoircraftFavorite(int fboid, int aircraftId)
        {
            var result = await _fboAircraftFavoritesService.AddAircraftFavorite(fboid, aircraftId);
            return Ok(result);
        }
        //DELETE api/favorites/company/{oid}        
        [HttpDelete("company/{oid}")]
        public async Task<IActionResult> DeleteFavoriteCompany(int oid)
        {
            await _fboCompaniesFavoritesService.DeleteCompanyFavorite(oid);
            return NoContent();
        }
        //DELETE api/favorites/aircraft/{oid}
        [HttpDelete("aircraft/{oid}")]
        public async Task<IActionResult> DeleteFavoriteAircraft(int oid)
        {
            await _fboCompaniesFavoritesService.DeleteCompanyFavorite(oid);
            return NoContent();
        }
    }
}