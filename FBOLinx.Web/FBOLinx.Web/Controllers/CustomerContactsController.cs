using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CustomerContactsController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public CustomerContactsController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/CustomerContacts
        [HttpGet]
        public IEnumerable<CustomerContacts> GetCustomerContacts()
        {
            return _context.CustomerContacts;
        }

        // GET: api/CustomerContacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerContacts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerContacts = await _context.CustomerContacts.FindAsync(id);

            if (customerContacts == null)
            {
                return NotFound();
            }

            return Ok(customerContacts);
        }

        // PUT: api/CustomerContacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerContacts([FromRoute] int id, [FromBody] CustomerContacts customerContacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerContacts.Oid)
            {
                return BadRequest();
            }

            _context.Entry(customerContacts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerContactsExists(id))
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

        // POST: api/CustomerContacts
        [HttpPost]
        public async Task<IActionResult> PostCustomerContacts([FromBody] CustomerContacts customerContacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerContacts.Add(customerContacts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerContacts", new { id = customerContacts.Oid }, customerContacts);
        }

        // DELETE: api/CustomerContacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerContacts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerContacts = await _context.CustomerContacts.FindAsync(id);
            if (customerContacts == null)
            {
                return NotFound();
            }

            _context.CustomerContacts.Remove(customerContacts);
            await _context.SaveChangesAsync();

            return Ok(customerContacts);
        }

        private bool CustomerContactsExists(int id)
        {
            return _context.CustomerContacts.Any(e => e.Oid == id);
        }
    }
}