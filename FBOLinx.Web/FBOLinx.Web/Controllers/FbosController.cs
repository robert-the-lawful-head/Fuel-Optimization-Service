using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Auth;
using System.Web;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services;
using FBOLinx.Web.Services.Interfaces;
using Fuelerlinx.SDK;

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
        private readonly IPriceFetchingService _priceFetchingService;
        private readonly RampFeesService _rampFeeService;
        private readonly FuelerLinxApiService _fuelerLinxApiService;
        private readonly FbopricesService _fbopricesService;
        private readonly IPricingTemplateService _pricingTemplateService;

        public FbosController(
            FboLinxContext context,
            DegaContext degaContext, 
            GroupFboService groupFboService, 
            FboService fboService, 
            IHttpContextAccessor httpContextAccessor, 
            OAuthService oAuthService, 
            IPriceFetchingService priceFetchingService, 
            RampFeesService rampFeeService, 
            FuelerLinxApiService fuelerLinxApiService,
            FbopricesService fbopricesService,
            IPricingTemplateService pricingTemplateService)
        {
            _groupFboService = groupFboService;
            _context = context;
            _degaContext = degaContext;
            _fboService = fboService;
            _httpContextAccessor = httpContextAccessor;
            _oAuthService = oAuthService;
            _priceFetchingService = priceFetchingService;
            _rampFeeService = rampFeeService;
            _fuelerLinxApiService = fuelerLinxApiService;
            _fbopricesService = fbopricesService;
            _pricingTemplateService = pricingTemplateService;
        }

        // GET: api/Fbos/group/5
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetFboByGroupID([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbos = await (
                            from f in _context.Fbos
                            join fa in _context.Fboairports on f.Oid equals fa.Fboid
                            where f.GroupId == groupId && f.Active == true
                            select new FbosGridViewModel
                            {
                                Active = f.Active,
                                Fbo = f.Fbo,
                                Icao = fa.Icao,
                                Iata = fa.Iata,
                                Oid = f.Oid,
                                GroupId = f.GroupId ?? 0,
                            }).ToListAsync();

            foreach (var fbo in fbos)
            {
                var prices = await _fbopricesService.GetPrices(fbo.Oid);
                var currentPrices = prices.Where(p => p.Product.Contains("JetA") && p.EffectiveFrom <= DateTime.UtcNow).ToList();
                if (currentPrices.Count > 0)
                {
                    fbo.CostPrice = currentPrices[0].Price;
                    fbo.RetailPrice = currentPrices[1].Price;
                }
            }

            return Ok(fbos);
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

            await _pricingTemplateService.FixCustomCustomerTypes(request.GroupId ?? 0, fbo.Oid);

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

                await _pricingTemplateService.FixDefaultPricingTemplate(fbo.Oid);
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
            catch (Exception)
            {
                return Ok(new { Message = "" });
            }
        }

        // POST: api/uploadfboLogo
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
            catch (Exception)
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
        [APIKey(Core.Enums.IntegrationPartnerTypes.Internal)]
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
        [APIKey(Core.Enums.IntegrationPartnerTypes.OtherSoftware)]
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
                var acukwikFbo = await _degaContext.AcukwikFbohandlerDetail.Where(x => x.HandlerId == handlerId).FirstOrDefaultAsync();
                if (acukwikFbo == null)
                    return BadRequest("FBO not found");

                var acukwikAirport = await _degaContext.AcukwikAirports.Where(x => x.AirportId == acukwikFbo.AirportId).FirstOrDefaultAsync();

                var importedFboEmail = await _degaContext.ImportedFboEmails.Where(x => x.AcukwikFBOHandlerId == handlerId).FirstOrDefaultAsync();

                if (importedFboEmail != null)
                {
                    var newFbo = new SingleFboRequest() { Group = acukwikFbo.HandlerLongName, Icao = acukwikAirport.Icao, Iata = acukwikAirport.Iata, Fbo = acukwikFbo.HandlerLongName, AcukwikFboHandlerId = handlerId, AccountType = Core.Enums.AccountTypes.NonRevFBO, FuelDeskEmail = importedFboEmail.Email };
                    fbo = await _groupFboService.CreateNewFbo(newFbo);

                    User newUser = new User() { FboId = fbo.Oid, Role = Core.Enums.UserRoles.NonRev, Username = importedFboEmail.Email, FirstName = importedFboEmail.Email, GroupId = fbo.GroupId };
                    _context.User.Add(newUser);
                    await _context.SaveChangesAsync();
                }
            }

            var user = await _context.User.Where(x => x.FboId == fbo.Oid && x.Role == Core.Enums.UserRoles.NonRev).FirstOrDefaultAsync();

            //Return URL with authentication for 3 days
            AccessTokens accessToken = await _oAuthService.GenerateAccessToken(user, 4320);
            return Ok("https://" + _httpContextAccessor.HttpContext.Request.Host + "/outside-the-gate-layout/auth?token=" + HttpUtility.UrlEncode(accessToken.AccessToken));
        }

        [HttpGet("sendengagementemails")]
        public async Task EngagementEmails()
        {
            //get all active fbos with expiredpricing
            var fbosWithExpiredPricing = await _priceFetchingService.GetAllFbosWithExpiredPricing();
            //var responseFbos = await _apiClient.GetAsync("fboprices/getallfboswithexpiredretailpricing", conductorUser.Token);
            //var fbos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FbosGridViewModel>>(responseFbos);

            foreach (var fbo in fbosWithExpiredPricing)
            {
                var icao = fbo.Icao.ToLower();

                var acukwikAirport = await _degaContext.AcukwikAirports.Where(x => (x != null && x.Icao == icao)).FirstOrDefaultAsync();
                //var responseAcukwikAirport = await _apiClient.GetAsync("acukwikairports/byicao/" + fbo.Icao, conductorUser.Token);
                //var acukwikAirport = Newtonsoft.Json.JsonConvert.DeserializeObject<AcukwikAirports>(responseAcukwikAirport);

                //send email if it's 9am local airport time
                var airportDateTime = DateTime.UtcNow.AddHours(acukwikAirport.IntlTimeZone.GetValueOrDefault());
                if (acukwikAirport.DaylightSavingsYn == "Y")
                    airportDateTime = airportDateTime.AddHours(1);

                if (airportDateTime.Hour == 9)
                {
                    var toEmails = await GetToEmailsForEngagementEmails(fbo.Oid);

                    if (toEmails.Count > 0)
                        await GenerateExpiredPricesEmail(toEmails, fbo.Fbo);
                }
            }

            //Ramp fee emails, only goes out at 9am Pacific
            //await CheckRampFees();
        }

        private async Task CheckRampFees()
        {
            var KVNYAirport = await _degaContext.AcukwikAirports.Where(x => x.Icao == "kvny").FirstOrDefaultAsync();
            //var responseKVNYAirport = await _apiClient.GetAsync("acukwikairports/byicao/KVNY", conductorUser.Token);
            //var KVNYAirport = Newtonsoft.Json.JsonConvert.DeserializeObject<AcukwikAirports>(responseKVNYAirport);
            var KVNYDateTime = DateTime.UtcNow.AddHours(KVNYAirport.IntlTimeZone.GetValueOrDefault());
            if (KVNYAirport.DaylightSavingsYn == "Y")
                KVNYDateTime = KVNYDateTime.AddHours(1);

            if (KVNYDateTime.Hour == 9)
            {
                //go through each active fbo icao and call fuelerlinx to pull the latest pullhistory for that icao within the past 24 hours
                var activeFbos= await GetAllFbos().Where(x => x.Active == true).Include(f => f.Users).Include("fboAirport").ToListAsync();
                //var responseActiveFbos = await _apiClient.GetAsync("fbos", conductorUser.Token);
                //var activeFbos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FbosGridViewModel>>(responseActiveFbos);

                foreach (var fbo in activeFbos)
                {
                    if (fbo.GroupId > 1)
                    {
                        //check if there's ramp fees
                        var rampFees = await _rampFeeService.GetRampFeesByFbo(fbo.Oid);
                        //var rampFeesResponse = await _apiClient.GetAsync("rampfees/fbo/" + fbo.Oid, conductorUser.Token);
                        //var rampFees = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<RampFees>>(rampFeesResponse);
                        var noRampFees = true;

                        foreach (RampFeesGridViewModel rampFee in rampFees)
                        {
                            if (rampFee.Price > 0)
                            {
                                noRampFees = false;
                                break;
                            }
                        }

                        //if not, find the latest pull history record from fuelerlinx and send email
                        if (noRampFees)
                        {
                            var fuelerLinxCustomerId = await _fuelerLinxApiService.GetLatestFlightDeptPullHistoryForIcao(new FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest() { Icao = fbo.fboAirport.Icao });
                            //var fuelerLinxCustomerId = 0;
                            //var fuelerLinxCustomerIdResponse = await _apiClient.PostAsync("fboprices/get-latest-flight-dept-pullhistory-for-icao/", new FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest() { Icao = fbo.Icao }, conductorUser.Token);
                            //if (fuelerLinxCustomerIdResponse != "" && int.TryParse(fuelerLinxCustomerIdResponse, out fuelerLinxCustomerId))
                            //    fuelerLinxCustomerId = int.Parse(fuelerLinxCustomerIdResponse);

                            if (fuelerLinxCustomerId > 0)
                            {
                                var customer = await _context.Customers.Where(x => x.FuelerlinxId == fuelerLinxCustomerId).FirstOrDefaultAsync();
                                //var customerResponse = await _apiClient.GetAsync("customers/getbyfuelerlinxid/" + fuelerLinxCustomerId, conductorUser.Token);
                                //var customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customers>(customerResponse);

                                var toEmails = await GetToEmailsForEngagementEmails(fbo.Oid);

                                if (toEmails.Count > 0)
                                    await GenerateNoRampFeesEmail(toEmails, fbo.Fbo, customer.Company, fbo.fboAirport.Icao);
                            }
                        }
                    }
                }
            }
        }

        private async Task<List<string>> GetToEmailsForEngagementEmails(int fboId)
        {
            List<string> toEmails = new List<string>();

            var fboInfo = await _context.Fbos.FindAsync(fboId);
            //var responseFbo = await _apiClient.GetAsync("fbos/" + fbo.Oid, conductorUser.Token);
            //var fboInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Fbos>(responseFbo);

            if (fboInfo.FuelDeskEmail != "")
                toEmails.Add(fboInfo.FuelDeskEmail);

            var fboContacts = await _context.Fbocontacts
                                .Include("Contact")
                                .Where(x => x.Fboid == fboId && !string.IsNullOrEmpty(x.Contact.Email))
                                .Select(f => new FboContactsViewModel
                                {
                                    ContactId = f.ContactId,
                                    FirstName = f.Contact.FirstName,
                                    LastName = f.Contact.LastName,
                                    Title = f.Contact.Title,
                                    Oid = f.Oid,
                                    Email = f.Contact.Email,
                                    Primary = f.Contact.Primary,
                                    CopyAlerts = f.Contact.CopyAlerts,
                                    CopyOrders = f.Contact.CopyOrders
                                })
                                .ToListAsync();
            //var responseFboContacts = await _apiClient.GetAsync("fbocontacts/fbo/" + fbo.Oid, conductorUser.Token);
            //var fboContacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FboContactsViewModel>>(responseFboContacts);

            foreach (FboContactsViewModel fboContact in fboContacts)
            {
                if (fboContact.CopyAlerts.GetValueOrDefault())
                    toEmails.Add(fboContact.Email);
            }

            return toEmails;
        }

        private async Task GenerateExpiredPricesEmail(List<string> toEmails, string fboName)
        {
            await _priceFetchingService.NotifyFboExpiredPrices(toEmails, fboName);
            //var requestObject = new NotifyFboNoRampFeesRequest();
            //requestObject.ToEmails = toEmails;
            //requestObject.FBO = fboName;
            //var result = await _apiClient.PostAsync("fboprices/notify-fbo-expired-prices", requestObject, token);
        }

        private async Task GenerateNoRampFeesEmail(List<string> toEmails, string fboName, string flightDepartment, string icao)
        {
            await _rampFeeService.NotifyFboNoRampFees(toEmails, fboName, flightDepartment, icao);
            //var requestObject = new NotifyFboNoRampFeesRequest();
            //requestObject.ToEmails = toEmails;
            //requestObject.FBO = fboName;
            //requestObject.CustomerName = flightDepartment;
            //requestObject.ICAO = icao;
            //var result = await _apiClient.PostAsync("rampfees/notify-fbo-no-rampfees", requestObject, token);
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