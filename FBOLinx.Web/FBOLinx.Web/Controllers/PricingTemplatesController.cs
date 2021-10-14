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
using FBOLinx.Web.DTO;
using FBOLinx.Web.Services;
using FBOLinx.Web.Services.Interfaces;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PricingTemplatesController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IPriceFetchingService _priceFetchingService;
        private readonly IPricingTemplateService _pricingTemplateService;

        public PricingTemplatesController(FboLinxContext context, IPriceFetchingService priceFetchingService, IPricingTemplateService pricingTemplateService)
        {
            _context = context;
            _priceFetchingService = priceFetchingService;
            _pricingTemplateService = pricingTemplateService;
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

        [HttpGet("fbodefaultpricingtemplate/group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetPricingTemplateByFboIdForDefaultTemplate([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PricingTemplateService pricingTemplateService = new PricingTemplateService(_context);
            await pricingTemplateService.FixDefaultPricingTemplate(fboId);

            List<PricingTemplatesGridViewModel> marginTemplates = await _pricingTemplateService.GetPricingTemplates(fboId, groupId);

            return Ok(marginTemplates);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetPricingTemplateByGroupIdAndFboId([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<PricingTemplatesGridViewModel> marginTemplates = await _pricingTemplateService.GetPricingTemplates(fboId, groupId);

            return Ok(marginTemplates);
        }

        [HttpGet("with-email-content/group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetPricingTemplatesWithEmailContentByGroupIdAndFboId([FromRoute] int groupId, [FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<PricingTemplatesGridViewModel> templates = await _pricingTemplateService.GetPricingTemplates(fboId, groupId);
            var templatesWithEmailContent = (
                from t in templates
                join ec in _context.EmailContent on t.EmailContentId equals ec.Oid
                into leftJoinedEC
                from ec in leftJoinedEC.DefaultIfEmpty()
                select new PricingTemplatesGridViewModel
                {
                    CustomerId = t.CustomerId,
                    Default = t.Default,
                    Fboid = t.Fboid,
                    Margin = t.Margin,
                    YourMargin = t.Margin,
                    MarginType = t.MarginType,
                    Name = t.Name,
                    Notes = t.Notes,
                    Oid = t.Oid,
                    Type = t.Type,
                    Subject = t.Subject,
                    Email = t.Email,
                    IsPricingExpired = t.IsPricingExpired,
                    IntoPlanePrice = t.IntoPlanePrice,
                    CustomersAssigned = t.CustomersAssigned,
                    EmailContentId = t.EmailContentId,
                    EmailContent = ec
                }
                ).ToList();

            foreach (var t in templatesWithEmailContent)
            {
                t.CustomerEmails = await (from cg in _context.CustomerInfoByGroup.Where((x => x.GroupId == groupId))
                                    join c in _context.Customers on cg.CustomerId equals c.Oid
                                    join cc in _context.CustomCustomerTypes.Where(x => x.Fboid == fboId) on cg.CustomerId equals cc.CustomerId
                                    join custc in _context.CustomerContacts on c.Oid equals custc.CustomerId
                                    join co in _context.Contacts on custc.ContactId equals co.Oid
                                    join cibg in _context.ContactInfoByGroup on co.Oid equals cibg.ContactId
                                    where (cg.Active ?? false)
                                          && cc.CustomerType == t.Oid
                                          && (cibg.CopyAlerts ?? false) == true
                                          && !string.IsNullOrEmpty(cibg.Email)
                                          && cibg.GroupId == groupId
                                          && (c.Suspended ?? false) == false
                                    select cibg.Email
                                ).ToListAsync();
            }

            return Ok(templatesWithEmailContent);
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
                        MaxPrice = cmResults.Max(x => (x.Amount ?? 0))
                    }) on p.Oid equals cm.TemplateId
                into leftJoinCustomerMargins
                from cm in leftJoinCustomerMargins.DefaultIfEmpty()
                join fp in (
                    from f in _context.Fboprices
                    where f.EffectiveTo > DateTime.UtcNow && f.Fboid == fboId && f.Expired != true
                    select f) on p.MarginTypeProduct equals fp.Product into leftJoinFboPrices
                from fp in leftJoinFboPrices.DefaultIfEmpty()
                where p.Fboid == fboId
                    && (p.Default ?? false)
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
                    IntoPlanePrice = (fp == null ? 0 : (fp.Price ?? 0)) +
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
                        Notes = existingTemplate.Notes,
                        MarginType = existingTemplate.MarginType,
                        Email = existingTemplate.Email,
                        Subject = existingTemplate.Subject,
                        EmailContentId = existingTemplate.EmailContentId
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

            PricingTemplate defaultPricingTemplate = await _context.PricingTemplate.FirstOrDefaultAsync(p => p.Fboid.Equals(fboId) && (p.Default ?? false));
            if (defaultPricingTemplate != null)
            {
                var customers = _context.CustomCustomerTypes
                    .Where(c => c.Fboid.Equals(fboId) && c.CustomerType.Equals(oid))
                    .Select(s => s.CustomerId)
                    .ToList();

                var groupInfo = _context.Fbos.FirstOrDefault(s => s.Oid == fboId).GroupId;
                _context.CustomerInfoByGroup.Where(s => customers.Contains(s.CustomerId) && s.GroupId == groupInfo).ToList().ForEach(s => s.PricingTemplateRemoved = true);

                var customCustomerTypes = _context.CustomCustomerTypes
                    .Where(c => c.Fboid.Equals(fboId) && c.CustomerType.Equals(oid))
                    .ToList();

                customCustomerTypes.ForEach(c => c.CustomerType = defaultPricingTemplate.Oid);

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
                    x.Fboid == pricingTemplate.Fboid && (x.Default ?? false) && x.Oid != pricingTemplate.Oid);
                foreach (var otherDefault in otherDefaults)
                {
                    otherDefault.Default = false;
                    _context.Entry(otherDefault).State = EntityState.Modified;
                }
            }
        }
    }
}
