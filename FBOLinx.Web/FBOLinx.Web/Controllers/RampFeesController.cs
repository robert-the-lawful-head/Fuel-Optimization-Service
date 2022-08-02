using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.ViewModels;
using static FBOLinx.DB.Models.RampFees;
using System.Text.RegularExpressions;
using System.Globalization;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.Web.Services;
using FBOLinx.ServiceLayer.BusinessServices.Mail;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RampFeesController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly RampFeesService _RampFeesService;
        private readonly AircraftService _aircraftService;
        private IMailService _MailService;

        public RampFeesController(FboLinxContext context, RampFeesService rampFeesService, AircraftService aircraftService, IMailService mailService)
        {
            _context = context;
            _RampFeesService = rampFeesService;
            _aircraftService = aircraftService;
            _MailService = mailService;
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
            RampFees rampFees = await _context.RampFees.FindAsync(id);

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
            var result = await _RampFeesService.GetRampFeesByFbo(fboId);

            return Ok(result);
        }

        [HttpGet("fbo/{fboId}/tail/{tailNumber}")]
        public async Task<ActionResult<RampFees>> GetRampFeeForAircraft([FromRoute] int fboId, [FromRoute] string tailNumber)
        {
            var result = await _RampFeesService.GetRampFeeForAircraft(fboId, tailNumber);

            return Ok(result);
        }

        // PUT: api/RampFees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRampFees(int id, RampFees rampFees)
        {
            if (id != rampFees.Oid)
            {
                return BadRequest();
            }

            rampFees.LastUpdated = DateTime.Now;

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
            rampFees.LastUpdated = DateTime.Now; 
            _context.RampFees.Add(rampFees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRampFees", new { id = rampFees.Oid }, rampFees);
        }

        // DELETE: api/RampFees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RampFees>> DeleteRampFees(int id)
        {
            RampFees rampFees = await _context.RampFees.FindAsync(id);
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

            try
            {
                foreach (var rampfee in rampfees)
                {
                    Double rampFeeValue = 0.0;
                    if (rampfee.tailnumber != null && rampfee.fboid != 0)
                    {
                        RampFees newRampFee = new RampFees();
                        newRampFee.Fboid = rampfee.fboid;
                        newRampFee.CategoryType = RampFeeCategories.TailNumber;
                        newRampFee.CategoryStringValue = rampfee.tailnumber;

                        Double waivedFeeValue = 0.0;
                        if (rampfee.waivedat.ToString().Equals(@"N/A"))
                        {
                            newRampFee.Waived = null;
                        }
                        else
                        {
                            string waivedFee = Regex.Match(rampfee.waivedat.ToString(), @"\d+").Value;

                            Double.TryParse(waivedFee, out waivedFeeValue);
                            newRampFee.Waived = waivedFeeValue;
                        }

                        var results = Regex.Matches(rampfee.rampfee, @"[-+]?(?<![0-9]\.)\b[0-9]+(?:[,\s][0-9]+)*\.[0-9]+(?:[eE][-+]?[0-9]+)?\b(?!\.[0-9])")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .ToList();

                        if (results.Count.Equals(0))
                        {
                            string ttrampFeeValue = Regex.Match(rampfee.rampfee, @"\d+").Value;
                            Double.TryParse(ttrampFeeValue, out rampFeeValue);
                            newRampFee.Price = rampFeeValue;
                        }
                        else
                        {
                            newRampFee.Price = Convert.ToDouble(results[0]);
                        }

                        var existingTailRampFee = _context.RampFees.FirstOrDefault(s => s.Fboid == newRampFee.Fboid && s.CategoryStringValue == newRampFee.CategoryStringValue);

                        if(existingTailRampFee != null)
                        {
                            existingTailRampFee.Price = newRampFee.Price;
                            existingTailRampFee.Waived = newRampFee.Waived;
                            existingTailRampFee.LastUpdated = DateTime.Now;
                            _context.RampFees.Update(existingTailRampFee);
                        }
                        else
                        {
                            newRampFee.LastUpdated = DateTime.Now;
                            _context.RampFees.Add(newRampFee);
                        }

                        _context.SaveChanges();

                    }
                    else if (rampfee.aircraftsize != null && rampfee.fboid != 0)
                    {
                        RampFees newRampFee = new RampFees();
                        newRampFee.Fboid = rampfee.fboid;

                        switch (rampfee.aircraftsize)
                        {
                            case "Very Light Jet":
                                newRampFee.Size = AircraftSizes.VeryLightJet;
                                break;
                            case "VLJ's":
                                newRampFee.Size = AircraftSizes.VeryLightJet;
                                break;
                            case "VLJ":
                                newRampFee.Size = AircraftSizes.VeryLightJet;
                                break;
                            case "VeryLightJet":
                                newRampFee.Size = AircraftSizes.VeryLightJet;
                                break;
                            case "Light Jets":
                                newRampFee.Size = AircraftSizes.LightJet;
                                break;
                            case "Light Jet":
                                newRampFee.Size = AircraftSizes.LightJet;
                                break;
                            case "LightJets":
                                newRampFee.Size = AircraftSizes.LightJet;
                                break;
                            case "LightJet":
                                newRampFee.Size = AircraftSizes.LightJet;
                                break;
                            case "Mid Jets":
                                newRampFee.Size = AircraftSizes.MediumJet;
                                break;
                            case "Medium Jets":
                                newRampFee.Size = AircraftSizes.MediumJet;
                                break;
                            case "MediumJets":
                                newRampFee.Size = AircraftSizes.MediumJet;
                                break;
                            case "MediumJet":
                                newRampFee.Size = AircraftSizes.MediumJet;
                                break;
                            case "Medium Jet":
                                newRampFee.Size = AircraftSizes.MediumJet;
                                break;
                            case "Heavy Jet":
                                newRampFee.Size = AircraftSizes.HeavyJet;
                                break;
                            case "Heavy Jets":
                                newRampFee.Size = AircraftSizes.HeavyJet;
                                break;
                            case "HeavyJet":
                                newRampFee.Size = AircraftSizes.HeavyJet;
                                break;
                            case "HeavyJets":
                                newRampFee.Size = AircraftSizes.HeavyJet;
                                break;
                            case "Super Heavy Jet":
                                newRampFee.Size = AircraftSizes.SuperHeavyJet;
                                break;
                            case "Super Heavy Jets":
                                newRampFee.Size = AircraftSizes.SuperHeavyJet;
                                break;
                            case "SHJ":
                                newRampFee.Size = AircraftSizes.SuperHeavyJet;
                                break;
                            case "Wide Body":
                                newRampFee.Size = AircraftSizes.WideBody;
                                break;
                            case "Wide Bodies":
                                newRampFee.Size = AircraftSizes.WideBody;
                                break;
                            case "Light Helicopter":
                                newRampFee.Size = AircraftSizes.LightHelicopter;
                                break;
                            case "Medium Helicopter":
                                newRampFee.Size = AircraftSizes.MediumHelicopter;
                                break;
                            case "Heavy Helicopter":
                                newRampFee.Size = AircraftSizes.HeavyHelicopter;
                                break;
                            case "Light Twin":
                                newRampFee.Size = AircraftSizes.LightTwin;
                                break;
                            case "Heavy Turbo Prop":
                                newRampFee.Size = AircraftSizes.HeavyTurboprop;
                                break;
                            case "Heavy TurboProp":
                                newRampFee.Size = AircraftSizes.HeavyTurboprop;
                                break;
                            case "Medium TurboProp":
                                newRampFee.Size = AircraftSizes.MediumTurboprop;
                                break;
                            case "Medium Turbo Prop":
                                newRampFee.Size = AircraftSizes.MediumTurboprop;
                                break;
                            case "Single Engine Piston":
                                newRampFee.Size = AircraftSizes.SingleEnginePiston;
                                break;
                            case "SingleEnginePiston":
                                newRampFee.Size = AircraftSizes.SingleEnginePiston;
                                break;
                            case "SingleTurboProp":
                                newRampFee.Size = AircraftSizes.SingleTurboProp;
                                break;
                            case "Single Turbo Prop":
                                newRampFee.Size = AircraftSizes.SingleTurboProp;
                                break;
                            default:
                                newRampFee.Size = AircraftSizes.NotSet;
                                break;
                        }



                        var results = Regex.Matches(rampfee.rampfee, @"[-+]?(?<![0-9]\.)\b[0-9]+(?:[,\s][0-9]+)*\.[0-9]+(?:[eE][-+]?[0-9]+)?\b(?!\.[0-9])")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .ToList();

                        if (results.Count.Equals(0))
                        {
                            string ttrampFeeValue = Regex.Match(rampfee.rampfee, @"\d+").Value;
                            if (ttrampFeeValue != string.Empty)
                            {
                                Double.TryParse(ttrampFeeValue, out rampFeeValue);
                                newRampFee.Price = rampFeeValue;
                            }
                            else
                            {
                                newRampFee.Price = null;
                            }

                        }
                        else
                        {
                            newRampFee.Price = Convert.ToDouble(results[0]);
                        }

                        Double waivedFeeValue = 0.0;
                        if (rampfee.avoidance.ToString().Equals(@"N/A"))
                        {
                            newRampFee.Waived = null;
                        }
                        else
                        {
                            string waivedFee = Regex.Match(rampfee.avoidance.ToString(), @"\d+").Value;
                            
                            Double.TryParse(waivedFee, out waivedFeeValue);
                            newRampFee.Waived = waivedFeeValue;
                        }

                        var checkForExisting = _context.RampFees.FirstOrDefault(s => s.Fboid == newRampFee.Fboid && s.Size == newRampFee.Size);

                        if(checkForExisting != null)
                        {
                            checkForExisting.Waived = newRampFee.Waived;
                            checkForExisting.Price = newRampFee.Price;
                            checkForExisting.LastUpdated = DateTime.Now;
                            _context.RampFees.Update(checkForExisting);
                        }
                        else
                        {
                            newRampFee.LastUpdated = DateTime.Now;
                            _context.RampFees.Add(newRampFee);
                        }

                        _context.SaveChanges();
                    }
                }

                return Ok(rampfees);
            }
            catch
            {
                return Ok(null);
            }
        }
    }
}
