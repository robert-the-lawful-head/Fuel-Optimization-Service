﻿using System;
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
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerContactsController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;

        public CustomerContactsController(FboLinxContext context, ILoggingService logger) : base(logger)
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
        [HttpPost("{userId}")]
        public async Task<IActionResult> PostCustomerContacts([FromRoute] int userId ,[FromBody] CustomerContacts customerContacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerContacts.Add(customerContacts);
            await _context.SaveChangesAsync(userId);
           

            return CreatedAtAction("GetCustomerContacts", new { id = customerContacts.Oid }, customerContacts);
        }

        // DELETE: api/CustomerContacts/5
        [HttpDelete("{id}/{userId}")]
        public async Task<IActionResult> DeleteCustomerContacts([FromRoute] int id , [FromRoute] int userId)
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
            await _context.SaveChangesAsync(userId);

            return Ok(customerContacts);
        }

        // GET: api/CustomerContacts/group/5/fbo/6/pricingtemplate/7
        [HttpGet("group/{groupId}/fbo/{fboId}/pricingtemplate/{pricingTemplateId}")]
        public async Task<IActionResult> GetDistributionEmailsByTemplate([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int pricingTemplateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emails = await (from cg in _context.CustomerInfoByGroup.Where((x => x.GroupId == groupId))
                                join c in _context.Customers on cg.CustomerId equals c.Oid
                                join cc in _context.CustomCustomerTypes.Where(x => x.Fboid == fboId) on cg.CustomerId equals cc.CustomerId
                                join custc in _context.CustomerContacts on c.Oid equals custc.CustomerId
                                join co in _context.Contacts on custc.ContactId equals co.Oid
                                join cibg in _context.ContactInfoByGroup on co.Oid equals cibg.ContactId
                                join cibf in _context.Set<ContactInfoByFbo>() on new { ContactId = c.Oid, FboId = fboId } equals new { ContactId = cibf.ContactId.GetValueOrDefault(), FboId = cibf.FboId.GetValueOrDefault() } into leftJoinCIBF
                                from cibf in leftJoinCIBF.DefaultIfEmpty()
                                where (cg.Active ?? false)
                                      && (cc.CustomerType == pricingTemplateId || pricingTemplateId == 0)
                                      && ((cibf.ContactId != null && (cibf.CopyAlerts ?? false)) || (cibf.ContactId == null && (cibg.CopyAlerts ?? false)))
                                      && !string.IsNullOrEmpty(cibg.Email)
                                      && cibg.GroupId == groupId
                                      && (c.Suspended ?? false) == false
                                select cibg.Email
                                ).ToListAsync();

            return Ok(emails);
        }

            
        private bool CustomerContactsExists(int id)
        {
            return _context.CustomerContacts.Any(e => e.Oid == id);
        }
    }
}