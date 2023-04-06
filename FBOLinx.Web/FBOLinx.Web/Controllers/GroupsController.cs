using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.DTO.Requests.Groups;
using FBOLinx.Web.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using FBOLinx.Web.DTO;
using FBOLinx.Web.ViewModels;
using FBOLinx.Core.Enums;
using FBOLinx.ServiceLayer.DTO;
using System.Net;
using FBOLinx.ServiceLayer.BusinessServices.CompanyPricingLog;
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly FuelerLinxContext _fcontext;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public IServiceScopeFactory _serviceScopeFactory;
        private readonly GroupFboService _groupFboService;
        private readonly IGroupService _groupService;
        private readonly ICustomerService _customerService;
        private ICompanyPricingLogService _CompanyPricingLogService;


        public GroupsController(FboLinxContext context, FuelerLinxContext fcontext, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory, GroupFboService groupFboService, ICustomerService customerService, IGroupService groupService, ILoggingService logger,
            ICompanyPricingLogService companyPricingLogService) : base(logger)
        {
            _CompanyPricingLogService = companyPricingLogService;
            _groupFboService = groupFboService;
            _context = context;
            _fcontext = fcontext;
            _HttpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
            _customerService = customerService;
            _groupService = groupService;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<IEnumerable<Group>> GetGroup()
        {
            int groupId = JwtManager.GetClaimedGroupId(_HttpContextAccessor);
            var role = JwtManager.GetClaimedRole(_HttpContextAccessor);

            return await _context.Group
                        .Where(x => !string.IsNullOrEmpty(x.GroupName) && (x.Oid == groupId || role == UserRoles.Conductor))
                        .Include(x => x.Users)
                        .OrderBy((x => x.GroupName))
                        .ToListAsync();
        }

        [HttpGet("group-fbo")]
        public async Task<IActionResult> GetGroupsAndFbos()
        {
            await DisableExpiredAccounts();

            var customersNeedAttention = await _customerService.GetNeedsAttentionCustomersCountByGroupFbo();

            //var companyPricingLogs =
            //    await _CompanyPricingLogService.GetCompanyPricingLogCountLast30Days();

            var fuelReqs = await (from fr in _context.FuelReq
                                  join f in _context.Fbos on fr.Fboid equals f.Oid
                                  where (fr.Cancelled == null || fr.Cancelled == false) && fr.DateCreated >= DateTime.UtcNow.AddDays(-30) && f.Active == true
                                  select new
                                  {
                                      fr.Oid,
                                      fr.Fboid,
                                      f.GroupId
                                  }).ToListAsync();

            var groups = await _context.Group
                            .Where(x => !string.IsNullOrEmpty(x.GroupName))
                            .Include(x => x.Users)
                            .Include(x => x.Fbos)
                            .OrderBy((x => x.GroupName))
                            .Select(x => new GroupViewModel
                            {
                                Oid = x.Oid,
                                GroupName = x.GroupName,
                                Username = x.Username,
                                Password = x.Password,
                                Isfbonetwork = x.Isfbonetwork,
                                Domain = x.Domain,
                                LoggedInHomePage = x.LoggedInHomePage,
                                Active = x.Active,
                                IsLegacyAccount = x.IsLegacyAccount,
                                Users = x.Users,
                                LastLogin = x.Fbos.Max(f => f.LastLogin),
                            })
                            .ToListAsync();

            foreach (var group in groups)
            {
                var needingAttentions = customersNeedAttention.Where(c => c.GroupId == group.Oid).ToList();
                group.NeedAttentionCustomers = needingAttentions.Count > 0 ? needingAttentions.Sum(c => c.CustomersNeedingAttention) : 0;
                //group.Quotes30Days = (companyPricingLogs.Where(x => x.GroupId == group.Oid)?.Sum(x => x.QuoteCount)).GetValueOrDefault();
                group.Orders30Days = fuelReqs.Count(x => x.GroupId == group.Oid);
            }

            var fboPrices = from f in _context.Fboprices
                            where f.EffectiveFrom <= DateTime.UtcNow && f.EffectiveTo > DateTime.UtcNow && f.Price != null && f.Expired != true
                            group f by f.Fboid into g
                            select new
                            {
                                fboId = g.Key
                            };

            //FbosViewModel used to display FBO info in the grid
            var fbos = await (from f in _context.Fbos
                              join fa in _context.Fboairports on f.Oid equals fa.Fboid into fas
                              from fairports in fas.DefaultIfEmpty()
                              join fp in fboPrices on f.Oid equals fp.fboId into fps
                              from fprices in fps.DefaultIfEmpty()
                              select new FbosGridViewModel
                              {
                                  Active = f.Active,
                                  Fbo = f.Fbo,
                                  Icao = fairports.Icao,
                                  Oid = f.Oid,
                                  GroupId = f.GroupId ?? 0,
                                  PricingExpired = fprices.fboId == null,
                                  LastLogin = f.LastLogin,
                                  AccountExpired = f.Active != true
                              }).ToListAsync();

            var users = (await _context.User.ToListAsync()).GroupBy(t => t.FboId);
            fbos.ForEach(f =>
            {
                f.Users = users.Where(u => u.Key == f.Oid).SelectMany(u => u).ToList();
                f.NeedAttentionCustomers = customersNeedAttention.Where(c => c.FboId == f.Oid).Sum(c => c.CustomersNeedingAttention);
                //f.Quotes30Days = (companyPricingLogs.FirstOrDefault(x => x.FboId == f.Oid)?.QuoteCount).GetValueOrDefault();
                f.Orders30Days = fuelReqs.Count(fr => fr.Fboid == f.Oid);
            });

            groups.ForEach(g =>
            {
                var groupFbos = fbos.Where(f => f.GroupId == g.Oid).ToList();
                g.FboCount = groupFbos.Count();
                g.ActiveFboCount = groupFbos.Where(f => f.Active.GetValueOrDefault()).Count();
                g.ExpiredFboAccountCount = groupFbos.Count(f => f.AccountExpired == true);
                g.ExpiredFboPricingCount = groupFbos.Count(f => f.Active.GetValueOrDefault() && f.PricingExpired == true);
            });

            return Ok(new GroupFboViewModel
            {
                Groups = groups,
                Fbos = fbos
            });
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        [UserRole(UserRoles.Conductor, UserRoles.GroupAdmin)]
        public async Task<IActionResult> GetGroup([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != JwtManager.GetClaimedGroupId(_HttpContextAccessor) && JwtManager.GetClaimedRole(_HttpContextAccessor) != UserRoles.Conductor)
                {
                    return BadRequest(ModelState);
                }

                var @group = await _context.Group.FindAsync(id);

                if (@group == null)
                {
                    return NotFound();
                }

                return Ok(@group);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(ex);
                return Ok("Get error: " + ex.Message);
            }

        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        [UserRole(new UserRoles[] { UserRoles.Conductor, UserRoles.GroupAdmin })]
        public async Task<IActionResult> PutGroup([FromRoute] int id, [FromBody] Group @group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != JwtManager.GetClaimedGroupId(_HttpContextAccessor) && JwtManager.GetClaimedRole(_HttpContextAccessor) != UserRoles.Conductor)
            {
                return BadRequest(ModelState);
            }

            bool groupActiveStatus = await _context.Group.Where(g => g.Oid.Equals(group.Oid))
                                                         .Select(g => g.Active)
                                                         .FirstAsync();
            if (groupActiveStatus != group.Active)
            {
                List<User> users = _context.User.Where(u => u.GroupId.Equals(id))
                                            .ToList();
                foreach (var user in users)
                {
                    user.Active = group.Active;
                }

                _context.User.UpdateRange(users);

                List<Fbos> fbos = _context.Fbos.Where(f => f.GroupId.Equals(id)).ToList();

                foreach (var fbo in fbos)
                {
                    fbo.Active = group.Active;
                }

                _context.Fbos.UpdateRange(fbos);

                if (!group.Active)
                {
                    // Expire All Prices from the de-activated FBOLinx accounts
                    List<Fboprices> fboPrices = await (
                                        from fp in _context.Fboprices
                                        join f in _context.Fbos on fp.Fboid equals f.Oid
                                        where f.GroupId == id
                                        select fp
                                    ).ToListAsync();

                    foreach (var fboPrice in fboPrices)
                    {
                        fboPrice.Expired = true;
                    }

                    _context.Fboprices.UpdateRange(fboPrices);
                }
            }

            _context.Group.Update(group);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Group.Any(e => e.Oid == id))
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

        // POST: api/Groups
        [HttpPost]
        [UserRole(UserRoles.Conductor)]
        public async Task<IActionResult> PostGroup([FromBody] Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                group = await _groupFboService.CreateNewGroup(group.GroupName);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(ex);
                return Ok(ex.Message);
            }

            return CreatedAtAction("GetGroup", new { id = group.Oid }, group);
        }

        [HttpPost("merge-groups")]
        [UserRole(UserRoles.Conductor)]
        public async Task<IActionResult> MergeGroups([FromBody] MergeGroupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _groupService.MergeGroups(request);

            return Ok();
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        [UserRole(UserRoles.Conductor)]
        public async Task<IActionResult> DeleteGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }

            try
            {
                await _groupFboService.DeleteGroup(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(@group);
        }


        [HttpGet("group/{groupId}/logo")]
        public async Task<IActionResult> GetFboLogo([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var logoUrl = await _groupService.GetLogo(groupId);

                return Ok(new { Message = logoUrl });
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(ex);
                return Ok(new { Message = "" });
            }
        }

        [HttpPost("upload-logo")]
        public async Task<IActionResult> PostFboLogo([FromBody] GroupLogoRequest request)
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

                var logoUrl = await _groupService.UploadLogo(request);

                return Ok(new { Message = logoUrl });
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(ex);
                return Ok(new { Message = "" });
            }
        }

        private async Task DisableExpiredAccounts()
        {
            var expiredFbos = await _context.Fbos
                .Where(f => f.Active == true && f.ExpirationDate != null && f.ExpirationDate < DateTime.UtcNow)
                .ToListAsync();

            expiredFbos.ForEach(fbo =>
            {
                fbo.Active = false;
            });

            await _context.SaveChangesAsync();
        }

        [HttpDelete("group/{groupId}/logo")]
        public async Task<IActionResult> DeleteLogo([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _groupService.DeleteLogo(groupId);

            return Ok();
        }
    }
}