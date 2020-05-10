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

        public GroupsController(FboLinxContext context, FuelerLinxContext fcontext, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _fcontext = fcontext;
            _HttpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // GET: api/Groups
        [HttpGet]
        public IEnumerable<Group> GetGroup()
        {
            int groupId = UserService.GetClaimedGroupId(_HttpContextAccessor);
            Models.User.UserRoles role = UserService.GetClaimedRole(_HttpContextAccessor);
            return _context.Group.Where(x => !string.IsNullOrEmpty(x.GroupName) && (x.Oid == groupId || role == Models.User.UserRoles.Conductor)).OrderBy((x => x.GroupName));
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
                _context.Group.Add(group);
                await _context.SaveChangesAsync();

                if (group.Oid != 0)
                {
                    try
                    {
                        _fcontext.Database.ExecuteSqlCommand("exec up_Insert_FBOlinxGroupIntofuelerList @GroupName='" + group.GroupName + "', @GroupID=" + group.Oid + "");
                    }
                    catch(Exception ex)
                    {
                        return Ok("SP error: " + ex.Message);
                    }

                    try
                    {
                        //var listWithCustomers = _context.Customers.Where(s => s.FuelerlinxId > 0 && s.Company != null).ToList();

                        //foreach (var cust in listWithCustomers)
                        //{
                        //    CustomerInfoByGroup cibg = new CustomerInfoByGroup();
                        //    cibg.GroupId = group.Oid;
                        //    cibg.CustomerId = cust.Oid;
                        //    cibg.Company = cust.Company;
                        //    cibg.Username = cust.Username;
                        //    cibg.Password = cust.Password;
                        //    cibg.Joined = cust.Joined;
                        //    cibg.Active = cust.Active;
                        //    cibg.Distribute = cust.Distribute;
                        //    cibg.Network = cust.Network;
                        //    cibg.MainPhone = cust.MainPhone;
                        //    cibg.Address = cust.Address;
                        //    cibg.City = cust.City;
                        //    cibg.State = cust.State;
                        //    cibg.ZipCode = cust.ZipCode;
                        //    cibg.Country = cust.Country;
                        //    cibg.Website = cust.Website;
                        //    cibg.ShowJetA = cust.ShowJetA;
                        //    cibg.Show100Ll = cust.Show100Ll;
                        //    cibg.Suspended = cust.Suspended;

                        //    _context.CustomerInfoByGroup.Add(cibg);
                        //    _context.SaveChanges();
                        //}

                        var task = Task.Run(async () =>
                        {
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var db = scope.ServiceProvider.GetService<FboLinxContext>();
                                await GroupCustomersService.BeginCustomerAircraftsImport(db, group.Oid);
                            }

                        });


                    }
                    catch (Exception ex)
                    {
                        return Ok("Customer Aircraft add error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok("Global error: " + ex.Message);
            }

            return CreatedAtAction("GetGroup", new { id = group.Oid }, group);
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
                var groupsFboService = new GroupFboService(_context);
                await groupsFboService.DeleteGroup(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(@group);
        }
    }
}
