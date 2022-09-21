using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.Requests.Integrations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IntegrationPartnerController : ControllerBase
    {
        private IIntegrationStatusService _IntegrationStatusService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtManager _jwtManager;
        private IAPIKeyManager _apiKeyManager;
        private readonly FboLinxContext _context;
        private FbopricesService _fboPricesService;

        public IntegrationPartnerController(IIntegrationStatusService integrationStatusService, IHttpContextAccessor httpContextAccessor, JwtManager jwtManager, IAPIKeyManager apiKeyManager, FboLinxContext context, FbopricesService fbopricesService)
        {
            _IntegrationStatusService = integrationStatusService;
            _httpContextAccessor = httpContextAccessor;
            _jwtManager = jwtManager;
            _apiKeyManager = apiKeyManager;
            _context = context;
            _fboPricesService = fbopricesService;
        }


        // POST: api/integrationpartner/update-integration-status
        [HttpPost("update-integration-status")]
        [APIKey(IntegrationPartnerTypes.OtherSoftware)]
        public async Task<IActionResult> UpdateIntegrationStatus(IntegrationStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid body request!" });
            }

            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                var claimPrincipal = _jwtManager.GetPrincipal(token);
                var claimedId = Convert.ToInt32(claimPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);

                var user = await _context.User.FindAsync(claimedId);
                var integrationPartner = await _apiKeyManager.GetIntegrationPartner();

                if (user.FboId > 0 && integrationPartner.Oid > 0)
                {
                    //Update status
                    await _IntegrationStatusService.UpdateIntegrationStatus(new Service.Mapping.Dto.IntegrationStatusDTO() { FboId = user.FboId, IntegrationPartnerId = integrationPartner.Oid, IsActive = request.IsActive });

                    //Expire any active pricing if deactivated
                    if (!request.IsActive)
                    {
                        await _fboPricesService.ExpirePricingForFbo(user.FboId);
                    }

                    return Ok(new { message = "Success" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid user" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
