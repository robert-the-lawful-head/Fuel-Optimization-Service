using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Auth;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using FBOLinx.Web.Models.Requests;
using Microsoft.Extensions.DependencyInjection;
using FBOLinx.Web.Services;


namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FbosController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private GroupFboService _groupFboService;
        private readonly FboService _fboService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly OAuthService _oAuthService;

        public FbosController(FboLinxContext context, DegaContext degaContext, GroupFboService groupFboService, FboService fboService, IHttpContextAccessor httpContextAccessor, OAuthService oAuthService)
        {
            _groupFboService = groupFboService;
            _context = context;
            _degaContext = degaContext;
            _fboService = fboService;
            _httpContextAccessor = httpContextAccessor;
            _oAuthService = oAuthService;
        }

        // GET: api/Fbos/group/5
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetFboByGroupID([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fbos = await GetAllFbos().Include("fboAirport").Where((x => x.GroupId == groupId)).ToListAsync();

            //FbosViewModel used to display FBO info in the grid
            var fbosVM = fbos.Select(f => new FbosGridViewModel
            {
                Active = f.Active,
                Fbo = f.Fbo,
                Icao = f.fboAirport == null ? "" : f.fboAirport.Icao,
                Iata = f.fboAirport == null ? "" : f.fboAirport.Iata,
                Oid = f.Oid,
                GroupId = (f.GroupId ?? 0)
            }).Distinct().ToList();
            return Ok(fbosVM);
        }

        // GET: api/Fbos
        [HttpGet]
        public async Task<IActionResult> GetFbos()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fbos = await GetAllFbos().Include(f => f.Users).Include("fboAirport").ToListAsync();
            if (fbos == null)
            {
                return NotFound();
            }

            //FbosViewModel used to display FBO info in the grid
            var fbosVM = fbos.Select(f => new FbosGridViewModel
            {
                Active = f.Active,
                Fbo = f.Fbo,
                Icao = f.fboAirport?.Icao,
                Oid = f.Oid,
                GroupId = f.GroupId ?? 0,
                Users = f.Users
            }).ToList();
            return Ok(fbosVM);
        }

        // GET: api/Fbos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFbo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbos = await _context.Fbos.FindAsync(id);

            if (fbos == null)
            {
                return NotFound();
            }

            return Ok(fbos);
        }

        // PUT: api/Fbos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFbos([FromRoute] int id, [FromBody] Fbos fbos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fbos.Oid)
            {
                return BadRequest();
            }

            bool activeStatus = await _context.Fbos.Where(f => f.Oid.Equals(fbos.Oid))
                                                   .Select(f => f.Active ?? false)
                                                   .FirstAsync();
            if (activeStatus != fbos.Active)
            {
                List<User> users = _context.User.Where(u => u.FboId.Equals(id))
                                            .ToList();
                foreach (var user in users)
                {
                    user.Active = fbos.Active ?? false;
                }

                _context.User.UpdateRange(users);

                if (!fbos.Active.GetValueOrDefault())
                {
                    // Expire All Prices from the de-activated FBOLinx accounts
                    var fboPrices = _context.Fboprices.Where(fp => fp.Fboid.Equals(id));

                    foreach (var fboPrice in fboPrices)
                    {
                        fboPrice.Expired = true;
                    }

                    _context.Fboprices.UpdateRange(fboPrices);
                }
            }

            fbos.Fbo = fbos.Fbo.Trim();

            _context.Fbos.Update(fbos);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FbosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Fbos
        [HttpPost]
        public async Task<IActionResult> PostFbos([FromBody] SingleFboRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = await _groupFboService.CreateNewFbo(request);

            PricingTemplateService pricingTemplateService = new PricingTemplateService(_context);

            await pricingTemplateService.FixCustomCustomerTypes(request.GroupId ?? 0, fbo.Oid);

            return CreatedAtAction("GetFbo", new { id = fbo.Oid }, new
            {
                request.Icao,
                request.Iata,
                Active = true,
                request.Fbo,
                fbo.Oid
            });
        }

        [HttpPost("single")]
        public async Task<IActionResult> CreateSingleFbo([FromBody] SingleFboRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var fbo = await _groupFboService.CreateNewFbo(request);

                return CreatedAtAction("GetFbo", new { id = fbo.Oid }, new
                {
                    request.Icao,
                    request.Iata,
                    Active = true,
                    request.Fbo,
                    fbo.Oid
                });
            }
            catch (System.Exception exception)
            {
                return Ok(exception.Message);
            }
        }

        [HttpPost("manage/{id}")]
        public async Task<IActionResult> ManageFbo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = _context.Fbos.Find(id);
            if (fbo == null)
            {
                return NotFound("FBO Not Found");
            }

            try
            {
                var group = _context.Group.Find(fbo.GroupId);

                if (group.IsLegacyAccount == true)
                {
                    await _fboService.DoLegacyGroupTransition(group.Oid);

                    group.IsLegacyAccount = false;
                    await _context.SaveChangesAsync();
                }

                PricingTemplateService pricingTemplateService = new PricingTemplateService(_context);

                await pricingTemplateService.FixDefaultPricingTemplate(fbo.Oid);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
            return Ok();
        }

        // DELETE: api/Fbos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFbos([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbos = await _context.Fbos.FindAsync(id);
            if (fbos == null)
            {
                return NotFound();
            }

            _context.Fbos.Remove(fbos);
            await _context.SaveChangesAsync();

            return Ok(fbos);
        }

        // GET: api/getfbologo
        [HttpGet("getfbologo/{fboId}")]
        public async Task<IActionResult> GetFboLogo([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var logoUrl = await _fboService.GetLogo(fboId);

                return Ok(new { Message = logoUrl });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = "" });
            }
        }

        // POST: api/get
        [HttpPost("uploadfboLogo")]
        public async Task<IActionResult> PostFboLogo([FromBody] FboLogoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (request.FileData.Contains(","))
                {
                    request.FileData = request.FileData.Substring(request.FileData.IndexOf(",") + 1);
                }

                var logoUrl = await _fboService.UploadLogo(request);

                return Ok(new { Message = logoUrl });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = "" });
            }
        }

        // DELETE: api/deletelogp/5
        [HttpDelete("fbologo/{id}")]
        public async Task<IActionResult> DeleteLogo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _fboService.DeleteLogo(id);

            return Ok();
        }


        [AllowAnonymous]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.Internal)]
        [HttpGet("by-akukwik-record/{handlerId}")]
        public async Task<ActionResult<Fbos>> GetFboByAcukwikRecord([FromRoute] int handlerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = await _context.Fbos.Where(x => x.AcukwikFBOHandlerId == handlerId).Include(x => x.Group).Include(x => x.fboAirport).Include(x => x.Contacts).ThenInclude(c => c.Contact).FirstOrDefaultAsync();

            return Ok(fbo);
        }

        [HttpGet("by-akukwik-handlerId/{handlerId}")]
        public async Task<ActionResult<Fbos>> GetFboByAcukwikHandlerId([FromRoute] int handlerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = await _context.Fbos.Where(x => x.AcukwikFBOHandlerId == handlerId).Include(x => x.Group).Include(x => x.fboAirport).Include(x => x.Contacts).ThenInclude(c => c.Contact).FirstOrDefaultAsync();

            return Ok(fbo);
        }

        // GET: api/Fbos/add-account-handlerid/100
        [AllowAnonymous]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.OtherSoftware)]
        [HttpGet("add-account-handlerid/{handlerId}")]
        public async Task<ActionResult<string>> AddNonRevAccountByAcukwikHandlerId([FromRoute] int handlerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = await _context.Fbos.Where(x => x.AcukwikFBOHandlerId == handlerId).FirstOrDefaultAsync();

            if (fbo == null)
            {
                var acukwikFbo = await _degaContext.AcukwikFbohandlerDetail.Where(x => x.AcukwikId == handlerId).FirstOrDefaultAsync();
                if (acukwikFbo == null)
                    return BadRequest("FBO not found");

                var acukwikAirport = await _degaContext.AcukwikAirports.Where(x => x.AirportId == acukwikFbo.AirportId).FirstOrDefaultAsync();

                var importedFboEmail = await _degaContext.ImportedFboEmails.Where(x => x.AcukwikFBOHandlerId == handlerId).FirstOrDefaultAsync();

                if (importedFboEmail != null)
                {
                    var newFbo = new SingleFboRequest() { Icao = acukwikAirport.Icao, Iata = acukwikAirport.Iata, Fbo = acukwikFbo.HandlerLongName, AcukwikFboHandlerId = handlerId, AccountType = Fbos.AccountTypes.NonRevFBO, FuelDeskEmail = importedFboEmail.Email };
                    fbo = await _groupFboService.CreateNewFbo(newFbo);

                    User newUser = new User() { FboId = fbo.Oid, Role = DB.Models.User.UserRoles.NonRev, Username = importedFboEmail.Email, FirstName = importedFboEmail.Email, GroupId = fbo.GroupId };
                    _context.User.Add(newUser);
                    await _context.SaveChangesAsync();
                }
            }

            var user = await _context.User.Where(x => x.FboId == fbo.Oid && x.Role == DB.Models.User.UserRoles.NonRev).FirstOrDefaultAsync();

            //Return URL with authentication for 3 days
            AccessTokens accessToken = await _oAuthService.GenerateAccessToken(user, 4320);
            return Ok("https://" + _httpContextAccessor.HttpContext.Request.Host + "/outside-the-gate-layout/auth?token=" + HttpUtility.UrlEncode(accessToken.AccessToken));
        }

        private bool FbosExists(int id)
        {
            return _context.Fbos.Any(e => e.Oid == id);
        }

        private IQueryable<Fbos> GetAllFbos()
        {
            return _context.Fbos.AsQueryable();
        }
    }
}