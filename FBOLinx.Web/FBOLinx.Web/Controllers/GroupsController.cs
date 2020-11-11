using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Remotion.Linq.Clauses.ResultOperators;
using FBOLinx.Web.DTO;
using FBOLinx.Web.ViewModels;
using FBOLinx.Web.Models.Requests;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly FuelerLinxContext _fcontext;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public IServiceScopeFactory _serviceScopeFactory;
        private readonly GroupFboService _groupFboService;
        private readonly CustomerService _customerService;

        public GroupsController(FboLinxContext context, FuelerLinxContext fcontext, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory, GroupFboService groupFboService, CustomerService customerService)
        {
            _groupFboService = groupFboService;
            _context = context;
            _fcontext = fcontext;
            _HttpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
            _customerService = customerService;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<IEnumerable<Group>> GetGroup()
        {
            int groupId = UserService.GetClaimedGroupId(_HttpContextAccessor);
            var role = UserService.GetClaimedRole(_HttpContextAccessor);

            return _context.Group
                        .Where(x => !string.IsNullOrEmpty(x.GroupName) && (x.Oid == groupId || role == Models.User.UserRoles.Conductor))
                        .Include(x => x.Users)
                        .OrderBy((x => x.GroupName));
        }

        [HttpGet("group-fbo")]
        public async Task<IActionResult> GetGroupsAndFbos()
        {
            var customersNeedAttention = await _customerService.GetNeedsAttentionCustomersCountByGroupFbo();

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
                                NeedAttentionCustomers = customersNeedAttention.Where(c => c.GroupId == x.Oid).Sum(c => c.CustomersNeedingAttention)
                            })
                            .ToListAsync();

            var products = Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes));

            var fboPrices = (from f in _context.Fboprices
                             where f.EffectiveTo > DateTime.UtcNow && f.Price != null && f.Expired != true
                             group f by f.Fboid into g
                             select new {
                                 fboId = g.Key,
                                 lastEffectiveDate = g.Max(t => t.EffectiveTo),
                                 product = g.Select(t => t.Product),
                                 expired = g.Select(t => t.Expired)
                             });

            var users = await _context.User.GroupBy(t => t.FboId).ToListAsync();

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
                                  PricingExpired = fprices == null,
                                  LastLogin = f.LastLogin
                              }).ToListAsync();

            fbos.ForEach(f =>
            {
                f.Users = users.Where(u => u.Key == f.Oid).SelectMany(u => u).ToList();
                f.NeedAttentionCustomers = customersNeedAttention.Where(c => c.FboId == f.Oid).Count();
            });

            return Ok(new GroupFboViewModel
            {
                Groups = groups,
                Fbos = fbos
            });
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        [UserRole(Models.User.UserRoles.Conductor, Models.User.UserRoles.GroupAdmin)]
        public async Task<IActionResult> GetGroup([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != UserService.GetClaimedGroupId(_HttpContextAccessor) && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.Conductor)
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
            catch(Exception ex)
            {
                return Ok("Get error: " + ex.Message);
            }
            
        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        [UserRole(new User.UserRoles[] {Models.User.UserRoles.Conductor, Models.User.UserRoles.GroupAdmin})]
        public async Task<IActionResult> PutGroup([FromRoute] int id, [FromBody] Group @group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != UserService.GetClaimedGroupId(_HttpContextAccessor) && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.Conductor)
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
        [UserRole(Models.User.UserRoles.Conductor)]
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
                return Ok(ex.Message);
            }

            return CreatedAtAction("GetGroup", new { id = group.Oid }, group);
        }

        [HttpPost("merge-groups")]
        [UserRole(Models.User.UserRoles.Conductor)]
        public async Task<IActionResult> MergeGroups([FromBody] MergeGroupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var changeableGroups = request.Groups.Where(group => group.Oid != request.BaseGroupId).Select(group => group.Oid).ToList();

                    var users = _context.User.Where(a => changeableGroups.Contains(a.GroupId.GetValueOrDefault())).ToList();
                    var fbos = _context.Fbos.Where(a => changeableGroups.Contains(a.GroupId.GetValueOrDefault())).ToList();
                    var adminEmails = _context.AdminEmails.Where(a => changeableGroups.Contains(a.GroupId)).ToList();
                    var companiesByGroups = _context.CompaniesByGroup.Where(a => changeableGroups.Contains(a.GroupId)).ToList();
                    var contactInfoByGroups = _context.ContactInfoByGroup.Where(a => changeableGroups.Contains(a.GroupId)).ToList();
                    var customerAircrafts = _context.CustomerAircrafts.Where(a => changeableGroups.Contains(a.GroupId.GetValueOrDefault())).ToList();
                    var customerAircraftViewedByGroups = _context.CustomerAircraftViewedByGroup.Where(a => changeableGroups.Contains(a.GroupId.GetValueOrDefault())).ToList();
                    var customerCompanyTypes = _context.CustomerCompanyTypes.Where(a => changeableGroups.Contains(a.GroupId)).ToList();
                    var customerInfoByGroups = _context.CustomerInfoByGroup.Where(a => changeableGroups.Contains(a.GroupId)).ToList();
                    var customerNotes = _context.CustomerNotes.Where(a => changeableGroups.Contains(a.GroupId)).ToList();
                    var customerSchedulingSoftwareByGroups = _context.CustomerSchedulingSoftwareByGroup.Where(a => changeableGroups.Contains(a.GroupId)).ToList();
                    var distributionLogs = _context.DistributionLog.Where(a => changeableGroups.Contains(a.GroupId.GetValueOrDefault())).ToList();
                    var distributionQueues = _context.DistributionQueue.Where(a => changeableGroups.Contains(a.GroupId)).ToList();

                    users.ForEach(a => a.GroupId = request.BaseGroupId);
                    fbos.ForEach(a => a.GroupId = request.BaseGroupId);
                    adminEmails.ForEach(a => a.GroupId = request.BaseGroupId);
                    companiesByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                    contactInfoByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                    customerAircrafts.ForEach(a => a.GroupId = request.BaseGroupId);
                    customerAircraftViewedByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                    customerCompanyTypes.ForEach(a => a.GroupId = request.BaseGroupId);
                    customerInfoByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                    customerNotes.ForEach(a => a.GroupId = request.BaseGroupId);

                    customerSchedulingSoftwareByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                    distributionLogs.ForEach(a => a.GroupId = request.BaseGroupId);
                    distributionQueues.ForEach(a => a.GroupId = request.BaseGroupId);

                    var baseCustomerInfoByGroups = _context.CustomerInfoByGroup
                        .Where(a => a.GroupId == request.BaseGroupId)
                        .Concat(customerInfoByGroups)
                        .ToList();
                    var distinctedByCustomerIDCustomerInfoByGroups = baseCustomerInfoByGroups
                        .GroupBy(cg => new { cg.CustomerId, cg.GroupId })
                        .Select(g => g.First())
                        .ToList();
                    var duplicatedByCustomerIDCustomerInfoByGroups = baseCustomerInfoByGroups
                        .Except(distinctedByCustomerIDCustomerInfoByGroups)
                        .ToList();
                    _context.CustomerInfoByGroup.RemoveRange(duplicatedByCustomerIDCustomerInfoByGroups);

                    var baseContactInfoByGroups = _context.ContactInfoByGroup
                        .Where(a => a.GroupId == request.BaseGroupId)
                        .Concat(contactInfoByGroups)
                        .ToList();
                    var distinctedContactInfoByGroups = baseContactInfoByGroups
                        .GroupBy(cg => new { cg.GroupId, cg.Email })
                        .Select(g => g.First())
                        .ToList();
                    var duplicatedContactInfoByGroups = baseContactInfoByGroups
                        .Except(distinctedContactInfoByGroups)
                        .ToList();
                    _context.ContactInfoByGroup.RemoveRange(duplicatedContactInfoByGroups);

                    var groupedByCustomerNameCustomerInfoByGroups = distinctedByCustomerIDCustomerInfoByGroups
                            .GroupBy(cg => new { Company = cg.Company.ToLower(), cg.GroupId })
                            .ToList();
                    groupedByCustomerNameCustomerInfoByGroups.ForEach(groupedResult =>
                    {
                        if (groupedResult.Count() > 0)
                        {
                            var customerIds = groupedResult
                                .Skip(1)
                                .Select(cg => cg.CustomerId)
                                .ToList();
                            var replacingCustomerContacts = _context.CustomerContacts
                                .Where(c => customerIds.Contains(c.CustomerId))
                                .ToList();
                            replacingCustomerContacts.ForEach(c => c.CustomerId = groupedResult.First().CustomerId);
                            _context.CustomerContacts.UpdateRange(replacingCustomerContacts);
                            _context.CustomerInfoByGroup.RemoveRange(groupedResult.Skip(1));
                        }
                    });

                    var removingGroups = _context.Group.Where(a => changeableGroups.Contains(a.Oid));
                    _context.Group.RemoveRange(removingGroups);

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }

            return Ok();
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        [UserRole(Models.User.UserRoles.Conductor)]
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
            catch(Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(@group);
        }
    }
}
