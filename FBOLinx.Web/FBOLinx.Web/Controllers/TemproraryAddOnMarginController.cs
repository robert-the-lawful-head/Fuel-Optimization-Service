using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemproraryAddOnMarginController : ControllerBase
    {
        private readonly FboLinxContext _context;
        public TemproraryAddOnMarginController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/TemproraryAddOnMargi/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTempMargin([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var tempMargin = await _context.TempAddOnMargin.Where(x => x.FboId == id).Select(x=> new TempAddOnMargin {
                Id = x.Id,
                FboId = x.FboId,
                EffectiveFrom = x.EffectiveFrom.ToLocalTime(),
                EffectiveTo = x.EffectiveTo.ToLocalTime(),
                MarginAvgas = x.MarginAvgas,
                MarginJet = x.MarginJet
            }) .FirstOrDefaultAsync();

            return Ok(tempMargin);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemporaryMargin([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var temp = await _context.TempAddOnMargin.FindAsync(id);

            if (temp == null)
            {
                return NotFound();
            }

            _context.TempAddOnMargin.Remove(temp);
            await _context.SaveChangesAsync();

            return Ok(temp);
        }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> InsertTemporaryAddOnMargin([FromBody] TempAddOnMargin tempAddOnMargin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //tempAddOnMargin.EffectiveFrom.ToUniversalTime();
            //tempAddOnMargin.EffectiveTo.ToUniversalTime();

            _context.TempAddOnMargin.Add(tempAddOnMargin);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (tempMarginExists(tempAddOnMargin.FboId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTempMargin", new { id = tempAddOnMargin.Id }, tempAddOnMargin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> editTempMargin([FromRoute] int id, [FromBody] TempAddOnMargin tempAddOnMargin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tempAddOnMargin.Id)
            {
                return BadRequest();
            }

            _context.Entry(tempAddOnMargin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tempMarginExists(id))
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

        private bool tempMarginExists(int id)
        {
            return _context.TempAddOnMargin.Any(e => e.FboId == id);
        }

    }
}