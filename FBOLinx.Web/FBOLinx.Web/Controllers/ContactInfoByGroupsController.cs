using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoByGroupsController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public ContactInfoByGroupsController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/ContactInfoByGroups
        [HttpGet]
        public IEnumerable<ContactInfoByGroup> GetContactInfoByGroup()
        {
            return _context.ContactInfoByGroup;
        }

        // GET: api/ContactInfoByGroups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactInfoByGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactInfoByGroup = await _context.ContactInfoByGroup.FindAsync(id);

            if (contactInfoByGroup == null)
            {
                return NotFound();
            }

            return Ok(contactInfoByGroup);
        }

        // GET: api/ContactInfoByGroups/5
        [HttpGet("group/{groupId}/customer/{customerId}")]
        public async Task<IActionResult> GetCustomerContactInfoByGroup([FromRoute] int groupId, [FromRoute] int customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerContactInfoByGroupVM = (from cc in _context.CustomerContacts
                join c in _context.Contacts on cc.ContactId equals c.Oid
                join cibg in _context.ContactInfoByGroup on c.Oid equals cibg.ContactId
                where cibg.GroupId == groupId
                && cc.CustomerId == customerId
                select new CustomerContactsByGroupGridViewModel()
                {
                    ContactInfoByGroupId = cibg.Oid,
                    CustomerContactId = cc.Oid,
                    ContactId = c.Oid,
                    FirstName = cibg.FirstName,
                    LastName = cibg.LastName,
                    Title = cibg.Title,
                    Primary = cibg.Primary,
                    CopyAlerts = cibg.CopyAlerts
                });

            return Ok(customerContactInfoByGroupVM);
        }

        // PUT: api/ContactInfoByGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactInfoByGroup([FromRoute] int id, [FromBody] ContactInfoByGroup contactInfoByGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactInfoByGroup.Oid)
            {
                return BadRequest();
            }

            _context.Entry(contactInfoByGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactInfoByGroupExists(id))
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
        public async Task<IActionResult> DeleteContactInfoByGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactInfoByGroup = await _context.ContactInfoByGroup.FindAsync(id);
            if (contactInfoByGroup == null)
            {
                return NotFound();
            }

            _context.ContactInfoByGroup.Remove(contactInfoByGroup);
            await _context.SaveChangesAsync();

            return Ok(contactInfoByGroup);
        }

        private bool ContactInfoByGroupExists(int id)
        {
            return _context.ContactInfoByGroup.Any(e => e.Oid == id);
        }

        private IQueryable<ContactInfoByGroup> GetAllContactsInfoByGroup()
        {
            return _context.ContactInfoByGroup.AsQueryable();
        }
    }
}