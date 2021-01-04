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
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PriceTiersController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public PriceTiersController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/PriceTiers
        [HttpGet]
        public IEnumerable<PriceTiers> GetPriceTiers()
        {
            return _context.PriceTiers;
        }

        // GET: api/PriceTiers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPriceTiers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var priceTiers = await _context.PriceTiers.FindAsync(id);

            if (priceTiers == null)
            {
                return NotFound();
            }

            return Ok(priceTiers);
        }

        // PUT: api/PriceTiers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPriceTiers([FromRoute] int id, [FromBody] PriceTiers priceTiers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceTiers.Oid)
            {
                return BadRequest();
            }

            _context.Entry(priceTiers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceTiersExists(id))
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

        // POST: api/PriceTiers
        [HttpPost]
        public async Task<IActionResult> PostPriceTiers([FromBody] PriceTiers priceTiers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PriceTiers.Add(priceTiers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPriceTiers", new { id = priceTiers.Oid }, priceTiers);
        }

        // Post: api/PriceTiers/CustomerMargins
        [HttpPost("CustomerMargins")]
        public async Task<IActionResult> PostPriceTiersFromCustomerMarginsViewModel([FromBody] List<CustomerMarginsGridViewModel> customerMargins)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int indexLoopPriceTier = 0;

            foreach (CustomerMarginsGridViewModel customerMarginsGridViewModel in customerMargins)
            {
                if((indexLoopPriceTier + 1) < customerMargins.Count)
                {
                    var nextPriceTier = customerMargins[indexLoopPriceTier + 1];
                    if (nextPriceTier != null)
                    {
                        customerMarginsGridViewModel.Max = nextPriceTier.Min - 1;
                        indexLoopPriceTier++;
                    }
                }
                else
                {
                    customerMarginsGridViewModel.Max = 99999;
                }


                PriceTiers priceTier = new PriceTiers()
                {
                    Max = customerMarginsGridViewModel.Max,
                    Min = customerMarginsGridViewModel.Min,
                    MaxEntered = customerMarginsGridViewModel.MaxEntered,
                    Oid = customerMarginsGridViewModel.PriceTierId
                };
                if (priceTier.Oid > 0)
                    _context.PriceTiers.Update(priceTier);
                else
                    _context.PriceTiers.Add(priceTier);
                await _context.SaveChangesAsync();
                customerMarginsGridViewModel.PriceTierId = priceTier.Oid;

                CustomerMargins customerMargin = new CustomerMargins()
                {
                    Amount = customerMarginsGridViewModel.Amount,
                    Oid = customerMarginsGridViewModel.Oid,
                    TemplateId = customerMarginsGridViewModel.TemplateId,
                    PriceTierId = customerMarginsGridViewModel.PriceTierId
                };

                if (customerMargin.Oid > 0)
                    _context.CustomerMargins.Update(customerMargin);
                else
                    _context.CustomerMargins.Add(customerMargin);

                await _context.SaveChangesAsync();

                customerMarginsGridViewModel.Oid = customerMargin.Oid;
            }

            return Ok(customerMargins);
        }

        // DELETE: api/PriceTiers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriceTiers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var priceTiers = await _context.PriceTiers.FindAsync(id);
            if (priceTiers == null)
            {
                return NotFound();
            }

            _context.PriceTiers.Remove(priceTiers);
            await _context.SaveChangesAsync();

            return Ok(priceTiers);
        }

        private bool PriceTiersExists(int id)
        {
            return _context.PriceTiers.Any(e => e.Oid == id);
        }
    }
}