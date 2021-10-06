using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{
    public class ContactInfoByFboController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public ContactInfoByFboController(FboLinxContext context)
        {
            _context = context;
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
                return NotFound();
            }

            return Ok(contactInfoByFbo);
        }

        //// PUT: api/ContactInfoByFbo/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutContactInfoByFbo([FromRoute] int id, [FromBody] ContactInfoByFbo contactInfoByFbo)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != contactInfoByFbo.Oid)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(contactInfoByFbo).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ContactInfoByFboExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/ContactInfoByGroups
        [HttpPost]
        public async Task<IActionResult> PostContactInfoByGroup([FromBody] ContactInfoByGroup contactInfoByGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ContactInfoByGroup.Add(contactInfoByGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactInfoByGroup", new { id = contactInfoByGroup.Oid }, contactInfoByGroup);
        }

        // DELETE: api/ContactInfoByGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactInfoByFbo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactInfoByFbo = await _context.ContactInfoByFbo.FindAsync(id);
            if (contactInfoByFbo == null)
            {
                return NotFound();
            }

            _context.ContactInfoByFbo.Remove(contactInfoByFbo);
            await _context.SaveChangesAsync();

            return Ok(contactInfoByFbo);
        }

        //private bool ContactInfoByFboExists(int id)
        //{
        //    return _context.ContactInfoByFbo.Any(e => e.Oid == id);
        //}
    }
}

