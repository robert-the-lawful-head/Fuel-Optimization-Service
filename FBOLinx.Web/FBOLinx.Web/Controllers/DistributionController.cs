using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Configurations;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DistributionController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly FuelerLinxContext _fuelerLinxContext;
        private IHttpContextAccessor _HttpContextAccessor;
        private IFileProvider _FileProvider;
        private IOptions<MailSettings> _MailSettings;
        private JwtManager _jwtManager;
        private PriceDistributionService _PriceDistributionService;

        public DistributionController(FboLinxContext context, FuelerLinxContext fuelerLinxContext, IHttpContextAccessor httpContextAccessor, IFileProvider fileProvider, IOptions<MailSettings> mailSettings, JwtManager jwtManager, PriceDistributionService priceDistributionService)
        {
            _MailSettings = mailSettings;
            _FileProvider = fileProvider;
            _HttpContextAccessor = httpContextAccessor;
            _context = context;
            _fuelerLinxContext = fuelerLinxContext;
            _jwtManager = jwtManager;
            _PriceDistributionService = priceDistributionService;
        }

        // GET: fbo/5/log/10
        [HttpGet("fbo/{fboId}/log/{resultCount}")]
        public async Task<IActionResult> GetDistributionLogForFbo([FromRoute] int fboId, [FromRoute] int resultCount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var distributionLog = await (from dl in _context.DistributionLog
                join cg in _context.CustomerInfoByGroup on new
                    {
                        CustomerId = dl.CustomerId.GetValueOrDefault(),
                        GroupId = dl.GroupId.GetValueOrDefault()
                    } equals new {cg.CustomerId, cg.GroupId}
                    into leftJoinCustomerInfoByGroup
                from cg in leftJoinCustomerInfoByGroup.DefaultIfEmpty()
                join pt in _context.PricingTemplate on new
                {
                    PricingTemplateId = dl.PricingTemplateId.GetValueOrDefault()
                } equals new
                {
                    PricingTemplateId = pt.Oid
                }
                into leftJoinPricingtemplate
                from pt in leftJoinPricingtemplate.DefaultIfEmpty()
                join cct in _context.CustomerCompanyTypes on new
                {
                    dl.CustomerCompanyType
                } equals new
                {
                    CustomerCompanyType = cct.Oid
                }
                into leftJoinCCT
                from cct in leftJoinCCT.DefaultIfEmpty()
                where dl.Fboid == fboId
                select new
                {
                    dl.Oid,
                    dl.DateSent,
                    dl.Fboid,
                    dl.GroupId,
                    dl.UserId,
                    dl.PricingTemplateId,
                    dl.CustomerId,
                    dl.CustomerCompanyType,
                    CustomerName = cg != null ? cg.Company : "All",
                    PricingTemplateName = pt != null ? pt.Name : "All",
                    CustomerCompanyTypeName = cct != null ? cct.Name : "All"
                }).OrderByDescending(x => x.DateSent).ToListAsync();

            return Ok(distributionLog);
        }

        // GET: fbo/5/validity
        [HttpGet("fbo/{fboId}/validity")]
        public async Task<IActionResult> GetDistributionValidityForFbo([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentPrices = await (from f in _context.Fboprices
                where f.EffectiveTo > DateTime.UtcNow && f.Fboid == fboId && f.Expired != true
                select f).ToListAsync();

            if (currentPrices.Count == 0)
                return Ok(new { message = "No current prices available.  Please update your pricing for this week."});

            return Ok(new { message = "" });
        }

        // POST: api/distribution/distributepricing
        [HttpPost("distributepricing")]
        public async Task<IActionResult> PostDistributePricing(Models.Requests.DistributePricingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.FboId != UserService.GetClaimedFboId(_HttpContextAccessor) && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.GroupAdmin && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.Conductor)
                return BadRequest(ModelState);

            await _PriceDistributionService.DistributePricing(request, false);

            return Ok();
        }

        // POST: api/distribution/previewdistribution
        [HttpPost("previewdistribution")]
        public async Task<IActionResult> PostPreviewDistribution(Models.Requests.DistributePricingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.FboId != UserService.GetClaimedFboId(_HttpContextAccessor) && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.GroupAdmin && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.Conductor)
                return BadRequest(ModelState);

            await _PriceDistributionService.DistributePricing(request, true);

            return Ok();
        }
    }
}
