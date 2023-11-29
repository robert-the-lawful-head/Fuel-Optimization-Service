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


        public FavoritesController(IFboAircraftFavoritesService fboAircraftFavoritesService, IFboCompaniesFavoritesService fboCompaniesFavoritesService, ILoggingService logger) : base(logger)
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
        [HttpPost("aircraft/group/{groupId}")]
        public async Task<ActionResult<FboFavoriteAircraft>> CreateAoircraftFavorite([FromBody] FboFavoriteAircraft fboFavoriteAircraft, int groupid)
        {
            var result = await _fboAircraftFavoritesService.AddAircraftFavorite(fboFavoriteAircraft, groupid);
            return Ok(result);
        }
        //DELETE api/favorites/company/{oid}        
        [HttpDelete("company/{oid}")]
        public async Task<IActionResult> DeleteFavoriteCompany(int oid)
        {
            await _fboCompaniesFavoritesService.DeleteCompanyFavorite(oid);
            return NoContent();
        }
        //DELETE api/favorites/aircraft/{oid}/group/{groupId}
        [HttpDelete("aircraft/{oid}/group/{groupId}")]
        public async Task<IActionResult> DeleteFavoriteAircraft(int oid,int groupId)
        {
            await _fboAircraftFavoritesService.DeleteAircraftFavorite(oid, groupId);
            return NoContent();
        }
    }
}