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
using Remotion.Linq.Clauses;
using FBOLinx.Web.DTO;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PricingTemplatesController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public PricingTemplatesController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/PricingTemplates
        [HttpGet]
        public IEnumerable<PricingTemplate> GetPricingTemplate()
        {
            return _context.PricingTemplate;
        }

        // GET: api/PricingTemplates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPricingTemplate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pricingTemplate = await _context.PricingTemplate.FindAsync(id);

            if (pricingTemplate == null)
            {
                return NotFound();
            }

            return Ok(pricingTemplate);
        }

        // GET: api/PricingTemplates/fbo/5
        [HttpGet("fbo/{fboId}")]
        public async Task<IActionResult> GetPricingTemplateByFboId([FromRoute] int fboId)
        {
            List<PricingTemplatesGridViewModel> marginTemplates = await GetPricingTemplates(fboId, null);

            return Ok(marginTemplates);
        }

        [HttpGet("fbodefaultpricingtemplate/{fboId}")]
        public async Task<IActionResult> GetPricingTemplateByFboIdForDefaultTemplate([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<PricingTemplatesGridViewModel> marginTemplates = await GetPricingTemplates(fboId, null);

            if (fboId != 0 && marginTemplates.Count == 0)
            {
                PricingTemplate ptNew = new PricingTemplate
                {
                    Fboid = fboId,
                    Name = "Temporary Default Template",
                    Default = true,
                    Notes = "This is temporary default template created because the customer did not have any templates",
                    Type = 0,
                    MarginType = PricingTemplate.MarginTypes.RetailMinus
                };

                _context.PricingTemplate.Add(ptNew);
                await _context.SaveChangesAsync();

                List<PricingTemplatesGridViewModel> updatedMarginTemplates = await GetPricingTemplates(fboId, null);


                return Ok(updatedMarginTemplates);
            }

            return Ok(marginTemplates);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetPricingTemplateByGroupIdAndFboId([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<PricingTemplatesGridViewModel> marginTemplates = await GetPricingTemplates(fboId, groupId);

            return Ok(marginTemplates);
        }

        [HttpGet("getcostpluspricingtemplate/{fboId}")]
        public async Task<IActionResult> GetCostPlusPricingTemplates([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var templateCustomersCount = (
                from tc in
                (
                    from cig in _context.CustomerInfoByGroup
                    join cct in _context.CustomCustomerTypes on cig.CustomerId equals cct.CustomerId
                    join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid
                    where pt.Fboid == fboId && !string.IsNullOrEmpty(cct.CustomerType.ToString())
                    select new
                    {
                        pt.Oid,
                        cct.CustomerId
                    }
                )
                group tc by tc.Oid into resultsGroup
                select new
                {
                    Oid = resultsGroup.Key,
                    Count = resultsGroup.Count()
                }).ToList();



            List<PricingTemplatesGridViewModel> result = (
                from p in _context.PricingTemplate
                join tcc in templateCustomersCount on p.Oid equals tcc.Oid
                into leftJoinTemplateCustomersCount
                from tcc in leftJoinTemplateCustomersCount.DefaultIfEmpty()
                where p.Fboid == fboId && p.MarginType == 0
                select new PricingTemplatesGridViewModel
                {
                    CustomersAssigned = tcc == null ? 0 : tcc.Count
                }).ToList();

            if(result.Count > 0)
            {
                var custAssigned = result.FirstOrDefault(s => s.CustomersAssigned > 0);

                if(custAssigned != null)
                {
                    return Ok(new { Exist = true });
                }
                else
                {
                    return Ok(new { Exist = false });
                }
            }

            return Ok(new { Exist = false });
        }


        // GET: api/PricingTemplates/fbo/5/default
        [HttpGet("fbo/{fboId}/default")]
        public async Task<IActionResult> GetDefaultPricingTemplateByFboId([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await (
                from p in _context.PricingTemplate
                join cm in (
                    from c in _context.CustomerMargins
                    group c by new { c.TemplateId }
                    into cmResults
                    select new CustomerMarginModel {
                        TemplateId = cmResults.Key.TemplateId,
                        MaxPrice = cmResults.Max(x => x.Amount.GetValueOrDefault())
                    }) on p.Oid equals cm.TemplateId
                into leftJoinCustomerMargins
                from cm in leftJoinCustomerMargins.DefaultIfEmpty()
                join fp in (
                    from f in _context.Fboprices
                    where f.EffectiveTo > DateTime.Now.AddDays(-1) && f.Fboid == fboId && f.Expired != true
                    select f) on p.MarginTypeProduct equals fp.Product into leftJoinFboPrices
                from fp in leftJoinFboPrices.DefaultIfEmpty()
                where p.Fboid == fboId
                    && p.Default.GetValueOrDefault()
                select new PricingTemplatesGridViewModel
                {
                    CustomerId = p.CustomerId,
                    Default = p.Default,
                    Fboid = p.Fboid,
                    Margin = cm == null ? 0 : cm.MaxPrice,
                    MarginType = p.MarginType,
                    Name = p.Name,
                    Notes = p.Notes,
                    Subject = p.Subject,
                    Email = p.Email,
                    Oid = p.Oid,
                    Type = p.Type,
                    IntoPlanePrice = (fp == null ? 0 : fp.Price.GetValueOrDefault()) +
                                     (cm == null ? 0 : cm.MaxPrice)
                }).ToListAsync();


            return Ok(result);
        }

        // PUT: api/PricingTemplates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPricingTemplate([FromRoute] int id, [FromBody] PricingTemplate pricingTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pricingTemplate.Oid)
            {
                return BadRequest();
            }

            _context.Entry(pricingTemplate).State = EntityState.Modified;
            FixOtherDefaults(pricingTemplate);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricingTemplateExists(id))
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

        // POST: api/PricingTemplates
        [HttpPost]
        public async Task<IActionResult> PostPricingTemplate([FromBody] PricingTemplate pricingTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FixOtherDefaults(pricingTemplate);

            _context.PricingTemplate.Add(pricingTemplate);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPricingTemplate", new { id = pricingTemplate.Oid }, pricingTemplate);
        }

        [HttpPost("copypricingtemplate")]
        public async Task<IActionResult> CopyPricingTemplate([FromBody] PricingTemplateVM pricingTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(pricingTemplate.currentPricingTemplateId !=null && pricingTemplate.name != string.Empty)
            {
                var existingTemplate = _context.PricingTemplate.FirstOrDefault(s => s.Oid == pricingTemplate.currentPricingTemplateId);

                if(existingTemplate != null)
                {
                    PricingTemplate pt = new PricingTemplate
                    {
                        Name = pricingTemplate.name,
                        Fboid = existingTemplate.Fboid,
                        Default = false,
                        Notes = "",
                        MarginType = existingTemplate.MarginType,
                        Email = existingTemplate.Email,
                        Subject = existingTemplate.Subject
                    };

                    _context.PricingTemplate.Add(pt);
                    await _context.SaveChangesAsync();

                    if(pt.Oid != 0)
                    {
                        var listMargins = _context.CustomerMargins.Where(s => s.TemplateId == pricingTemplate.currentPricingTemplateId && s.PriceTierId != 0).ToList();

                        if(listMargins.Count > 0)
                        {
                            foreach(var margin in listMargins)
                            {
                                CustomerMargins cm = new CustomerMargins
                                {
                                    TemplateId = pt.Oid,
                                    Amount = margin.Amount
                                };

                                _context.CustomerMargins.Add(cm);
                                _context.SaveChanges();

                                var priceTier = _context.PriceTiers.Where(s => s.Oid == margin.PriceTierId).ToList();

                                foreach (var pricet in priceTier)
                                {
                                    PriceTiers ptNew = new PriceTiers
                                    {
                                        Min = pricet.Min,
                                        Max = pricet.Max
                                    };

                                    _context.PriceTiers.Add(ptNew);
                                    await _context.SaveChangesAsync();

                                    if (ptNew.Oid != 0)
                                    {

                                        cm.PriceTierId = ptNew.Oid;

                                        _context.CustomerMargins.Update(cm);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    return Ok(pt.Oid);
                }
            }

            return null;

        }

        [HttpGet("checkdefaulttemplate/{fboId}")]
        public async Task<IActionResult> CheckDefaultTemplate([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(fboId != 0)
            {
                var result = _context.PricingTemplate.FirstOrDefault(s => s.Fboid == fboId && s.Default == true);

                if (result != null)
                {
                    return Ok(result);
                }
            }
            

            return Ok(null);
        }

        // DELETE: api/PricingTemplates/5/fbo/124
        [HttpDelete("{oid}/fbo/{fboId}")]
        public async Task<IActionResult> DeletePricingTemplate([FromRoute] int oid, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PricingTemplate pricingTemplate = await _context.PricingTemplate.FindAsync(oid);
            if (pricingTemplate == null)
            {
                return NotFound();
            }

            _context.PricingTemplate.Remove(pricingTemplate);
            await _context.SaveChangesAsync();

            PricingTemplate defaultPricingTemplate = _context.PricingTemplate.Where(p => p.Fboid.Equals(fboId) && p.Default.GetValueOrDefault()).FirstOrDefault();
            if (defaultPricingTemplate != null)
            {
                _context.CustomCustomerTypes
                    .Where(c => c.Fboid.Equals(fboId) && c.CustomerType.Equals(oid))
                    .ToList()
                    .ForEach(c => c.CustomerType = defaultPricingTemplate.Oid);

                await _context.SaveChangesAsync();
            }

            return Ok(pricingTemplate);
        }

        private bool PricingTemplateExists(int id)
        {
            return _context.PricingTemplate.Any(e => e.Oid == id);
        }

        private void FixOtherDefaults(PricingTemplate pricingTemplate)
        {
            if (pricingTemplate.Default.GetValueOrDefault())
            {
                var otherDefaults = _context.PricingTemplate.Where(x =>
                    x.Fboid == pricingTemplate.Fboid && x.Default.GetValueOrDefault() && x.Oid != pricingTemplate.Oid);
                foreach (var otherDefault in otherDefaults)
                {
                    otherDefault.Default = false;
                    _context.Entry(otherDefault).State = EntityState.Modified;
                }
            }
        }

        private double GetMargin(Fboprices jetaACostRecord, Fboprices price, CustomerMarginModel cm)
        {
            if (jetaACostRecord == null || jetaACostRecord.Price.GetValueOrDefault() <= 0)
            {
                return 0;
            }
            double result = 0;
            if (price != null)
            {
                result = price.Price.GetValueOrDefault();
            }

            if (cm == null)
            {
                return result - jetaACostRecord.Price.GetValueOrDefault();
            }
            return result + cm.MaxPrice - jetaACostRecord.Price.GetValueOrDefault();
        }

        private async Task<List<PricingTemplatesGridViewModel>> GetPricingTemplates(int fboId, int? groupId)
        {
            Fboprices jetaACostRecord = await _context.Fboprices.Where(x => x.Fboid == fboId && x.Product == "JetA Cost" && x.Expired != true).FirstOrDefaultAsync();
            IEnumerable<Utilities.Enum.EnumDescriptionValue> products = Utilities.Enum.GetDescriptions(typeof(Models.Fboprices.FuelProductPriceTypes));

            var fboPrices = await (from f in _context.Fboprices
                                   where f.EffectiveTo > DateTime.Now.AddDays(-1) && f.Fboid == fboId && f.Expired != true
                                   select f).ToListAsync();

            var templateCustomersCount = await (
                from tc in
                (
                    from cig in _context.CustomerInfoByGroup
                    join cct in _context.CustomCustomerTypes on cig.CustomerId equals cct.CustomerId
                    join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid
                    where cig.GroupId == groupId && pt.Fboid == fboId && !string.IsNullOrEmpty(cct.CustomerType.ToString())
                    select new
                    {
                        pt.Oid,
                        cct.CustomerId
                    }
                )
                group tc by tc.Oid into resultsGroup
                select new
                {
                    Oid = resultsGroup.Key,
                    Count = resultsGroup.Count()
                }).ToListAsync();

            var resultPrices =
                          from p in products
                          join f in fboPrices on new { Product = p.Description, FboId = fboId } equals new { f.Product, FboId = f.Fboid.GetValueOrDefault() }
                          into leftJoinFBOPrices
                          from f in leftJoinFBOPrices.DefaultIfEmpty()
                          join s in (from s in _context.TempAddOnMargin
                                     where s.FboId == fboId && s.EffectiveTo >= DateTime.Today.ToUniversalTime()
                                     select s) on fboId equals s.FboId
                              into tmpJoin
                          from s in tmpJoin.DefaultIfEmpty()
                          select new
                          {
                              Oid = f?.Oid ?? 0,
                              Fboid = fboId,
                              Product = p.Description,
                              f?.Price,
                              EffectiveFrom = f?.EffectiveFrom ?? DateTime.Now,
                              f?.EffectiveTo,
                              TimeStamp = f?.Timestamp,
                              f?.SalesTax,
                              f?.Currency,
                              tempJet = s?.MarginJet,
                              tempAvg = s?.MarginAvgas,
                              tempId = s?.Id,
                              tempDateFrom = s?.EffectiveFrom,
                              tempDateTo = s?.EffectiveTo
                          };

            double? jetACost = resultPrices.FirstOrDefault(s => s.Product == "JetA Cost").Price;
            double? jetARetail = resultPrices.FirstOrDefault(s => s.Product == "JetA Retail").Price;

            List<PricingTemplatesGridViewModel> result = (
                from p in _context.PricingTemplate
                join f in (_context.Fbos.Include("Preferences")) on p.Fboid equals f.Oid
                join cm in (
                    from c in _context.CustomerMargins
                    join tm in _context.PriceTiers
                    on c.PriceTierId equals tm.Oid
                    group c by new { c.TemplateId }
                    into cmResults
                    select new CustomerMarginModel
                    {
                        TemplateId = cmResults.Key.TemplateId,
                        MaxPrice = cmResults.FirstOrDefault().Amount.GetValueOrDefault()
                    }
                ) on p.Oid equals cm.TemplateId
                into leftJoinCustomerMargins
                from cm in leftJoinCustomerMargins.DefaultIfEmpty()
                join fp in fboPrices on p.MarginTypeProduct equals fp.Product
                into leftJoinFboPrices
                from fp in leftJoinFboPrices.DefaultIfEmpty()
                join tcc in templateCustomersCount on p.Oid equals tcc.Oid
                into leftJoinTemplateCustomersCount
                from tcc in leftJoinTemplateCustomersCount.DefaultIfEmpty()
                where p.Fboid == fboId
                select new PricingTemplatesGridViewModel
                {
                    CustomerId = p.CustomerId.GetValueOrDefault(),
                    Default = p.Default.GetValueOrDefault(),
                    Fboid = p.Fboid,
                    Margin = cm == null ? 0 : cm.MaxPrice,
                    MarginType = p.MarginType.GetValueOrDefault(),
                    Name = p.Name,
                    Notes = p.Notes,
                    Oid = p.Oid,
                    Type = p.Type.GetValueOrDefault(),
                    Subject = p.Subject,
                    Email = p.Email,
                    IntoPlanePrice = (jetaACostRecord == null ? 0 : jetaACostRecord.Price.GetValueOrDefault()) + (cm == null ? 0 : cm.MaxPrice),
                    IsInvalid = (f != null && f.Preferences != null &&
                        ((f.Preferences.OmitJetACost.GetValueOrDefault() && p.MarginType.GetValueOrDefault() == PricingTemplate.MarginTypes.CostPlus)
                        || f.Preferences.OmitJetARetail.GetValueOrDefault() && p.MarginType.GetValueOrDefault() == PricingTemplate.MarginTypes.RetailMinus)
                    ) ? true : false,
                    IsPricingExpired = (fp == null && (p.MarginType == null || p.MarginType != PricingTemplate.MarginTypes.FlatFee)),
                    YourMargin = GetMargin(jetaACostRecord, fp, cm),
                    CustomersAssigned = tcc == null ? 0 : tcc.Count
                }).ToList();

            result = result.OrderBy(s => s.Name).GroupBy(s => s.Oid).Select(g => g.First()).ToList();

            foreach (PricingTemplatesGridViewModel res in result)
            {
                if (res.Oid != 0)
                {
                    CustomerMargins margins = _context.CustomerMargins.FirstOrDefault(s => s.TemplateId == res.Oid && s.PriceTierId != 0);

                    if (margins != null)
                    {
                        if (res.MarginTypeDescription == "Retail -")
                        {
                            if (jetARetail != null)
                            {
                                res.IntoPlanePrice = jetARetail.Value - Convert.ToDouble(margins.Amount);
                            }
                            else
                            {
                                res.IntoPlanePrice = Convert.ToDouble(margins.Amount) + 0;
                            }

                            if (jetACost != null)
                            {
                                res.YourMargin = res.Margin;
                            }
                            else
                            {
                                res.YourMargin = res.IntoPlanePrice - 0;
                            }
                        }
                        else if (res.MarginTypeDescription == "Cost +")
                        {
                            if (jetACost != null)
                            {
                                res.IntoPlanePrice = Convert.ToDouble(margins.Amount) + jetACost.Value;
                                res.YourMargin = res.IntoPlanePrice - jetACost.Value;
                            }
                            else
                            {
                                res.IntoPlanePrice = Convert.ToDouble(margins.Amount) + 0;
                                res.YourMargin = res.IntoPlanePrice - 0;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
