using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesByGroupsController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public CompaniesByGroupsController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/CompaniesByGroups
        [HttpGet]
        public IEnumerable<CompaniesByGroup> GetCompaniesByGroup()
        {
            return _context.CompaniesByGroup;
        }

        // GET: api/CompaniesByGroups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompaniesByGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var companiesByGroup = await _context.CompaniesByGroup.FindAsync(id);

            if (companiesByGroup == null)
            {
                return NotFound();
            }

            return Ok(companiesByGroup);
        }

        // PUT: api/CompaniesByGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompaniesByGroup([FromRoute] int id, [FromBody] CompaniesByGroup companiesByGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != companiesByGroup.Oid)
            {
                return BadRequest();
            }

            _context.Entry(companiesByGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompaniesByGroupExists(id))
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

        // POST: api/CompaniesByGroups
        [HttpPost]
        public async Task<IActionResult> PostCompaniesByGroup([FromBody] CompaniesByGroup companiesByGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CompaniesByGroup.Add(companiesByGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompaniesByGroup", new { id = companiesByGroup.Oid }, companiesByGroup);
        }

        // DELETE: api/CompaniesByGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompaniesByGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var companiesByGroup = await _context.CompaniesByGroup.FindAsync(id);
            if (companiesByGroup == null)
            {
                return NotFound();
            }

            _context.CompaniesByGroup.Remove(companiesByGroup);
            await _context.SaveChangesAsync();

            return Ok(companiesByGroup);
        }

        private bool CompaniesByGroupExists(int id)
        {
            return _context.CompaniesByGroup.Any(e => e.Oid == id);
        }
    }
}