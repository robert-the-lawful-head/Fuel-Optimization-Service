using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Services;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using static FBOLinx.Web.Models.AirCrafts;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAircraftsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private IHttpContextAccessor _HttpContextAccessor;

        public CustomerAircraftsController(FboLinxContext context, IHttpContextAccessor httpContextAccessor)
        {
            _HttpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<CustomerAircrafts> GetCustomerAircrafts()
        {
            return _context.CustomerAircrafts;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAircrafts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAircrafts = await (from ca in _context.CustomerAircrafts
                                           join ac in _context.Aircrafts on ca.AircraftId equals ac.AircraftId into leftJoinAircrafts
                                           from ac in leftJoinAircrafts.DefaultIfEmpty()
                                           where ca.Oid == id
                                           select new
                                           {
                                               ca.CustomerId,
                                               ca.AircraftId,
                                               ca.Oid,
                                               Size = (ca.Size.HasValue && ca.Size.Value != Models.AirCrafts.AircraftSizes.NotSet) || ac == null
                                                   ? ca.Size
                                                   : (AirCrafts.AircraftSizes)ac.Size.GetValueOrDefault(),
                                               ca.AddedFrom,
                                               ca.NetworkCode,
                                               ca.BasedPaglocation,
                                               ca.TailNumber,
                                               ca.GroupId,
                                               ac.Make,
                                               ac.Model
                                           }).FirstOrDefaultAsync();

            if (customerAircrafts == null)
            {
                return NotFound();
            }

            return Ok(customerAircrafts);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}/customer/{customerId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroupAndCustomerId([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pricingTemplates = await (
                                   from ap in _context.AircraftPrices
                                   join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                   where pt.Fboid == fboId
                                   select new
                                   {
                                       Oid = pt == null ? 0 : pt.Oid,
                                       Name = pt == null ? "" : pt.Name,
                                       ap.CustomerAircraftId
                                   }).Distinct().ToListAsync();

            List<CustomerAircraftsGridViewModel> customerAircraftVM = await (
                from ca in _context.CustomerAircrafts
                join ac in _context.Aircrafts on ca.AircraftId equals ac.AircraftId
                into acjoin from subacjoin in acjoin.DefaultIfEmpty()
                join pt in pricingTemplates on ca.Oid equals pt.CustomerAircraftId
                into leftJoinPt
                from pt in leftJoinPt.DefaultIfEmpty()
                where ca.GroupId == groupId && ca.CustomerId == customerId
                select new CustomerAircraftsGridViewModel
                {
                    Oid = ca.Oid,
                    GroupId = ca.GroupId,
                    CustomerId = ca.CustomerId,
                    AircraftId = ca.AircraftId,
                    TailNumber = ca.TailNumber,
                    Size = ca.Size.HasValue && ca.Size != AircraftSizes.NotSet
                                ? ca.Size
                                 : subacjoin.Size.GetValueOrDefault(),
                    BasedPaglocation = ca.BasedPaglocation,
                    NetworkCode = ca.NetworkCode,
                    AddedFrom = ca.AddedFrom.GetValueOrDefault(),
                    PricingTemplateId = pt == null ? 0 : pt.Oid,
                    PricingTemplateName = pt == null ? "" : pt.Name,
                    Make = subacjoin.Make,
                    Model = subacjoin.Model
                })
                .OrderBy(x => x.TailNumber)
                .ToListAsync();

            return Ok(customerAircraftVM);
        }

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroup([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (groupId != UserService.GetClaimedGroupId(_HttpContextAccessor))
                return BadRequest(ModelState);

            List<CustomerAircraftsGridViewModel> customerAircraft = await (
                from ca in _context.CustomerAircrafts
                join ac in _context.Aircrafts on ca.AircraftId equals ac.AircraftId
                join a in _context.AircraftPrices on ca.Oid equals a.CustomerAircraftId
                into leftJoinAircraftPrices
                from a in leftJoinAircraftPrices.DefaultIfEmpty()
                join p in _context.PricingTemplate on a.PriceTemplateId equals p.Oid
                into leftJoinPricingTemplate
                from p in leftJoinPricingTemplate.DefaultIfEmpty()
                join cg in _context.CustomerInfoByGroup on new {groupId, ca.CustomerId} equals new {groupId = cg.GroupId, cg.CustomerId}
                where ca.GroupId.GetValueOrDefault() == groupId
                select new CustomerAircraftsGridViewModel
                {
                    Oid = ca.Oid,
                    GroupId = ca.GroupId.GetValueOrDefault(),
                    CustomerId = ca.CustomerId,
                    Company = cg == null ? "" : cg.Company,
                    AircraftId = ca.AircraftId,
                    TailNumber = ca.TailNumber,
                    Size = ca.Size.HasValue && ca.Size != AircraftSizes.NotSet
                        ? ca.Size
                        : ac.Size.GetValueOrDefault(),
                    BasedPaglocation = ca.BasedPaglocation,
                    NetworkCode = ca.NetworkCode,
                    AddedFrom = ca.AddedFrom.GetValueOrDefault(),
                    PricingTemplateId = a == null ? 0 : a.PriceTemplateId.GetValueOrDefault(),
                    PricingTemplateName = p == null ? "" : p.Name,
                    Make = ac.Make,
                    Model = ac.Model
                }).OrderBy((x => x.TailNumber)).ToListAsync();

            return Ok(customerAircraft);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroupAndFbo([FromRoute] int groupId, [FromRoute] int fboId)
        {
            var pricingTemplates = await (
                                   from ap in _context.AircraftPrices
                                   join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                   where pt.Fboid == fboId
                                   select new
                                   {
                                       Oid = pt == null ? 0 : pt.Oid,
                                       Name = pt == null ? "" : pt.Name,
                                       ap.CustomerAircraftId
                                   }).Distinct().ToListAsync();

            List<CustomerAircraftsGridViewModel> customerAircraftVM = await (
               from ca in _context.CustomerAircrafts
               join cg in _context.CustomerInfoByGroup on new { groupId, ca.CustomerId } equals new { groupId = cg.GroupId, cg.CustomerId }
               join c in _context.Customers on cg.CustomerId equals c.Oid
               join ac in _context.Aircrafts on ca.AircraftId equals ac.AircraftId
               into acjoin from subacjoin in acjoin.DefaultIfEmpty()
               join pt in pricingTemplates on ca.Oid equals pt.CustomerAircraftId
               into leftJoinPt
               from pt in leftJoinPt.DefaultIfEmpty()
               where ca.GroupId == groupId
               select new CustomerAircraftsGridViewModel
               {
                   Oid = ca.Oid,
                   GroupId = ca.GroupId,
                   CustomerId = ca.CustomerId,
                   Company = cg.Company,
                   AircraftId = ca.AircraftId,
                   TailNumber = ca.TailNumber,
                   Size = ca.Size.HasValue && ca.Size != AircraftSizes.NotSet
                               ? ca.Size
                                : subacjoin.Size.GetValueOrDefault(),
                   BasedPaglocation = ca.BasedPaglocation,
                   NetworkCode = ca.NetworkCode,
                   AddedFrom = ca.AddedFrom.GetValueOrDefault(),
                   PricingTemplateId = pt == null ? 0 : pt.Oid,
                   PricingTemplateName = pt == null ? "" : pt.Name,
                   Make = subacjoin.Make,
                   Model = subacjoin.Model,
                   IsFuelerlinxNetwork = c.FuelerlinxId > 0
               })
               .OrderBy(x => x.TailNumber)
               .ToListAsync();

            return Ok(customerAircraftVM);
        }

        [HttpGet("group/{groupId}/count")]
        public IActionResult GetCustomerAircraftsCountByGroupId([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAircraftCount = (from ca in _context.CustomerAircrafts
                                         join cg in _context.CustomerInfoByGroup on new
                                         {
                                             ca.CustomerId,
                                             GroupId = ca.GroupId.GetValueOrDefault()
                                         }
                                             equals new
                                             {
                                                 cg.CustomerId,
                                                 cg.GroupId
                                             }
                                         where ca.GroupId == groupId

                                         select new
                                         {
                                             Oid = ca.Oid
                                         }).Count();

            return Ok(customerAircraftCount);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAircrafts([FromRoute] int id, [FromBody] CustomerAircraftsGridViewModel customerAircrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerAircrafts.Oid)
            {
                return BadRequest();
            }


            try
            {
                CustomerAircrafts custAircraft = _context.CustomerAircrafts.FirstOrDefault(s => s.Oid == customerAircrafts.Oid);

                if (custAircraft != null)
                {
                    custAircraft.TailNumber = customerAircrafts.TailNumber;
                    custAircraft.AircraftId = customerAircrafts.AircraftId;
                    custAircraft.Size = customerAircrafts.Size;
                    _context.CustomerAircrafts.Update(custAircraft);
                }
                await _context.SaveChangesAsync();

                return Ok(custAircraft);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAircraftsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPut("fbo/{fboid}")]
        public async Task<IActionResult> PutCustomerAircraftsTemplate([FromRoute] int fboid, [FromBody] CustomerAircraftsGridViewModel customerAircrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                CustomerAircrafts custAircraft = _context.CustomerAircrafts.FirstOrDefault(s => s.Oid == customerAircrafts.Oid);

                AircraftPrices aircraftPrice = _context.AircraftPrices.FirstOrDefault(a => a.CustomerAircraftId.Equals(custAircraft.Oid));
                CustomCustomerTypes customerMargin = _context.CustomCustomerTypes.FirstOrDefault(s => s.CustomerId == customerAircrafts.CustomerId && s.Fboid == fboid);
                PricingTemplate pricingTemplate = _context.PricingTemplate.FirstOrDefault(s => s.Name == customerAircrafts.PricingTemplateName && s.Fboid == fboid);
                if (aircraftPrice != null)
                {
                    aircraftPrice.PriceTemplateId = pricingTemplate?.Oid;
                    _context.AircraftPrices.Update(aircraftPrice);
                }
                else
                {
                    AircraftPrices newAircraftPrice = new AircraftPrices
                    {
                        CustomerAircraftId = custAircraft.Oid,
                        PriceTemplateId = pricingTemplate.Oid
                    };
                    _context.AircraftPrices.Add(newAircraftPrice);
                }

                if (custAircraft != null)
                {
                    custAircraft.TailNumber = customerAircrafts.TailNumber;
                    custAircraft.AircraftId = customerAircrafts.AircraftId;
                    custAircraft.Size = customerAircrafts.Size;
                    _context.CustomerAircrafts.Update(custAircraft);
                }

                // Tail-specific margin template overrides customer level templates.
                if (customerMargin != null && pricingTemplate != null)
                {
                    customerMargin.CustomerType = pricingTemplate.Oid;
                }

                await _context.SaveChangesAsync();
                return Ok(custAircraft);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAircraftsExists(customerAircrafts.Oid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomerAircrafts([FromBody] CustomerAircrafts customerAircrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerAircrafts.Add(customerAircrafts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerAircrafts", new { id = customerAircrafts.Oid }, customerAircrafts);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCustomerAircrafts([FromBody] List<AircraftImportVM> customerAircrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach(var singleAircraft in customerAircrafts)
            {
                var aircraft = _context.Aircrafts.FirstOrDefault(s => s.Make == singleAircraft.AircraftMake && s.Model == singleAircraft.Model);

                if (aircraft != null)
                {
                    CustomerAircrafts newCa = new CustomerAircrafts();
                    newCa.GroupId = singleAircraft.GroupId;
                    newCa.CustomerId = singleAircraft.CustomerId;
                    newCa.AircraftId = aircraft.AircraftId;
                    newCa.TailNumber = singleAircraft.TailNumber;
                    if (singleAircraft.Size != null)
                    {
                        AircraftSizes acSize = (AircraftSizes)singleAircraft.Size;
                        newCa.Size = acSize;
                    }
                    singleAircraft.IsImported = true;
                    _context.CustomerAircrafts.Add(newCa);
                    _context.SaveChanges();

                    
                }
                else
                {
                    singleAircraft.IsImported = false;
                    var otherOptions = _context.Aircrafts.Where(s => s.Make == singleAircraft.AircraftMake).Select(s => s.Model);

                    if(otherOptions != null)
                    {
                        List<string> listOfModels = new List<string>();
                        foreach(var model in otherOptions)
                        {
                            listOfModels.Add(model);    
                        }
                        listOfModels = listOfModels.OrderByDescending(s => s).ToList();
                        singleAircraft.OtherOptions = listOfModels;
                        singleAircraft.selectedModel = "";
                    }

                    
                }
            }

            var checkForErrors = customerAircrafts.FirstOrDefault(s => s.IsImported == false);

            if (checkForErrors !=null)
            {
                return Ok(customerAircrafts.Where(s => s.IsImported == false).ToList());
            }

            return Ok(customerAircrafts);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAircrafts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAircrafts = await _context.CustomerAircrafts.FindAsync(id);
            if (customerAircrafts == null)
            {
                return NotFound();
            }

            _context.CustomerAircrafts.Remove(customerAircrafts);
            await _context.SaveChangesAsync();

            return Ok(customerAircrafts);
        }

        private bool CustomerAircraftsExists(int id)
        {
            return _context.CustomerAircrafts.Any(e => e.Oid == id);
        }
    }
}