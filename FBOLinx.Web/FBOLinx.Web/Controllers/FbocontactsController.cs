﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Services;
using Fuelerlinx.SDK;
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FbocontactsController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly FuelerLinxApiService _fuelerLinxApiService;

        public FbocontactsController(FboLinxContext context, FuelerLinxApiService fuelerLinxApiService, ILoggingService logger) : base(logger)
        {
            _context = context;
            _fuelerLinxApiService = fuelerLinxApiService;
        }

        // GET: api/Fbocontacts/fbo/5
        [HttpGet("fbo/{fboId}")]
        public async Task<IActionResult> GetFbocontactsByFboId([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbocontacts = await GetAllFboContacts()
                                        .Include("Contact")
                                        .Where(x => x.Fboid == fboId && !string.IsNullOrEmpty(x.Contact.Email))
                                        .Select(f => new FboContactsViewModel
                                        {
                                            ContactId = f.ContactId,
                                            FirstName = f.Contact.FirstName,
                                            LastName = f.Contact.LastName,
                                            Title = f.Contact.Title,
                                            Oid = f.Oid,
                                            Email = f.Contact.Email,
                                            Primary = f.Contact.Primary,
                                            CopyAlerts = f.Contact.CopyAlerts,
                                            CopyOrders = f.Contact.CopyOrders
                                        })
                                        .ToListAsync();
            
            return Ok(fbocontacts);
        }

        // GET: api/Fbocontacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFbocontacts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbocontacts = await _context.Fbocontacts.FindAsync(id);

            if (fbocontacts == null)
            {
                return NotFound();
            }

            return Ok(fbocontacts);
        }

        // PUT: api/Fbocontacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFbocontacts([FromRoute] int id, [FromBody] Fbocontacts fbocontacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fbocontacts.Oid)
            {
                return BadRequest();
            }

            _context.Entry(fbocontacts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FbocontactsExists(id))
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

        // POST: api/Fbocontacts
        [HttpPost]
        public async Task<IActionResult> PostFbocontacts([FromBody] Fbocontacts fbocontacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Fbocontacts.Add(fbocontacts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFbocontacts", new { id = fbocontacts.Oid }, fbocontacts);
        }

        [HttpPost("fbo/{fboId}/newcontact")]
        public async Task<ActionResult<FboContactsViewModel>> AddNewFboContacts([FromRoute] int fboId, [FromBody] Contacts contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            var fbocontacts = new Fbocontacts
            {
                Fboid = fboId,
                ContactId = contact.Oid
            };
            _context.Fbocontacts.Add(fbocontacts);
            await _context.SaveChangesAsync();

            return Ok(new FboContactsViewModel
            {
                ContactId = fbocontacts.ContactId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Title = contact.Title,
                Oid = fbocontacts.Oid,
                Primary = contact.Primary,
                CopyAlerts = contact.CopyAlerts,
                CopyOrders = contact.CopyOrders
            });
        }

        [HttpPost("fbo/{fboId}/update-fuel-vendor")]
        public async Task<IActionResult> UpdateFuelVendor([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = _context.Fbos.Find(fboId);
            var emails = _context.Fbocontacts
                                    .Include("Contact")
                                    .Where(x => x.Fboid == fboId)
                                    .Where(c => !string.IsNullOrEmpty(c.Contact.Email))
                                    .Select(c => c.Contact.Email);
            var ccemail = string.Join("; ", emails);

            FBOLinxFuelVendorUpdateRequest request = new FBOLinxFuelVendorUpdateRequest
            {
                EmailToCC = ccemail,
                GroupId = fbo.GroupId,
                Email = fbo.FuelDeskEmail
            };

            var response = await _fuelerLinxApiService.UpdateFuelVendorEmails(request);

            return Ok(response);
        }

        // DELETE: api/Fbocontacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFbocontacts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbocontacts = await _context.Fbocontacts.FindAsync(id);
            if (fbocontacts == null)
            {
                return NotFound();
            }

            _context.Fbocontacts.Remove(fbocontacts);
            await _context.SaveChangesAsync();

            return Ok(fbocontacts);
        }

        private bool FbocontactsExists(int id)
        {
            return _context.Fbocontacts.Any(e => e.Oid == id);
        }

        private IQueryable<Fbocontacts> GetAllFboContacts()
        {
            return _context.Fbocontacts.AsQueryable();
        }
    }
}