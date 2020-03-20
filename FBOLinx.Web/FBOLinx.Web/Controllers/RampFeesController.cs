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
using FBOLinx.Web.ViewModels;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RampFeesController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public RampFeesController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/RampFees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RampFees>>> GetRampFees()
        {
            return await _context.RampFees.ToListAsync();
        }

        // GET: api/RampFees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RampFees>> GetRampFees(int id)
        {
            var rampFees = await _context.RampFees.FindAsync(id);

            if (rampFees == null)
            {
                return NotFound();
            }

            return rampFees;
        }

        // GET: api/RampFees/fbo/5
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<IEnumerable<RampFees>>> GetRampFeesForFbo([FromRoute] int fboId)
        {
            try
            {
                //Grab all of the aircraft sizes and return a record for each size, even if the FBO hasn't customized them
                var sizes = FBOLinx.Web.Utilities.Enum.GetDescriptions(typeof(Models.AirCrafts.AircraftSizes));
                var result = (from s in sizes
                    join r in _context.RampFees on new
                        {
                            size = (int?) ((short?) ((FBOLinx.Web.Models.AirCrafts.AircraftSizes) s.Value)),
                            fboId = (int?) fboId
                        }
                        equals new
                        {
                            size = r.CategoryMinValue,
                            fboId = r.Fboid
                        }
                        into leftJoinRampFees
                    from r in leftJoinRampFees.DefaultIfEmpty()
                              select new ViewModels.RampFeesGridViewModel()
                    {
                        Oid = r?.Oid ?? 0,
                        Price = r?.Price,
                        Waived = r?.Waived,
                        Fboid = r?.Fboid,
                        CategoryType = r?.CategoryType,
                        CategoryMinValue = r?.CategoryMinValue,
                        CategoryMaxValue = r?.CategoryMaxValue,
                        ExpirationDate = r?.ExpirationDate,
                        Size = (FBOLinx.Web.Models.AirCrafts.AircraftSizes)s.Value,
                        AppliesTo = (from a in _context.Aircrafts
                                     where a.Size.HasValue && a.Size == (FBOLinx.Web.Models.AirCrafts.AircraftSizes)s.Value
                                     select a).OrderBy((x => x.Make)).ThenBy((x => x.Model)).ToList()

                    }).ToList();

                //Pull additional "custom" ramp fees (weight, tail, wingspan, etc.)
                var customRampFees = await (from r in _context.RampFees
                    join a in _context.Aircrafts on r.CategoryMinValue equals (a.AircraftId) into leftJoinAircrafts
                    from a in leftJoinAircrafts.DefaultIfEmpty()
                                            where r.Fboid == fboId && r.CategoryType.HasValue &&
                          r.CategoryType.Value != RampFees.RampFeeCategories.AircraftSize
                    select new ViewModels.RampFeesGridViewModel()
                    {
                        Oid = r.Oid,
                        Price = r.Price,
                        Waived = r.Waived,
                        Fboid = r.Fboid,
                        CategoryType = r.CategoryType,
                        CategoryMinValue = r.CategoryMinValue,
                        CategoryMaxValue = r.CategoryMaxValue,
                        ExpirationDate = r.ExpirationDate,
                        AircraftMake = a == null ? "" : a.Make,
                        AircraftModel = a == null ? "" : a.Model

                    }).ToListAsync();

                result.AddRange(customRampFees);
                return Ok(result);
            }
            catch (System.Exception exception)
            {
                var test = exception;
                return null;
            }
        }

        // PUT: api/RampFees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRampFees(int id, RampFees rampFees)
        {
            if (id != rampFees.Oid)
            {
                return BadRequest();
            }

            _context.Entry(rampFees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RampFeesExists(id))
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

        // POST: api/RampFees
        [HttpPost]
        public async Task<ActionResult<RampFees>> PostRampFees(RampFees rampFees)
        {
            _context.RampFees.Add(rampFees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRampFees", new { id = rampFees.Oid }, rampFees);
        }

        // DELETE: api/RampFees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RampFees>> DeleteRampFees(int id)
        {
            var rampFees = await _context.RampFees.FindAsync(id);
            if (rampFees == null)
            {
                return NotFound();
            }

            _context.RampFees.Remove(rampFees);
            await _context.SaveChangesAsync();

            return rampFees;
        }

        private bool RampFeesExists(int id)
        {
            return _context.RampFees.Any(e => e.Oid == id);
        }

        [HttpPost("importrampfees")]
        public async Task<IActionResult> ImportRampFees([FromBody] List<RampFeesImportVM> rampfees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            foreach (var rampfee in rampfees)
            {
                Customers newC = new Customers();
                
              
            }

            return Ok(null);
        }
    }
}
