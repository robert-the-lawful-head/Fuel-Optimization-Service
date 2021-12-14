using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using FBOLinx.Web.DTO;
using FBOLinx.Web.ViewModels;
using FBOLinx.Web.Models.Requests;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly FuelerLinxContext _fcontext;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public IServiceScopeFactory _serviceScopeFactory;
        private readonly AssociationsService _associationsService;

        public AssociationsController(FboLinxContext context, FuelerLinxContext fcontext, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory, AssociationsService associationsService)
        {
            _associationsService = associationsService;
            _context = context;
            _fcontext = fcontext;
            _HttpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }
                    
        // GET: api/Associations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssociation([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != JwtManager.GetClaimedGroupId(_HttpContextAccessor) && JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor)
                {
                    return BadRequest(ModelState);
                }

                var association = await _context.Associations.FindAsync(id);

                if (association == null)
                {
                    return NotFound();
                }

                return Ok(association);
            }
            catch (Exception ex)
            {
                return Ok("Get error: " + ex.Message);
            }

        }

        // PUT: api/Associations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssociation([FromRoute] int id, [FromBody] Associations association)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != JwtManager.GetClaimedGroupId(_HttpContextAccessor) && JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor)
            {
                return BadRequest(ModelState);
            }

            _context.Associations.Update(association);

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

        // POST: api/associations
        [HttpPost]
        public async Task<IActionResult> PostAssociation([FromBody] Associations association)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                association = await _associationsService.CreateNewAssociation(association.AssociationName);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

            return CreatedAtAction("GetAssociation", new { id = association.Oid }, association);
        }

        // DELETE: api/associations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssociation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var association = await _context.Associations.FindAsync(id);
            if (association == null)
            {
                return NotFound();
            }

            try
            {
                await _associationsService.DeleteAssociation(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(association);
        }
    }
}
