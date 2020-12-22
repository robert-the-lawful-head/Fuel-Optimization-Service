using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses;
using FBOLinx.Web.Services;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using IO.Swagger.Model;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Auth;
using Newtonsoft.Json;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FuelReqsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly FuelerLinxService _fuelerLinxService;
        private readonly AircraftService _aircraftService;

        public FuelReqsController(FboLinxContext context, IHttpContextAccessor httpContextAccessor, FuelerLinxService fuelerLinxService, AircraftService aircraftService)
        {
            _fuelerLinxService = fuelerLinxService;
            _context = context;
            _HttpContextAccessor = httpContextAccessor;
            _aircraftService = aircraftService;
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

        // Get: api/FuelReqs/fbo/5
        [HttpGet("fbo/{fboId}")]
        public async Task<IActionResult> GetFuelReqsByFbo([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

        // POST: api/FuelReqs/fbo/5/daterange
        [HttpPost("fbo/{fboId}/daterange")]
        public async Task<IActionResult> GetFuelReqsByFbo([FromRoute] int fboId, [FromBody] FuelReqsByFboAndDateRangeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<FuelReq> fuelReq = await GetAllFuelRequests()
                                            .Include("Customer")
                                            .Include("CustomerAircraft")
                                            .Include("Fbo")
                                            .Where((x => x.Fboid == fboId && x.Eta > request.StartDateTime && x.Eta < request.EndDateTime))
                                            .ToListAsync();

            IOrderedEnumerable<FuelReqsGridViewModel> fuelReqVM = fuelReq.Select(f => new FuelReqsGridViewModel
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
                FboName = f.Fbo?.Fbo,
                Email = f.Email,
                PhoneNumber = f.PhoneNumber
            }).OrderByDescending((x => x.Oid));

            return Ok(fuelReqVM);
        }

        // POST: api/FuelReqs/fbo/5/daterange
        [HttpPost("group/{groupId}/fbo/{fboId}/daterange")]
        public async Task<IActionResult> GetFuelReqsByGroupAndFbo([FromRoute] int groupId, [FromRoute] int fboId, [FromBody] FuelReqsByFboAndDateRangeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Fboairports airport = _context.Fboairports.Where(x => x.Fboid == fboId).FirstOrDefault();
            string fbo = _context.Fbos.Where(f => f.Oid.Equals(fboId)).Select(f => f.Fbo).FirstOrDefault();

            var customers = await (from c in _context.Customers
                                   join ci in _context.CustomerInfoByGroup on c.Oid equals ci.CustomerId
                                   where c.FuelerlinxId > 0 && ci.GroupId == groupId
                                   select new
                                   {
                                       c.FuelerlinxId,
                                       ci.Company
                                   }).ToListAsync();

            FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = _fuelerLinxService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = request.EndDateTime, StartDateTime = request.StartDateTime, Icao = airport.Icao, Fbo = fbo });

            List<FuelReqsGridViewModel> fuelReqsFromFuelerLinx = new List<FuelReqsGridViewModel>();


            foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result)
            {
                transaction.CustomerName = customers.Where(x => x.FuelerlinxId == transaction.CompanyId).Select(x => x.Company).FirstOrDefault();
                fuelReqsFromFuelerLinx.Add(FuelReqsGridViewModel.Cast(transaction));
                
            }

            List<FuelReqsGridViewModel> fuelReqVM = await
                (from fr in _context.FuelReq
                 join c in _context.CustomerInfoByGroup on new { GroupId = groupId, CustomerId = (fr.CustomerId ?? 0) } equals new { c.GroupId, c.CustomerId }
                 join ca in _context.CustomerAircrafts on fr.CustomerAircraftId equals ca.Oid
                 join f in _context.Fbos on fr.Fboid equals f.Oid
                 join frp in _context.FuelReqPricingTemplate on fr.Oid equals frp.FuelReqId
                 into leftJoinedFRP
                 from frp in leftJoinedFRP.DefaultIfEmpty()
                 where fr.Fboid == fboId && fr.Eta > request.StartDateTime && fr.Eta < request.EndDateTime
                 select new FuelReqsGridViewModel
                 {
                     Oid = fr.Oid,
                     ActualPpg = fr.ActualPpg,
                     ActualVolume = fr.ActualVolume,
                     Archived = fr.Archived,
                     Cancelled = fr.Cancelled,
                     CustomerId = fr.CustomerId,
                     DateCreated = fr.DateCreated,
                     DispatchNotes = fr.DispatchNotes,
                     Eta = fr.Eta,
                     Etd = fr.Etd,
                     Icao = fr.Icao,
                     Notes = fr.Notes,
                     QuotedPpg = fr.QuotedPpg,
                     QuotedVolume = fr.QuotedVolume,
                     Source = fr.Source,
                     SourceId = fr.SourceId,
                     TimeStandard = fr.TimeStandard,
                     CustomerName = c == null ? "" : c.Company,
                     TailNumber = ca == null ? "" : ca.TailNumber,
                     FboName = f == null ? "" : f.Fbo,
                     Email = fr.Email,
                     PhoneNumber = fr.PhoneNumber,
                     PricingTemplateName = frp == null ? "" : frp.PricingTemplateName
                 }
                )
                .OrderByDescending(f => f.Oid)
                .ToListAsync();

            fuelReqVM.AddRange(fuelReqsFromFuelerLinx);

            return Ok(fuelReqVM);
        }

        [AllowAnonymous]
        //[APIKey(IntegrationPartners.IntegrationPartnerTypes.Internal)]
        [HttpPost("fbo/{fboId}/create")]
        public async Task<IActionResult> CreateFuelReqByFbo([FromRoute] int fboId, [FromBody] FuelReqRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var fbo = await _context.Fbos.Include(x => x.Group).FirstOrDefaultAsync(x => x.Oid == fboId);

            if (fbo == null)
                return BadRequest("Invalid FBO");

            if (fbo.Group == null || fbo.Group.IsLegacyAccount.GetValueOrDefault())
                return BadRequest("Legacy FBO client.  This FBO does not support API orders yet.");

            var fuelReqsPt =
                            (from c in _context.Customers
                             join cg in _context.CustomerInfoByGroup on
                                 new { CustomerId = c.Oid, c.FuelerlinxId, Active = true }
                                 equals
                                 new { cg.CustomerId, FuelerlinxId = request.CompanyId, Active = cg.Active ?? false }
                             join cct in _context.CustomCustomerTypes on cg.CustomerId equals cct.CustomerId
                             join pt in _context.PricingTemplate on new { cct.CustomerType, Fboid = fboId } equals new { CustomerType = pt.Oid, pt.Fboid }
                             join f in _context.Fbos on
                                 new { cg.GroupId, FboId = fboId, Active = true }
                                 equals
                                 new { GroupId = (f.GroupId ?? 0), FboId = f.Oid, Active = f.Active ?? false }
                             join ca in _context.CustomerAircrafts on
                                 new { TailNumber = request.TailNumber.Trim(), CustomerId = c.Oid, cg.GroupId }
                                 equals
                                 new { TailNumber = ca.TailNumber.Trim(), ca.CustomerId, GroupId = (ca.GroupId ?? 0) }
                             select new
                             {
                                 Fboid = fboId,
                                 CustomerAircraftId = ca.Oid,
                                 request.Eta,
                                 request.Etd,
                                 request.Icao,
                                 request.Notes,
                                 QuotedPpg = request.FuelEstCost,
                                 QuotedVolume = request.FuelEstWeight,
                                 request.TimeStandard,
                                 CustomerId = c.Oid,
                                 DateCreated = DateTime.Now,
                                 Source = "Fuelerlinx",
                                 request.SourceId,
                                 request.Email,
                                 request.PhoneNumber,
                                 PricingTemplate = pt
                             })
                            .Distinct()
                            .ToList();

            var fuelReqs = fuelReqsPt.Select(fr => new FuelReq
            {
                Fboid = fr.Fboid,
                CustomerAircraftId = fr.CustomerAircraftId,
                Eta = fr.Eta,
                Etd = fr.Etd,
                Icao = fr.Icao,
                Notes = fr.Notes,
                QuotedPpg = fr.QuotedPpg,
                QuotedVolume = fr.QuotedVolume,
                TimeStandard = fr.TimeStandard,
                CustomerId = fr.CustomerId,
                DateCreated = fr.DateCreated,
                Source = fr.Source,
                SourceId = fr.SourceId,
                Email = fr.Email,
                PhoneNumber = fr.PhoneNumber,
            }).ToList();

            _context.FuelReq.AddRange(fuelReqs);
            await _context.SaveChangesAsync();

            List<FuelReqPricingTemplate> fuelReqPricingTemplates = new List<FuelReqPricingTemplate>();
            for (int i = 0; i < fuelReqs.Count; i++)
            {
                var pricingTemplate = fuelReqsPt[i].PricingTemplate;
                fuelReqPricingTemplates.Add(new FuelReqPricingTemplate
                {
                    FuelReqId = fuelReqs[i].Oid,
                    PricingTemplateId = pricingTemplate.Oid,
                    PricingTemplateName = pricingTemplate.Name,
                    PricingTemplateRaw = JsonConvert.SerializeObject(new
                    {
                        pricingTemplate.Oid,
                        pricingTemplate.Name,
                        pricingTemplate.Fboid,
                        pricingTemplate.CustomerId,
                        pricingTemplate.Default,
                        pricingTemplate.Notes,
                        pricingTemplate.Email,
                        pricingTemplate.Subject,
                        pricingTemplate.Type,
                        CustomerMargins = _context.CustomerMargins
                            .Where(cm => cm.TemplateId == pricingTemplate.Oid)
                            .Select(cm => new
                            {
                                cm.Oid,
                                cm.PriceTierId,
                                cm.TemplateId,
                                cm.Amount
                            }).ToList()
                    }),
                });
            }
            _context.FuelReqPricingTemplate.AddRange(fuelReqPricingTemplates);
            await _context.SaveChangesAsync();

            return Ok(fuelReqs);
        }

        // Get: api/FuelReqs/fbo/5/count/startdate
        [HttpPost("fbo/{fboId}/count/startdate")]
        public async Task<IActionResult> GetFuelReqsByFboCount([FromRoute] int fboId, [FromBody] FuelReqsByFboAndDateRangeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int fuelReqCount = await GetAllFuelRequests().Include("Fbo").Where((x => x.Fboid == fboId && x.Eta > request.StartDateTime)).CountAsync();

            return Ok(fuelReqCount);
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

        #region Analysis

        [HttpPost("analysis/top-customers/fbo/{fboId}")]
        public async Task<IActionResult> GetTopCustomersForFbo([FromRoute] int fboId, [FromBody] FuelReqsTopCustomersByFboRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var customerFuelReqsByCustomer = await (from orders in (from fr in _context.FuelReq
                                                                        join c in _context.Customers on (fr.CustomerId ?? 0) equals c.Oid
                                                                        where fr.Fboid == fboId &&
                                                                            fr.DateCreated.HasValue && fr.DateCreated.Value >= request.StartDateTime &&
                                                                            fr.DateCreated.Value <= request.EndDateTime
                                                                        select new
                                                                        {
                                                                            fr.CustomerId,
                                                                            CustomerName = c.Company
                                                                        })
                                                        group orders by new { orders.CustomerId, orders.CustomerName }
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
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/total-orders-by-month/fbo/{fboId}")]
        public async Task<IActionResult> GetTotalOrdersByMonthForFbo([FromRoute] int fboId, [FromBody] FuelReqsTotalOrdersByMonthForFboRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                IEnumerable<int> months = Enumerable.Range(1, 12);
                IEnumerable<int> years = Enumerable.Range(request.StartDateTime.Year, request.EndDateTime.Year - request.StartDateTime.Year + 1);


                //Average retail prices
                var fboRetailPricesByMonth = await (from f in _context.Fboprices
                                             where f.Product.ToLower() == "JetA Retail"
                                                   && f.Fboid == fboId
                                                   && f.EffectiveFrom.HasValue
                                                   && f.EffectiveFrom >= request.StartDateTime
                                                   && f.EffectiveFrom <= request.EndDateTime
                                             group f by new
                                             {
                                                 f.EffectiveFrom.Value.Month,
                                                 f.EffectiveFrom.Value.Year
                                             }
                                             into results
                                             select new
                                             {
                                                 results.Key.Month,
                                                 results.Key.Year,
                                                 AveragePrice = results.Average((x => (x.Price ?? 0)))
                                             }).ToListAsync();

                //Average cost prices
                var fboCostPricesByMonth = await (from f in _context.Fboprices
                                           where f.Product.ToLower() == "JetA Cost"
                                                 && f.Fboid == fboId
                                                 && f.EffectiveFrom.HasValue
                                                 && f.EffectiveFrom >= request.StartDateTime
                                                 && f.EffectiveFrom <= request.EndDateTime
                                           group f by new
                                           {
                                               f.EffectiveFrom.Value.Month,
                                               f.EffectiveFrom.Value.Year
                                           }
                                            into results
                                           select new
                                           {
                                               results.Key.Month,
                                               results.Key.Year,
                                               AveragePrice = results.Average((x => (x.Price ?? 0)))
                                           }).ToListAsync();

                //Total orders by month
                var fuelReqsOrdersByMonth = await (from f in _context.FuelReq
                                            where f.Fboid == fboId
                                                  && f.DateCreated.HasValue
                                                  && f.DateCreated >= request.StartDateTime
                                                  && f.DateCreated <= request.EndDateTime
                                            group f by new
                                            {
                                                f.DateCreated.Value.Month,
                                                f.DateCreated.Value.Year
                                            }
                                             into results
                                            select new
                                            {
                                                results.Key.Month,
                                                results.Key.Year,
                                                TotalOrders = results.Count()
                                            }).ToListAsync();

                var fuelReqsTotalOrdersByMonthVM = (from m in months
                                                    join y in years on 1 equals 1
                                                    join retail in fboRetailPricesByMonth
                                                        on new { Month = m, Year = y } equals new
                                                        {
                                                            retail.Month,
                                                            retail.Year
                                                        }
                                                    into leftJoinRetail
                                                    from retail in leftJoinRetail.DefaultIfEmpty()
                                                    join cost in fboCostPricesByMonth on new { Month = m, Year = y } equals new
                                                    {
                                                        cost.Month,
                                                        cost.Year
                                                    }
                                                    into leftJoinCost
                                                    from cost in leftJoinCost.DefaultIfEmpty()
                                                    join orders in fuelReqsOrdersByMonth on new { Month = m, Year = y } equals new
                                                    {
                                                        orders.Month,
                                                        orders.Year
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
                                                    }).OrderBy((x => x.Year)).ThenBy((x => x.Month));

                return Ok(fuelReqsTotalOrdersByMonthVM);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/total-orders-by-aircraft-size/fbo/{fboId}")]
        public async Task<IActionResult> GetTotalOrdersByAircraftSizeForFbo([FromRoute] int fboId, [FromBody] FuelReqsTotalOrdersByMonthForFboRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Total orders by aircraft size
                var fuelReqsByAircraftSizeVM = await (from f in _context.FuelReq
                                                      join ca in
                                                          (from ca in _context.CustomerAircrafts
                                                           join ac in _aircraftService.GetAllAircraftsAsQueryable() on ca.AircraftId equals ac.AircraftId
                                                           select new
                                                           {
                                                               Size = (ca.Size.HasValue && ca.Size.Value != AirCrafts.AircraftSizes.NotSet
                                                                ? ca.Size
                                                                : (AirCrafts.AircraftSizes)ac.Size),
                                                               ca.Oid
                                                           }) on (f.CustomerAircraftId ?? 0) equals ca.Oid
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
            catch (Exception exception)
            {
                var test = exception;
                return null;
            }
        }

        [HttpPost("analysis/fuelerlinx/orders-by-location")]
        public async Task<IActionResult> GetOrdersByLocation([FromBody] FuelerLinxUpliftsByLocationRequestContent request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (string.IsNullOrEmpty(request.ICAO))
                {
                    User user = _context.User.Find(UserService.GetClaimedUserId(_HttpContextAccessor));
                    Fboairports airport =
                        await _context.Fboairports.Where(x => x.Fboid == request.FboId).FirstOrDefaultAsync();

                    if (airport == null)
                        return NotFound();

                    request.ICAO = airport.Icao;
                }

                var result = _fuelerLinxService.GetTransactionsCountForAirport(new FBOLinxOrdersRequest()
                { EndDateTime = request.EndDateTime, StartDateTime = request.StartDateTime, Icao = request.ICAO });

                var response = new { totalOrders = result.Result, icao = request.ICAO };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/quotes-orders-over-time/fbo/{fboId}")]
        public async Task<IActionResult> GetQuotesAndOrdersWonChartData([FromRoute] int fboId, [FromBody] FuelerLinxUpliftsByLocationRequestContent request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(request.ICAO))
            {
                User user = _context.User.Find(UserService.GetClaimedUserId(_HttpContextAccessor));
                Fboairports airport = _context.Fboairports.Where(x => x.Fboid == fboId).FirstOrDefault();

                if (airport == null)
                    return NotFound();

                request.ICAO = airport.Icao;
            }

            try
            {
                IEnumerable<int> months = Enumerable.Range(1, 12);
                IEnumerable<int> years = Enumerable.Range(request.StartDateTime.Year, request.EndDateTime.Year - request.StartDateTime.Year + 1);

                List<FuelReqForChart> fuelReqs = await GetFuelRequestsByMonthDateRange(fboId, request.StartDateTime, request.EndDateTime);

                List<NgxChartItemType> fuelReqsByMonth = (from month in months
                                                          join year in years on 1 equals 1
                                                          join f in fuelReqs on new { Month = month, Year = year } equals new { f.Month, f.Year }
                                                          into leftJoinFuelReqs
                                                          from f in leftJoinFuelReqs.DefaultIfEmpty()
                                                          where string.Compare(year.ToString() + month.ToString().PadLeft(2), request.StartDateTime.Year.ToString() + request.StartDateTime.Month.ToString().PadLeft(2)) >= 0
                                                          && string.Compare(year.ToString() + month.ToString().PadLeft(2), request.EndDateTime.Year.ToString() + request.EndDateTime.Month.ToString().PadLeft(2)) <= 0
                                                          select new NgxChartItemType
                                                          {
                                                              Month = month,
                                                              Year = year,
                                                              Name = month + "/" + year,
                                                              Value = f?.TotalOrders ?? 0
                                                          })
                                      .OrderBy(x => x.Year)
                                      .ThenBy(x => x.Month)
                                      .ToList();

                FuelerLinxUpliftsByLocationResponseContent quotes = await _fuelerLinxService.GetOrderCountByLocation(request);

                List<NgxChartItemType> quotesByMonth = (from month in months
                                                        join year in years on 1 equals 1
                                                        join q in quotes.TotalOrdersByMonth on new { Month = month, Year = year } equals new { q.Month, q.Year }
                                                        into leftJoinQuotes
                                                        from q in leftJoinQuotes.DefaultIfEmpty()
                                                        where string.Compare(year.ToString() + month.ToString().PadLeft(2), request.StartDateTime.Year.ToString() + request.StartDateTime.Month.ToString().PadLeft(2)) >= 0
                                                          && string.Compare(year.ToString() + month.ToString().PadLeft(2), request.EndDateTime.Year.ToString() + request.EndDateTime.Month.ToString().PadLeft(2)) <= 0
                                                        select new NgxChartItemType
                                                        {
                                                            Month = month,
                                                            Year = year,
                                                            Name = month + "/" + year,
                                                            Value = q?.Count ?? 0
                                                        })
                                     .OrderBy(x => x.Year)
                                     .ThenBy(x => x.Month)
                                     .ToList();

                List<NgxChartItemType> dollarVolumeByMonth = (from month in months
                                                              join year in years on 1 equals 1
                                                              join f in fuelReqs on new { Month = month, Year = year } equals new { f.Month, f.Year }
                                                              into leftJoinFuelReqs
                                                              from f in leftJoinFuelReqs.DefaultIfEmpty()
                                                              where string.Compare(year.ToString() + month.ToString().PadLeft(2), request.StartDateTime.Year.ToString() + request.StartDateTime.Month.ToString().PadLeft(2)) >= 0
                                                              && string.Compare(year.ToString() + month.ToString().PadLeft(2), request.EndDateTime.Year.ToString() + request.EndDateTime.Month.ToString().PadLeft(2)) <= 0
                                                              select new NgxChartItemType
                                                              {
                                                                  Month = month,
                                                                  Year = year,
                                                                  Name = month + "/" + year,
                                                                  Value = f?.TotalSum ?? 0
                                                              })
                                           .OrderBy(x => x.Year)
                                           .ThenBy(x => x.Month)
                                           .ToList();

                List<List<NgxChartDataType>> chartData = new List<List<NgxChartDataType>>()
                            {
                                new List<NgxChartDataType>(){
                                    new NgxChartDataType
                                    {
                                        Name = "Orders",
                                        Series = fuelReqsByMonth
                                    },
                                    new NgxChartDataType
                                    {
                                        Name = "Quotes",
                                        Series = quotesByMonth
                                    }
                                },
                                new List<NgxChartDataType>(){
                                    new NgxChartDataType
                                    {
                                        Name = "Sum of Dollar Volume",
                                        Series = dollarVolumeByMonth
                                    }
                                }
                            };

                return Ok(chartData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/orders-won-over-time/fbo/{fboId}")]
        public async Task<IActionResult> GetOrdersWonChartDataAsync([FromRoute] int fboId, [FromBody] FuelerLinxUpliftsByLocationRequestContent request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (string.IsNullOrEmpty(request.ICAO))
            {
                User user = _context.User.Find(UserService.GetClaimedUserId(_HttpContextAccessor));
                Fboairports airport = _context.Fboairports.Where(x => x.Fboid == fboId).FirstOrDefault();

                if (airport == null)
                    return NotFound();

                request.ICAO = airport.Icao;
            }

            try
            {
                IEnumerable<int> months = Enumerable.Range(1, 12);
                IEnumerable<int> years = Enumerable.Range(request.StartDateTime.Year, request.EndDateTime.Year - request.StartDateTime.Year + 1);

                List<FuelReqForChart> fuelReqs = await GetFuelRequestsByMonthDateRange(fboId, request.StartDateTime, request.EndDateTime);

                List<NgxChartItemType> fuelReqsByMonth = (from month in months
                                                          join year in years on 1 equals 1
                                                          join f in fuelReqs on new { Month = month, Year = year } equals new { f.Month, f.Year }
                                                          into leftJoinFuelReqs
                                                          from f in leftJoinFuelReqs.DefaultIfEmpty()
                                                          where string.Compare(year.ToString() + month.ToString().PadLeft(2), request.StartDateTime.Year.ToString() + request.StartDateTime.Month.ToString().PadLeft(2)) >= 0
                                                          && string.Compare(year.ToString() + month.ToString().PadLeft(2), request.EndDateTime.Year.ToString() + request.EndDateTime.Month.ToString().PadLeft(2)) <= 0
                                                          select new NgxChartItemType
                                                          {
                                                              Month = month,
                                                              Year = year,
                                                              Name = month + "/" + year,
                                                              Value = f?.TotalOrders ?? 0
                                                          })
                                      .OrderBy(x => x.Year)
                                      .ThenBy(x => x.Month)
                                      .ToList();

                return Ok(fuelReqsByMonth);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/volumes-nearby-airport/fbo/{fboId}")]
        public IActionResult GetCountOfOrderVolumesNearByAirport([FromRoute] int fboId, [FromBody] FBOLinxNearbyAirportsRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string icao = _context.Fboairports.Where(f => f.Fboid.Equals(fboId)).Select(f => f.Icao).FirstOrDefault();
                request.Icao = icao;

                List<FBOLinxNearbyAirportsModel> volumes = _fuelerLinxService.GetTransactionsForNearbyAirports(request).Result;
                IEnumerable<NgxChartBarChartItemType> result = volumes.Select(f => new NgxChartBarChartItemType
                {
                    Name = f.Icao,
                    Value = f.AirportsCount.GetValueOrDefault()
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/market-share-airport/fbo/{fboId}")]
        public async Task<IActionResult> GetMarketShareAtAirport([FromRoute] int fboId, [FromBody] FBOLinxOrdersRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string icao = _context.Fboairports.Where(f => f.Fboid.Equals(fboId)).Select(f => f.Icao).FirstOrDefault();
                List<int> otherFbos = _context.Fboairports.Where(f => f.Icao.Equals(icao)).Select(f => f.Fboid).ToList();

                request.Icao = icao;

                int fboOrderCount = await _context.FuelReq
                    .CountAsync(f => f.Fboid.Equals(fboId) && f.Etd >= request.StartDateTime && f.Etd < request.EndDateTime.GetValueOrDefault().AddDays(1));

                int directOrdersCount = _fuelerLinxService.GetTransactionsDirectOrdersCount(request).Result.GetValueOrDefault();

                int fuelerlinxOrdersCount = _fuelerLinxService.GetTransactionsCountForAirport(request).Result.GetValueOrDefault();

                List<NgxChartBarChartItemType> chartData = new List<NgxChartBarChartItemType>()
                            {
                                new NgxChartBarChartItemType
                                {
                                    Name = "My Total Orders",
                                    Value = fboOrderCount
                                },
                                new NgxChartBarChartItemType
                                {
                                    Name = "Other FBO's Orders",
                                    Value = (directOrdersCount - fboOrderCount)
                                },
                                new NgxChartBarChartItemType
                                {
                                    Name = "Contract/Reseller Orders",
                                    Value = (fuelerlinxOrdersCount - directOrdersCount)
                                }
                            };
                return Ok(chartData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/fbo-fuel-vendor-sources/fbo/{fboId}")]
        public async Task<IActionResult> GetFBOFuelVendorSources([FromRoute] int fboId, [FromBody] FBOLinxOrdersRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string icao = _context.Fboairports.Where(f => f.Fboid.Equals(fboId)).Select(f => f.Icao).FirstOrDefault();

                request.Icao = icao;

                int fboOrderCount = await _context.FuelReq
                    .CountAsync(f => f.Fboid.Equals(fboId) && f.Etd >= request.StartDateTime && f.Etd < request.EndDateTime.GetValueOrDefault().AddDays(1));

                string fbo = await _context.Fbos.Where(f => f.Oid.Equals(fboId)).Select(f => f.Fbo).FirstOrDefaultAsync();
                request.Fbo = fbo;

                List<FbolinxContractFuelVendorTransactionsCountAtAirport> fuelerlinxContractFuelVendorOrdersCount = _fuelerLinxService.GetContractFuelVendorsTransactionsCountForAirport(request).Result;

                List<NgxChartBarChartItemType> chartData = new List<NgxChartBarChartItemType>()
                            {
                                new NgxChartBarChartItemType
                                {
                                    Name = "Directs",
                                    Value = fboOrderCount
                                }
                            };

                foreach (FbolinxContractFuelVendorTransactionsCountAtAirport vendor in fuelerlinxContractFuelVendorOrdersCount)
                {
                    if (vendor.ContractFuelVendor != null && !vendor.ContractFuelVendor.ToLower().Contains("fbolinx") && !vendor.ContractFuelVendor.Contains(fbo) && !vendor.ContractFuelVendor.Contains(" - " + icao))
                    {
                        NgxChartBarChartItemType chartItemType = new NgxChartBarChartItemType();
                        chartItemType.Name = vendor.ContractFuelVendor;
                        chartItemType.Value = vendor.TransactionsCount.GetValueOrDefault();
                        chartData.Add(chartItemType);
                    }
                }

                return Ok(chartData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/market-share-fbos-airport/fbo/{fboId}")]
        public async Task<IActionResult> GetMarketShareFBOsAtAirport([FromRoute] int fboId, [FromBody] FBOLinxOrdersRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string icao = await _context.Fboairports.Where(f => f.Fboid.Equals(fboId)).Select(f => f.Icao).FirstOrDefaultAsync();

                request.Icao = icao;

                string fbo = await _context.Fbos.Where(f => f.Oid.Equals(fboId)).Select(f => f.Fbo).FirstOrDefaultAsync();
                request.Fbo = fbo;

                FboLinxFbosTransactionsCountResponse fuelerlinxFBOsOrdersCount = _fuelerLinxService.GetFBOsTransactionsCountForAirport(request);

                List<NgxChartBarChartItemType> chartData = new List<NgxChartBarChartItemType>();

                int i = 1;
                foreach (GroupedTransactionCountByFBOAtAirport vendor in fuelerlinxFBOsOrdersCount.Result)
                {
                    if (vendor.Fbo == "Competitor FBO")
                    {
                        NgxChartBarChartItemType chartItemType = new NgxChartBarChartItemType();
                        chartItemType.Name = vendor.Fbo + " " + i;
                        chartItemType.Value = vendor.Count.GetValueOrDefault();
                        chartData.Add(chartItemType);
                        i++;
                    }
                    else
                    {
                        NgxChartBarChartItemType chartItemType = new NgxChartBarChartItemType();
                        chartItemType.Name = vendor.Fbo;
                        chartItemType.Value = vendor.Count.GetValueOrDefault();
                        chartData.Add(chartItemType);
                    }
                }

                return Ok(chartData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/customers-breakdown/fbo/{fboId}")]
        public async Task<IActionResult> GetCustomersBreakdown([FromRoute] int fboId, [FromBody] FBOLinxOrdersRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var chartData = await (from fr in (
                                     from fr in _context.FuelReq
                                     where fr.Fboid.Equals(fboId) && fr.Etd >= request.StartDateTime && fr.Etd <= request.EndDateTime
                                     group fr by new
                                     {
                                         CustomerID = fr.CustomerId,
                                         FboId = fr.Fboid
                                     }
                                     into groupedFuelReqs
                                     select new
                                     {
                                         groupedFuelReqs.Key.CustomerID,
                                         Volume = groupedFuelReqs.Sum(fr => (fr.ActualVolume ?? 0) * (fr.ActualPpg ?? 0) > 0 ?
                                                 fr.ActualVolume * fr.ActualPpg :
                                                 (fr.QuotedVolume ?? 0) * (fr.QuotedPpg ?? 0)),
                                         Orders = groupedFuelReqs.Count(),
                                         FboId = groupedFuelReqs.Key.FboId
                                     }
                                )
                                       join f in _context.Fbos on fr.FboId equals f.Oid
                                       join c in _context.CustomerInfoByGroup on (fr.CustomerID ?? 0) equals c.CustomerId
                                       where f.GroupId == c.GroupId
                                       select new
                                       {
                                           Name = c.Company,
                                           fr.Volume,
                                           fr.Orders
                                       })
                                .Distinct()
                                .ToListAsync();
                return Ok(chartData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/company-quoting-deal-statistics/group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetCompanyStatistics([FromRoute] int groupId, [FromRoute] int fboId, [FromBody] FuelReqsCompanyStatisticsRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string icao = await _context.Fboairports.Where(f => f.Fboid.Equals(fboId)).Select(f => f.Icao).FirstOrDefaultAsync();
                List<int> airportfbos = await _context.Fboairports.Where(f => f.Icao.Equals(icao)).Select(f => f.Fboid).ToListAsync();

                var fuelReqs = await (from fr in _context.FuelReq
                                      where fr.Etd >= request.StartDateTime && fr.Etd <= request.EndDateTime && airportfbos.Any(a => a == fr.Fboid)
                                      group fr by fr.CustomerId
                                into groupedFuelReqs
                                      select new
                                      {
                                          CustomerId = groupedFuelReqs.Key,
                                          TotalOrders = groupedFuelReqs.Count(),
                                          TotalVolume = groupedFuelReqs.Sum(fr => (fr.ActualVolume ?? 0) * (fr.ActualPpg ?? 0) > 0 ?
                                                       fr.ActualVolume * fr.ActualPpg :
                                                       (fr.QuotedVolume ?? 0) * (fr.QuotedPpg ?? 0)) ?? 0
                                      }).ToListAsync();

                var pricingLogs = await (from cpl in _context.CompanyPricingLog
                                         join c in _context.Customers on cpl.CompanyId equals c.FuelerlinxId
                                         join cibg in _context.CustomerInfoByGroup on c.Oid equals cibg.CustomerId
                                         where cibg.GroupId == groupId && cpl.ICAO == icao
                                         select new
                                         {
                                             cibg.CustomerId,
                                             cpl.CreatedDate
                                         }).ToListAsync();

                var customers = await _context.CustomerInfoByGroup
                                        .Where(c => c.GroupId.Equals(groupId))
                                        .Include(x => x.Customer)
                                        .Where(x => (x.Customer != null && x.Customer.Suspended != true))
                                        .Select(c => new
                                        {
                                            c.CustomerId,
                                            Company = c.Company.Trim(),
                                            Customer = c.Customer
                                        })
                                        .Distinct().ToListAsync();

                FBOLinxOrdersRequest fbolinxOrdersRequest = new FBOLinxOrdersRequest();
                fbolinxOrdersRequest.StartDateTime = request.StartDateTime;
                fbolinxOrdersRequest.EndDateTime = request.EndDateTime;
                fbolinxOrdersRequest.Icao = icao;

                List<FbolinxCustomerTransactionsCountAtAirport> fuelerlinxCustomerOrdersCount = _fuelerLinxService.GetCustomerTransactionsCountForAirport(fbolinxOrdersRequest).Result;

                string fbo = await _context.Fbos.Where(f => f.Oid.Equals(fboId)).Select(f => f.Fbo).FirstOrDefaultAsync();
                fbolinxOrdersRequest.Fbo = fbo;
                List<FbolinxCustomerTransactionsCountAtAirport> fuelerlinxCustomerFBOOrdersCount = _fuelerLinxService.GetCustomerFBOTransactionsCount(fbolinxOrdersRequest).Result;


                List<object> tableData = new List<object>();
                foreach (var customer in customers)
                {
                    //var fuelerLinxCustomerID = _context.Customers.Where(c => c.Oid.Equals(customer.CustomerId)).Select(c => c.FuelerlinxId).FirstOrDefault();
                    var fuelerLinxCustomerID = customer.Customer?.FuelerlinxId;
                    var selectedCompanyFuelReqs = fuelReqs.Where(f => f.CustomerId.Equals(customer.CustomerId)).FirstOrDefault();
                    var companyQuotes = pricingLogs.Where(c => c.CreatedDate >= request.StartDateTime && c.CreatedDate <= request.EndDateTime && c.CustomerId.Equals(customer.CustomerId)).Count();
                    var companyPricingLog = pricingLogs.Where(c => c.CustomerId.Equals(customer.CustomerId)).OrderByDescending(c => c.CreatedDate).FirstOrDefault();
                    var totalOrders = fuelerlinxCustomerFBOOrdersCount.Where(c => c.FuelerLinxCustomerId == fuelerLinxCustomerID).Select(f => f.TransactionsCount).FirstOrDefault();
                    var airportTotalOrders = fuelerlinxCustomerOrdersCount.Where(c => c.FuelerLinxCustomerId == fuelerLinxCustomerID).Select(f => f.TransactionsCount).FirstOrDefault();

                    tableData.Add(new
                    {
                        customer.Company,
                        CompanyQuotesTotal = companyQuotes,
                        DirectOrders = selectedCompanyFuelReqs == null ? 0 : selectedCompanyFuelReqs.TotalOrders,
                        ConversionRate = selectedCompanyFuelReqs == null || companyQuotes == 0 ? 0 : Math.Round(decimal.Parse(selectedCompanyFuelReqs.TotalOrders.ToString()) / decimal.Parse(companyQuotes.ToString()) * 100, 2),
                        TotalOrders = totalOrders == null ? 0 : totalOrders,
                        AirportOrders = airportTotalOrders == null ? 0 : airportTotalOrders,
                        LastPullDate = companyPricingLog == null ? "N/A" : companyPricingLog.CreatedDate.ToString(),
                        airportICAO = icao
                    });
                }

                return Ok(tableData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion

        #region Common methods

        private bool FuelReqExists(int id)
        {
            return _context.FuelReq.Any(e => e.Oid == id);
        }

        private IQueryable<FuelReq> GetAllFuelRequests()
        {
            return _context.FuelReq.AsQueryable();
        }

        private async Task<List<FuelReqForChart>> GetFuelRequestsByMonthDateRange(int fboId, DateTime startDateTime, DateTime endDateTime)
        {
            List<FuelReqForChart> fuelReqs = await
                           (from f in (
                               from f in _context.FuelReq
                               where f.Fboid.Equals(fboId) && f.Etd.HasValue && f.Etd >= startDateTime && f.Etd <= endDateTime
                               select new
                               {
                                   f.Etd,
                                   Sum = (f.ActualVolume ?? 0) * (f.ActualPpg ?? 0) > 0 ? f.ActualVolume * f.ActualPpg : (f.QuotedVolume ?? 0) * (f.QuotedPpg ?? 0)
                               }
                           )
                            group f by new
                            {
                                f.Etd.Value.Month,
                                f.Etd.Value.Year,
                                f.Sum
                            }
                           into results
                            select new FuelReqForChart
                            {
                                Month = results.Key.Month,
                                Year = results.Key.Year,
                                TotalOrders = results.Count(),
                                TotalSum = results.Sum(x => x.Sum)
                            }).ToListAsync();

            return fuelReqs;
        }

        #endregion
    }
}
