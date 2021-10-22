using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ContactInfoByFboController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public ContactInfoByFboController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/ContactInfoByFbo
        [HttpGet]
        public IEnumerable<ContactInfoByFbo> GetContactInfoByFbo()
        {
            return _context.ContactInfoByFbo;
        }

        // GET: api/ContactInfoByFbo//contact/5/fbo/6
        [HttpGet("contact/{contactId}/fbo/{fboId}")]
        public async Task<IActionResult> GetContactInfoByContactIdFboId([FromRoute] int contactId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactInfoByFbo = await _context.ContactInfoByFbo.Where(c => c.ContactId == contactId && c.FboId == fboId).FirstOrDefaultAsync();

            if (contactInfoByFbo == null)
            {
                return NoContent();
            }

            return Ok(contactInfoByFbo);
        }

        // PUT: api/ContactInfoByFbo/5
        [HttpPut("contact/{oid}")]
        public async Task<IActionResult> PutContactInfoByFbo([FromRoute] int oid, [FromBody] ContactInfoByFbo contactInfoByFbo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (oid != contactInfoByFbo.Oid)
            {
                return BadRequest();
            }

            _context.Entry(contactInfoByFbo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }

            return NoContent();
        }

        // POST: api/ContactInfoByFbo
        [HttpPost]
        public async Task<IActionResult> PostContactInfoByFbo([FromBody] ContactInfoByFbo contactInfoByFbo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ContactInfoByFbo.Add(contactInfoByFbo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactInfoByFbo", new { id = contactInfoByFbo.Oid }, contactInfoByFbo);
        }

        // DELETE: api/ContactInfoByGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactInfoByFbo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactsInfoByFbo = await _context.ContactInfoByFbo.Where(c => c.ContactId == id).ToListAsync();
            if (contactsInfoByFbo == null || contactsInfoByFbo.Count == 0)
            {
                return NoContent();
            }

            _context.ContactInfoByFbo.RemoveRange(contactsInfoByFbo);
            await _context.SaveChangesAsync();

            return Ok(contactsInfoByFbo);
        }
    }
}

