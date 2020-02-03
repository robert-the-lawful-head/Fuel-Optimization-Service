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

        // GET: api/CustomerAircrafts
        [HttpGet]
        public IEnumerable<CustomerAircrafts> GetCustomerAircrafts()
        {
            return _context.CustomerAircrafts;
        }

        // GET: api/CustomerAircrafts/5
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
                        : (AirCrafts.AircraftSizes) ac.Size.GetValueOrDefault(),
                    ca.AddedFrom,
                    ca.NetworkCode,
                    ca.BasedPaglocation,
                    ca.TailNumber,
                    ca.GroupId
                }).FirstOrDefaultAsync();

            if (customerAircrafts == null)
            {
                return NotFound();
            }

            return Ok(customerAircrafts);
        }

        // GET: api/CustomerAircrafts/group/5/fbo/6/customer/7
        [HttpGet("group/{groupId}/fbo/{fboId}/customer/{customerId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroupAndCustomerId([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAircraftVM = await (from ca in _context.CustomerAircrafts
                join ac in _context.Aircrafts on ca.AircraftId equals ac.AircraftId
                join a in _context.AircraftPrices on ca.Oid equals a.CustomerAircraftId into leftJoinAircraftPrices
                from a in leftJoinAircraftPrices.DefaultIfEmpty()
                join p in _context.PricingTemplate on a.PriceTemplateId equals p.Oid into leftJoinPricingTemplate
                from p in leftJoinPricingTemplate.DefaultIfEmpty()
                where ca.GroupId.GetValueOrDefault() == groupId && ca.CustomerId == customerId
                select new CustomerAircraftsGridViewModel
                {
                    Oid = ca.Oid,
                    GroupId = ca.GroupId.GetValueOrDefault(),
                    CustomerId = ca.CustomerId,
                    AircraftId = ca.AircraftId,
                    TailNumber = ca.TailNumber,
                    Size = ca.Size.HasValue && ca.Size != Models.AirCrafts.AircraftSizes.NotSet
                        ? ca.Size
                        : ac.Size.GetValueOrDefault(),
                    BasedPaglocation = ca.BasedPaglocation,
                    NetworkCode = ca.NetworkCode,
                    AddedFrom = ca.AddedFrom.GetValueOrDefault(),
                    PricingTemplateId = a == null ? 0 : a.PriceTemplateId.GetValueOrDefault(),
                    PricingTemplateName = p == null ? "Customer Default" : p.Name,
                    Make = ac.Make,
                    Model = ac.Model
                }).OrderBy((x => x.TailNumber)).ToListAsync();

            return Ok(customerAircraftVM);
        }

        // GET: api/CustomerAircrafts/group/5/fbo/6
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroup([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (groupId != UserService.GetClaimedGroupId(_HttpContextAccessor))
                return BadRequest(ModelState);

            var customerAircraft = await (from ca in _context.CustomerAircrafts
                                            join ac in _context.Aircrafts on ca.AircraftId equals ac.AircraftId
                                            //join a in _context.AircraftPrices on ca.Oid equals a.CustomerAircraftId into leftJoinAircraftPrices
                                            //from a in leftJoinAircraftPrices.DefaultIfEmpty()
                                            //join p in _context.PricingTemplate on a.PriceTemplateId equals p.Oid into leftJoinPricingTemplate
                                            //from p in leftJoinPricingTemplate.DefaultIfEmpty()
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
                                                Size = ca.Size.HasValue && ca.Size != Models.AirCrafts.AircraftSizes.NotSet
                                                    ? ca.Size
                                                    : ac.Size.GetValueOrDefault(),
                                                BasedPaglocation = ca.BasedPaglocation,
                                                NetworkCode = ca.NetworkCode,
                                                AddedFrom = ca.AddedFrom.GetValueOrDefault(),
                                                //PricingTemplateId = a == null ? 0 : a.PriceTemplateId.GetValueOrDefault(),
                                                //PricingTemplateName = p == null ? "Customer Default" : p.Name,
                                                Make = ac.Make,
                                                Model = ac.Model
                                            }).OrderBy((x => x.TailNumber)).ToListAsync();

            return Ok(customerAircraft);
        }

        // GET: api/CustomerAircrafts/group/5/fbo/6/count
        [HttpGet("group/{groupId}/count")]
        public async Task<IActionResult> GetCustomerAircraftsCountByGroupId([FromRoute] int groupId)
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

        // PUT: api/CustomerAircrafts/5
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

           // _context.Entry(customerAircrafts).State = EntityState.Modified;

            try
            {
                var custAircraft = _context.CustomerAircrafts.FirstOrDefault(s => s.Oid == customerAircrafts.Oid);

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

            return NoContent();
        }

        // POST: api/CustomerAircrafts
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

        // DELETE: api/CustomerAircrafts/5
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

        private IQueryable<CustomerAircrafts> GetAllCustomerAircrafts()
        {
            return _context.CustomerAircrafts.AsQueryable();
        }
    }
}