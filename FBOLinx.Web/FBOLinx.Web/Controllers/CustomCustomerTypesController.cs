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
using FBOLinx.Web.ViewModels;
using FBOLinx.Web.Models.Requests;
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomCustomerTypesController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;

        public CustomCustomerTypesController(FboLinxContext context, ILoggingService logger) : base(logger)
        {
            _context = context;
        }

        // GET: api/CustomCustomerTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomCustomerTypes>>> GetCustomCustomerTypes()
        {
            return await _context.CustomCustomerTypes.ToListAsync();
        }

        // GET: api/CustomCustomerTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomCustomerTypes>> GetCustomCustomerTypes(int id)
        {
            var customCustomerTypes = await _context.CustomCustomerTypes.FindAsync(id);

            if (customCustomerTypes == null)
            {
                return NotFound();
            }

            return customCustomerTypes;
        }

        // GET: api/CustomCustomerTypes/5
        [HttpGet("fbo/{fboId}/customer/{customerId}")]
        public async Task<IActionResult> GetCustomCustomerTypesForCustomer([FromRoute] int fboId, [FromRoute] int customerId)
        {
            var customCustomerTypes = await _context.CustomCustomerTypes.FirstOrDefaultAsync((x => x.CustomerId == customerId && x.Fboid == fboId));

            if (customCustomerTypes != null)
                return Ok(customCustomerTypes);

            //If no previous pricing template was attached, grab the default
            var defaultPricingTemplate = await _context.PricingTemplate
                .Where(x => x.Fboid == fboId && x.Default.HasValue && x.Default.Value).FirstOrDefaultAsync();
            
            if (defaultPricingTemplate == null)
                return null;
            
            var defaultCustomerType = new CustomCustomerTypes()
            {
                CustomerId = customerId,
                CustomerType = defaultPricingTemplate.Oid,
                Fboid = fboId
            };
            return Ok(defaultCustomerType);
        }

        [HttpPut("update")]
        public async Task<IActionResult> PutCustomCustomerTypesForCustomer( [FromBody] CustomCustomerTypeUpdateRequest request)
        {
            var customCustomerType = _context.CustomCustomerTypes
                                            .FirstOrDefault((x => x.CustomerId == request.CustomerId && x.Fboid == request.FboId));

            if (customCustomerType == null)
                return NotFound();

            customCustomerType.CustomerType = request.PricingTemplateId;
            _context.CustomCustomerTypes.Update(customCustomerType);
            
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/CustomCustomerTypes/5
        [HttpPut("{id}/{userId}")]
        public async Task<IActionResult> PutCustomCustomerTypes(int id , [FromRoute] int userId, CustomCustomerTypes customCustomerTypes)
        {
            if (id != customCustomerTypes.Oid)
            {
                return BadRequest();
            }           
            try
            {
                CustomCustomerTypes oldValue = await _context.CustomCustomerTypes.FirstOrDefaultAsync(c => c.Oid == customCustomerTypes.Oid);
                Fbos fbo = await _context.Fbos.Where(f => f.Oid == customCustomerTypes.Fboid).FirstOrDefaultAsync();

                if (oldValue != null)
                {
                    if (CompareCustoemrType(oldValue, customCustomerTypes) == false)
                    {
                        oldValue.CustomerId = customCustomerTypes.CustomerId;
                        oldValue.CustomerType = customCustomerTypes.CustomerType;
                        oldValue.Fboid = customCustomerTypes.Fboid;
                        _context.CustomCustomerTypes.Update(oldValue);
                        await _context.SaveChangesAsync(userId, customCustomerTypes.CustomerId, fbo.GroupId, customCustomerTypes.Fboid);
                    }
                }
               
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomCustomerTypesExists(id))
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


        // POST: api/CustomCustomerTypes
        [HttpPost]
        public async Task<ActionResult<CustomCustomerTypes>> PostCustomCustomerTypes(CustomCustomerTypes customCustomerTypes)
        {
            _context.CustomCustomerTypes.Add(customCustomerTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomCustomerTypes", new { id = customCustomerTypes.Oid }, customCustomerTypes);
        }


        [HttpPost("collection")]
        public async Task<ActionResult<List<CustomCustomerTypes>>> UpdateCustomCustomerTypeCollection(List<CustomCustomerTypes> customCustomerTypesCollection)
        {
            foreach (var customCustomerType in customCustomerTypesCollection)
            {
                if (customCustomerType.Oid > 0)
                    _context.CustomCustomerTypes.Update(customCustomerType);
                else
                    _context.CustomCustomerTypes.Add(customCustomerType);
            }
            await _context.SaveChangesAsync();
            return customCustomerTypesCollection;
        }


        // DELETE: api/CustomCustomerTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomCustomerTypes>> DeleteCustomCustomerTypes(int id)
        {
            var customCustomerTypes = await _context.CustomCustomerTypes.FindAsync(id);
            if (customCustomerTypes == null)
            {
                return NotFound();
            }

            _context.CustomCustomerTypes.Remove(customCustomerTypes);
            await _context.SaveChangesAsync();

            return customCustomerTypes;
        }


        private bool CustomCustomerTypesExists(int id)
        {
            return _context.CustomCustomerTypes.Any(e => e.Oid == id);
        }

        [HttpPost("updatedefaulttemplate")]
        public async Task<ActionResult<PricingTemplate>> UpdateCustomCustomerTypeCollection(UpdateTemplateVM updateTemplate)
        {
           if(updateTemplate != null)
            {
                var currentDefault = await _context.PricingTemplate.FirstOrDefaultAsync(s => s.Fboid == updateTemplate.fboid && s.Oid == updateTemplate.currenttemplate);

                var newDefault = await _context.PricingTemplate.FirstOrDefaultAsync(s => s.Fboid == updateTemplate.fboid && s.Oid == updateTemplate.newtemplate);

                if (newDefault != null)
                {
                    newDefault.Default = true;
                }


                if (currentDefault != null)
                {
                    if (updateTemplate.isDeleting)
                    {
                        _context.PricingTemplate.Remove(currentDefault);
                    }
                    else
                    {
                        currentDefault.Default = false;
                    }
                }

                if (updateTemplate.isDeleting)
                {
                    if (newDefault != null && currentDefault != null)
                    {
                        var customers = await _context.CustomCustomerTypes
                        .Where(c => c.Fboid.Equals(updateTemplate.fboid) && c.CustomerType.Equals(updateTemplate.currenttemplate))
                        .Select(s => s.CustomerId)
                        .ToListAsync();

                        var groupInfo = await _context.Fbos.FirstOrDefaultAsync(s => s.Oid == updateTemplate.fboid);
                        var customerInfoByGroup = await _context.CustomerInfoByGroup.Where(s => customers.Contains(s.CustomerId) && s.GroupId == groupInfo.GroupId).ToListAsync();

                        customerInfoByGroup.ForEach(s => s.PricingTemplateRemoved = true);

                        var customCustomerTypes = await _context.CustomCustomerTypes
                            .Where(c => c.Fboid.Equals(updateTemplate.fboid) && c.CustomerType.Equals(updateTemplate.currenttemplate))
                            .ToListAsync();

                        customCustomerTypes.ForEach(c => c.CustomerType = newDefault.Oid);
                    }
                }
                
                _context.PricingTemplate.Update(newDefault);

                await _context.SaveChangesAsync();

                return newDefault;
            }

            return null;
        }

        #region Private Methods 
         private bool CompareCustoemrType (CustomCustomerTypes oldValue , CustomCustomerTypes newValue)
        {
            return oldValue.CustomerId == newValue.CustomerId &&
                   oldValue.CustomerType == newValue.CustomerType &&
                   oldValue.Fboid == newValue.Fboid;
               
        }
        #endregion
    }
}
