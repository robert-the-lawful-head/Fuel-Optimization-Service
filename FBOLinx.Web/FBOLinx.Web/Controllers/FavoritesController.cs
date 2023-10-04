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

        //POST api/favorites/fbo/company
        [HttpPost("company")]
        public async Task<ActionResult<FboFavoriteCompany>> CreateCompanyFavorite(FboFavoriteCompany fboFavoriteCompany)
        {
            var result = await _fboCompaniesFavoritesService.AddCompanyFavorite(fboFavoriteCompany);
            return Ok(result);
        }
        //POST api/favorites/aircraft
        [HttpPost("aircraft")]
        public async Task<ActionResult<FboFavoriteAircraft>> CreateAoircraftFavorite([FromBody] FboFavoriteAircraft fboFavoriteAircraft)
        {
            var result = await _fboAircraftFavoritesService.AddAircraftFavorite(fboFavoriteAircraft);
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
            await _fboAircraftFavoritesService.DeleteAircraftFavorite(oid);
            return NoContent();
        }
    }
}