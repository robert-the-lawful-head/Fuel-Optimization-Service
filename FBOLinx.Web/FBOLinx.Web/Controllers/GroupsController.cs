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
        public async Task<IActionResult> PostGroup([FromBody] Group @group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //try
            //{
                
            //}
            //catch(Exception ex)
            //{
            //    var sss = ex.Message;
            //}

            _context.Group.Add(@group);
            await _context.SaveChangesAsync();

            _fcontext.Database.ExecuteSqlCommand("exec up_Insert_FBOlinxGroupIntofuelerList @GroupName='" + @group.GroupName + "', @GroupID=" + @group.Oid + "");

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