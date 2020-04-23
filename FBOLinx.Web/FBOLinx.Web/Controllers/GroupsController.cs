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

        public GroupsController(FboLinxContext context, FuelerLinxContext fcontext, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _fcontext = fcontext;
            _HttpContextAccessor = httpContextAccessor;
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

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        [UserRole(new User.UserRoles[] {Models.User.UserRoles.Conductor, Models.User.UserRoles.GroupAdmin})]
        public async Task<IActionResult> PutGroup([FromRoute] int id, [FromBody] Group @group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @group.Oid)
            {
                return BadRequest();
            }

            if (id != UserService.GetClaimedGroupId(_HttpContextAccessor) && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.Conductor)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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
                    _fcontext.Database.ExecuteSqlCommand("exec up_Insert_FBOlinxGroupIntofuelerList @GroupName='" + group.GroupName + "', @GroupID=" + group.Oid + "");

                    var listWithCustomers = _context.Customers.Where(s => s.FuelerlinxId > 0 && s.Company !=null).ToList();

                    foreach (var cust in listWithCustomers)
                    {
                        CustomerInfoByGroup cibg = new CustomerInfoByGroup();
                        cibg.GroupId = group.Oid;
                        cibg.CustomerId = cust.Oid;
                        cibg.Company = cust.Company;
                        cibg.Username = cust.Username;
                        cibg.Password = cust.Password;
                        cibg.Joined = cust.Joined;
                        cibg.Active = cust.Active;
                        cibg.Distribute = cust.Distribute;
                        cibg.Network = cust.Network;
                        cibg.MainPhone = cust.MainPhone;
                        cibg.Address = cust.Address;
                        cibg.City = cust.City;
                        cibg.State = cust.State;
                        cibg.ZipCode = cust.ZipCode;
                        cibg.Country = cust.Country;
                        cibg.Website = cust.Website;
                        cibg.ShowJetA = cust.ShowJetA;
                        cibg.Show100Ll = cust.Show100Ll;
                        cibg.Suspended = cust.Suspended;

                        _context.CustomerInfoByGroup.Add(cibg);
                        _context.SaveChanges();

                        var listOfAirplanes = _context.CustomerAircrafts.Where(s => s.CustomerId == cust.Oid).GroupBy(s => s.AircraftId).ToList();

                        foreach (var airplane in listOfAirplanes)
                        {
                            var singleAirplane = airplane;
                            CustomerAircrafts ca = new CustomerAircrafts();
                            ca.AircraftId = airplane.Key;
                            ca.CustomerId = cust.Oid;
                            ca.GroupId = group.Oid;
                            ca.TailNumber = airplane.First().TailNumber;
                            ca.Size = airplane.First().Size;
                            ca.NetworkCode = airplane.First().NetworkCode;
                            ca.AddedFrom = airplane.First().AddedFrom;

                            _context.CustomerAircrafts.Add(ca);
                            _context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var sss = ex.Message;
                return Ok(null);
            }

            return CreatedAtAction("GetGroup", new { id = @group.Oid }, @group);
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

            _context.Group.Remove(@group);
            await _context.SaveChangesAsync();

            return Ok(@group);
        }

        private bool GroupExists(int id)
        {
            return _context.Group.Any(e => e.Oid == id);
        }

        private void InsertNewGroupInfo()
        {

        }
    }
}