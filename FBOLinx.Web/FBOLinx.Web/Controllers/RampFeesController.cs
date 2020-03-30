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
using static FBOLinx.Web.Models.RampFees;
using System.Text.RegularExpressions;
using System.Globalization;

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
            try
            {
                //Grab all of the aircraft sizes and return a record for each size, even if the FBO hasn't customized them
                IEnumerable<Utilities.Enum.EnumDescriptionValue> sizes = Utilities.Enum.GetDescriptions(typeof(AirCrafts.AircraftSizes));
                List<ViewModels.RampFeesGridViewModel> result = (
                    from s in sizes
                    join r in _context.RampFees on new
                        {
                            size = (int?) ((short?) ((AirCrafts.AircraftSizes) s.Value)),
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
                        Size = (AirCrafts.AircraftSizes)s.Value,
                        AppliesTo = (from a in _context.Aircrafts
                                     where a.Size.HasValue && a.Size == (AirCrafts.AircraftSizes)s.Value
                                     select a).OrderBy((x => x.Make)).ThenBy((x => x.Model)).ToList()

                    }).ToList();

                // Pull additional "custom" ramp fees(weight, tail, wingspan, etc.)
                List<ViewModels.RampFeesGridViewModel> customRampFees = await (from r in _context.RampFees
                                            join a in _context.Aircrafts on r.CategoryMinValue equals (a.AircraftId) into leftJoinAircrafts
                                            from a in leftJoinAircrafts.DefaultIfEmpty()
                                            where r.Fboid == fboId && r.CategoryType.HasValue && r.CategoryType.Value != RampFees.RampFeeCategories.AircraftSize
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
                                                AircraftModel = a == null ? "" : a.Model,
                                                CategoryStringValue = r.CategoryStringValue
                                            }).ToListAsync();

                result.AddRange(customRampFees);
                return Ok(result);
            }
            catch (Exception)
            {
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
                        }
                        else
                        {
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
                                newRampFee.Size = AirCrafts.AircraftSizes.VeryLightJet;
                                break;
                            case "VLJ's":
                                newRampFee.Size = AirCrafts.AircraftSizes.VeryLightJet;
                                break;
                            case "VLJ":
                                newRampFee.Size = AirCrafts.AircraftSizes.VeryLightJet;
                                break;
                            case "VeryLightJet":
                                newRampFee.Size = AirCrafts.AircraftSizes.VeryLightJet;
                                break;
                            case "Light Jets":
                                newRampFee.Size = AirCrafts.AircraftSizes.LightJet;
                                break;
                            case "Light Jet":
                                newRampFee.Size = AirCrafts.AircraftSizes.LightJet;
                                break;
                            case "LightJets":
                                newRampFee.Size = AirCrafts.AircraftSizes.LightJet;
                                break;
                            case "LightJet":
                                newRampFee.Size = AirCrafts.AircraftSizes.LightJet;
                                break;
                            case "Mid Jets":
                                newRampFee.Size = AirCrafts.AircraftSizes.MediumJet;
                                break;
                            case "Medium Jets":
                                newRampFee.Size = AirCrafts.AircraftSizes.MediumJet;
                                break;
                            case "MediumJets":
                                newRampFee.Size = AirCrafts.AircraftSizes.MediumJet;
                                break;
                            case "MediumJet":
                                newRampFee.Size = AirCrafts.AircraftSizes.MediumJet;
                                break;
                            case "Medium Jet":
                                newRampFee.Size = AirCrafts.AircraftSizes.MediumJet;
                                break;
                            case "Heavy Jet":
                                newRampFee.Size = AirCrafts.AircraftSizes.HeavyJet;
                                break;
                            case "Heavy Jets":
                                newRampFee.Size = AirCrafts.AircraftSizes.HeavyJet;
                                break;
                            case "HeavyJet":
                                newRampFee.Size = AirCrafts.AircraftSizes.HeavyJet;
                                break;
                            case "HeavyJets":
                                newRampFee.Size = AirCrafts.AircraftSizes.HeavyJet;
                                break;
                            case "Super Heavy Jet":
                                newRampFee.Size = AirCrafts.AircraftSizes.SuperHeavyJet;
                                break;
                            case "Super Heavy Jets":
                                newRampFee.Size = AirCrafts.AircraftSizes.SuperHeavyJet;
                                break;
                            case "SHJ":
                                newRampFee.Size = AirCrafts.AircraftSizes.SuperHeavyJet;
                                break;
                            case "Wide Body":
                                newRampFee.Size = AirCrafts.AircraftSizes.WideBody;
                                break;
                            case "Wide Bodies":
                                newRampFee.Size = AirCrafts.AircraftSizes.WideBody;
                                break;
                            case "Light Helicopter":
                                newRampFee.Size = AirCrafts.AircraftSizes.LightHelicopter;
                                break;
                            case "Medium Helicopter":
                                newRampFee.Size = AirCrafts.AircraftSizes.MediumHelicopter;
                                break;
                            case "Heavy Helicopter":
                                newRampFee.Size = AirCrafts.AircraftSizes.HeavyHelicopter;
                                break;
                            case "Light Twin":
                                newRampFee.Size = AirCrafts.AircraftSizes.LightTwin;
                                break;
                            case "Heavy Turbo Prop":
                                newRampFee.Size = AirCrafts.AircraftSizes.HeavyTurboprop;
                                break;
                            case "Heavy TurboProp":
                                newRampFee.Size = AirCrafts.AircraftSizes.HeavyTurboprop;
                                break;
                            case "Medium TurboProp":
                                newRampFee.Size = AirCrafts.AircraftSizes.MediumTurboprop;
                                break;
                            case "Medium Turbo Prop":
                                newRampFee.Size = AirCrafts.AircraftSizes.MediumTurboprop;
                                break;
                            case "Single Engine Piston":
                                newRampFee.Size = AirCrafts.AircraftSizes.SingleEnginePiston;
                                break;
                            case "SingleEnginePiston":
                                newRampFee.Size = AirCrafts.AircraftSizes.SingleEnginePiston;
                                break;
                            case "SingleTurboProp":
                                newRampFee.Size = AirCrafts.AircraftSizes.SingleTurboProp;
                                break;
                            case "Single Turbo Prop":
                                newRampFee.Size = AirCrafts.AircraftSizes.SingleTurboProp;
                                break;
                            default:
                                newRampFee.Size = AirCrafts.AircraftSizes.NotSet;
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
                            _context.RampFees.Update(checkForExisting);
                        }
                        else
                        {
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
