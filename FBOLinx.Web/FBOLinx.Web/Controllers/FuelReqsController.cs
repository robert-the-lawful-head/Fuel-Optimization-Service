using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FuelReqsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public FuelReqsController(FboLinxContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _HttpContextAccessor = httpContextAccessor;
        }

        // GET: api/FuelReqs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFuelReq([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fuelReq = await _context.FuelReq.FindAsync(id);

            if (fuelReq == null)
            {
                return NotFound();
            }

            return Ok(fuelReq);
        }

        // Get: api/FuelReqs/group/5
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetFuelReqsByGroup([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var fuelReq = await GetAllFuelRequests().Where((x => x.fbo)).ToListAsync();

            return NotFound();
        }

        // Get: api/FuelReqs/fbo/5
        [HttpGet("fbo/{fboId}")]
        public async Task<IActionResult> GetFuelReqsByFbo([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UserService.GetClaimedFboId(_HttpContextAccessor) != fboId && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.GroupAdmin)
            {
                return BadRequest("Invalid FBO");
            }

            var fuelReq = await GetAllFuelRequests().Include("Customer").Include("CustomerAircraft").Include("Fbo").Where((x => x.Fboid == fboId)).ToListAsync();

            var fuelReqVM = fuelReq.Select(f => new FuelReqsGridViewModel
            {
                Oid = f.Oid,
                ActualPpg = f.ActualPpg,
                ActualVolume = f.ActualVolume,
                Archived = f.Archived,
                Cancelled = f.Cancelled,
                CustomerId = f.CustomerId,
                DateCreated = f.DateCreated,
                DispatchNotes = f.DispatchNotes,
                Eta = f.Eta,
                Etd = f.Etd,
                Icao = f.Icao,
                Notes = f.Notes,
                QuotedPpg = f.QuotedPpg,
                QuotedVolume = f.QuotedVolume,
                Source = f.Source,
                SourceId = f.SourceId,
                TimeStandard = f.TimeStandard,
                CustomerName = f.Customer?.Company,
                TailNumber = f.CustomerAircraft?.TailNumber,
                FboName = f.Fbo?.Fbo
            }).OrderByDescending((x => x.Oid));

            return Ok(fuelReqVM);
        }

        // Get: api/FuelReqs/fbo/5
        [HttpGet("fbo/{fboId}/count")]
        public async Task<IActionResult> GetFuelReqsByFboCount([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UserService.GetClaimedFboId(_HttpContextAccessor) != fboId && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.GroupAdmin)
            {
                return BadRequest("Invalid FBO");
            }

            var fuelReqCount = await GetAllFuelRequests().Include("Fbo").Where((x => x.Fboid == fboId)).CountAsync();

            return Ok(fuelReqCount);
        }

        // POST: api/FuelReqs/analysis/top-customers/fbo/5
        [HttpPost("analysis/top-customers/fbo/{fboId}")]
        public async Task<IActionResult> GetTopCustomersForFbo([FromRoute] int fboId, [FromBody] FuelReqsTopCustomersByFboRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UserService.GetClaimedFboId(_HttpContextAccessor) != fboId && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.GroupAdmin)
            {
                return BadRequest("Invalid FBO");
            }

            var customerFuelReqsByCustomer = await (from orders in (from fr in _context.FuelReq
                    join c in _context.Customers on fr.CustomerId.GetValueOrDefault() equals c.Oid
                    where fr.Fboid == fboId &&
                        fr.DateCreated.HasValue && fr.DateCreated.Value >= request.StartDateTime &&
                        fr.DateCreated.Value <= request.EndDateTime
                    select new
                    {
                        CustomerId = fr.CustomerId,
                        CustomerName = c.Company
                    })
                group orders by new {orders.CustomerId, orders.CustomerName}
                into orderGroup
                select new FuelReqsTopCustomersForFboViewModel
                {
                    CustomerId = orderGroup.Key.CustomerId,
                    CustomerName = orderGroup.Key.CustomerName,
                    TotalOrders = orderGroup.Count()
                })
                .OrderByDescending(x => x.TotalOrders)
                .Take(request.NumberOfResults).AsQueryable().ToListAsync();

            return Ok(customerFuelReqsByCustomer);
        }

        // POST: api/FuelReqs/analysis/total-orders-by-month/fbo/5
        [HttpPost("analysis/total-orders-by-month/fbo/{fboId}")]
        public async Task<IActionResult> GetTotalOrdersByMonthForFbo([FromRoute] int fboId,
            [FromBody] FuelReqsTotalOrdersByMonthForFboRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UserService.GetClaimedFboId(_HttpContextAccessor) != fboId && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.GroupAdmin)
            {
                return BadRequest("Invalid FBO");
            }

            var months = Enumerable.Range(1, 12);
            var years = Enumerable.Range(request.StartDateTime.Year, request.EndDateTime.Year - request.StartDateTime.Year + 1);


            //Average retail prices
            var fboRetailPricesByMonth = (from f in _context.Fboprices
                                          where f.Product.ToLower() == "JetA Retail"
                                                && f.Fboid == fboId
                                                && f.EffectiveFrom >= request.StartDateTime
                                                && f.EffectiveFrom <= request.EndDateTime
                                          group f by new
                                          {
                                              Month = f.EffectiveFrom.GetValueOrDefault().Month,
                                              Year = f.EffectiveFrom.Value.Year
                                          }
                into results
                                          select new
                                          {
                                              Month = results.Key.Month,
                                              Year = results.Key.Year,
                                              AveragePrice = results.Average((x => x.Price.GetValueOrDefault()))
                                          });

            //Average cost prices
            var fboCostPricesByMonth = (from f in _context.Fboprices
                                        where f.Product.ToLower() == "JetA Cost"
                                              && f.Fboid == fboId
                                              && f.EffectiveFrom >= request.StartDateTime
                                              && f.EffectiveFrom <= request.EndDateTime
                                        group f by new
                                        {
                                            Month = f.EffectiveFrom.GetValueOrDefault().Month,
                                            Year = f.EffectiveFrom.Value.Year
                                        }
                into results
                                        select new
                                        {
                                            Month = results.Key.Month,
                                            Year = results.Key.Year,
                                            AveragePrice = results.Average((x => x.Price.GetValueOrDefault()))
                                        });

            //Total orders by month
            var fuelReqsOrdersByMonth = (from f in _context.FuelReq
                                         where f.Fboid == fboId
                                               && f.DateCreated >= request.StartDateTime
                                               && f.DateCreated <= request.EndDateTime
                                         group f by new
                                         {
                                             Month = f.DateCreated.GetValueOrDefault().Month,
                                             Year = f.DateCreated.Value.Year
                                         }
                into results
                                         select new
                                         {
                                             Month = results.Key.Month,
                                             Year = results.Key.Year,
                                             TotalOrders = results.Count()
                                         });

            var fuelReqsTotalOrdersByMonthVM = (from m in months
                join y in years on 1 equals 1
                join retail in fboRetailPricesByMonth
                    on new {Month = m, Year = y} equals new
                    {
                        Month = retail.Month,
                        Year = retail.Year
                    }
                    into leftJoinRetail
                from retail in leftJoinRetail.DefaultIfEmpty()
                join cost in fboCostPricesByMonth on new {Month = m, Year = y} equals new
                    {
                        Month = cost.Month,
                        Year = cost.Year
                    }
                    into leftJoinCost
                from cost in leftJoinCost.DefaultIfEmpty()
                join orders in fuelReqsOrdersByMonth on new {Month = m, Year = y} equals new
                    {
                        Month = orders.Month,
                        Year = orders.Year
                    }
                    into leftJoinOrders
                from orders in leftJoinOrders.DefaultIfEmpty()
                where (m >= request.StartDateTime.Month || y > request.StartDateTime.Year)
                      && (m <= request.EndDateTime.Month || y < request.EndDateTime.Year)
                select new
                {
                    AverageJetACost = cost?.AveragePrice ?? 0,
                    AverageJetARetail = retail?.AveragePrice ?? 0,
                    TotalOrders = orders?.TotalOrders ?? 0,
                    Month = m,
                    Year = y,
                    MonthName = new DateTime(y, m, 1).ToString("MMM", CultureInfo.InvariantCulture) + " " + y.ToString(),
                    AverageJetACostFormatted = (cost?.AveragePrice ?? 0).ToString("C", CultureInfo.CurrentCulture),
                    AverageJetARetailFormatted = (retail?.AveragePrice ?? 0).ToString("C", CultureInfo.CurrentCulture)
                }
            ).OrderBy((x => x.Year)).ThenBy((x => x.Month));

            return Ok(fuelReqsTotalOrdersByMonthVM);
        }

        // POST: api/FuelReqs/analysis/total-orders-by-month/fbo/5
        [HttpPost("analysis/total-orders-by-aircraft-size/fbo/{fboId}")]
        public async Task<IActionResult> GetTotalOrdersByAircraftSizeForFbo([FromRoute] int fboId, [FromBody] FuelReqsTotalOrdersByMonthForFboRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (UserService.GetClaimedFboId(_HttpContextAccessor) != fboId && UserService.GetClaimedRole(_HttpContextAccessor) != Models.User.UserRoles.GroupAdmin)
                {
                    return BadRequest("Invalid FBO");
                }

                //Total orders by aircraft size
                var fuelReqsByAircraftSizeVM = await (from f in _context.FuelReq
                    join ca in
                        (from ca in _context.CustomerAircrafts
                            join ac in _context.Aircrafts on ca.AircraftId equals ac.AircraftId
                            select new
                            {
                                Size = (ca.Size.HasValue && ca.Size.Value != AirCrafts.AircraftSizes.NotSet
                                    ? ca.Size
                                    : (AirCrafts.AircraftSizes) ac.Size),
                                ca.Oid
                            })
                        on f.CustomerAircraftId.GetValueOrDefault() equals ca.Oid
                    where f.Fboid == fboId
                          && f.DateCreated >= request.StartDateTime
                          && f.DateCreated <= request.EndDateTime
                    group f by new
                    {
                        ca.Size
                    }
                    into results
                    select new FuelReqsTotalOrdersByAircraftSizeViewModel
                    {
                        Size = results.Key.Size,
                        TotalOrders = results.Count()
                    }).AsQueryable().ToListAsync();

                return Ok(fuelReqsByAircraftSizeVM);
            }
            catch (System.Exception exception)
            {
                var test = exception;
                return null;
            }
        }

        // POST: api/FuelReqs/analysis/total-orders-by-month/fbo/5
        [HttpPost("analysis/fuelerlinx/orders-by-location")]
        public async Task<IActionResult> GetOrdersByLocation([FromBody] Models.Requests.FuelerLinxUpliftsByLocationRequestContent request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(request.ICAO))
            {
                Models.User user = _context.User.Find(UserService.GetClaimedUserId(_HttpContextAccessor));
                Models.Fboairports airport =
                    await _context.Fboairports.Where(x => x.Fboid == request.FboId).FirstOrDefaultAsync();

                if (airport == null)
                    return NotFound();

                request.ICAO = airport.Icao;
            }

            Services.FuelerLinxService service = new Services.FuelerLinxService();
            var result = await service.GetOrderCountByLocation(request);

            return Ok(result);
        }

        // PUT: api/FuelReqs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuelReq([FromRoute] int id, [FromBody] FuelReq fuelReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fuelReq.Oid)
            {
                return BadRequest();
            }

            _context.Entry(fuelReq).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuelReqExists(id))
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

        // POST: api/FuelReqs
        [HttpPost]
        public async Task<IActionResult> PostFuelReq([FromBody] FuelReq fuelReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FuelReq.Add(fuelReq);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFuelReq", new { id = fuelReq.Oid }, fuelReq);
        }

        // DELETE: api/FuelReqs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuelReq([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fuelReq = await _context.FuelReq.FindAsync(id);
            if (fuelReq == null)
            {
                return NotFound();
            }

            _context.FuelReq.Remove(fuelReq);
            await _context.SaveChangesAsync();

            return Ok(fuelReq);
        }

        private bool FuelReqExists(int id)
        {
            return _context.FuelReq.Any(e => e.Oid == id);
        }

        private IQueryable<FuelReq> GetAllFuelRequests()
        {
            return _context.FuelReq.AsQueryable();
        }
    }
}