using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.Requests.Integrations.FuelerLinx;
using FBOLinx.ServiceLayer.DTO.Responses.Integrations.FuelerLinx;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Auth;
using Newtonsoft.Json;
using Fuelerlinx.SDK;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.DTO.Requests.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Responses.AirportWatch;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.Demo;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FuelReqsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly FuelerLinxApiService _fuelerLinxService;
        private readonly AircraftService _aircraftService;
        private readonly AirportFboGeofenceClustersService _airportFboGeofenceClustersService;
        private readonly IFboService _fboService;
        private readonly AirportWatchService _airportWatchService;
        private readonly IFuelReqService _fuelReqService;
        private readonly IDemoFlightWatch _demoFlightWatch;


        public FuelReqsController(FboLinxContext context, IHttpContextAccessor httpContextAccessor, FuelerLinxApiService fuelerLinxService, AircraftService aircraftService, 
            AirportFboGeofenceClustersService airportFboGeofenceClustersService, IFboService fboService, AirportWatchService airportWatchService, IFuelReqService fuelReqService, IDemoFlightWatch demoFlightWatch)
        {
            _fuelerLinxService = fuelerLinxService;
            _context = context;
            _HttpContextAccessor = httpContextAccessor;
            _aircraftService = aircraftService;
            _airportFboGeofenceClustersService = airportFboGeofenceClustersService;
            _fboService = fboService;
            _airportWatchService = airportWatchService;
            _fuelReqService = fuelReqService;
            _demoFlightWatch = demoFlightWatch;
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

        
        // POST: api/FuelReqs/group/3/fbo/5/daterange
        [HttpPost("group/{groupId}/fbo/{fboId}/daterange")]
        public async Task<IActionResult> GetFuelReqsByGroupAndFbo([FromRoute] int groupId, [FromRoute] int fboId, [FromBody] FuelReqsByFboAndDateRangeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _fuelReqService.GetDirectAndContractOrdersByGroupAndFbo(groupId, fboId, request.StartDateTime, request.EndDateTime);

            if (_demoFlightWatch.isDemoDataVisibleByFboId(fboId))
            {
                result.Add(_demoFlightWatch.GetFuelReqDemo());
            }

            return Ok(result);
        }


        [AllowAnonymous]
        [HttpPut("canceluncancel/id/{id}")]
        public async Task<IActionResult> CancelUnCancelFuelRequest([FromRoute] int id)
        {
            var fuelReq = await _context.FuelReq.Where(f => f.Oid == id).FirstOrDefaultAsync();

            if (fuelReq != null)
            {
                if (fuelReq.Cancelled == null)
                    fuelReq.Cancelled = false;

                fuelReq.Cancelled = !fuelReq.Cancelled;
                await _context.SaveChangesAsync();
            }

            return Ok(fuelReq);
        }

        [AllowAnonymous]
        [HttpPut("updatedatetimes/id/{id}")]
        public async Task<IActionResult> UpdateDateTimes([FromRoute] int id, [FromBody] FuelReqUpdateDateTimesRequest request)
        {
            var fuelReq = await _context.FuelReq.Where(f => f.Oid == id).FirstOrDefaultAsync();
            fuelReq.Eta = request.Eta;
            fuelReq.Etd = request.Etd;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [AllowAnonymous]
        [APIKey(Core.Enums.IntegrationPartnerTypes.Internal)]
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

            var fuelReqs = _context.FuelReq.Where(f => f.SourceId > 0 && f.SourceId == request.SourceId).ToList();

            if (fuelReqs.Count == 0)
            {
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
                                     Source = "FuelerLinx",
                                     request.SourceId,
                                     request.Email,
                                     request.PhoneNumber,
                                     PricingTemplate = pt,
                                     request.FuelOn
                                 })
                                .Distinct()
                                .ToList();

                fuelReqs = fuelReqsPt.Select(fr => new FuelReq
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
                    FuelOn = fr.FuelOn,
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
            }
            else
            {
                fuelReqs[0].Eta = request.Eta;
                fuelReqs[0].Etd = request.Etd;
                fuelReqs[0].Notes = request.Notes;
                fuelReqs[0].QuotedPpg = request.FuelEstCost;
                fuelReqs[0].QuotedVolume = request.FuelEstWeight;
                fuelReqs[0].TimeStandard = request.TimeStandard;
                fuelReqs[0].Email = request.Email;
                fuelReqs[0].PhoneNumber = request.PhoneNumber;
                fuelReqs[0].FuelOn = request.FuelOn;
            }
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

            int fuelReqCount = await GetAllFuelRequests().Include("Fbo").Where((x => x.Fboid == fboId && (x.Cancelled == null || x.Cancelled == false) && x.Eta > request.StartDateTime)).CountAsync();

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
                                                                            (fr.Cancelled == null || fr.Cancelled == false) &&
                                                                            fr.Etd.HasValue && fr.Etd.Value >= request.StartDateTime &&
                                                                            fr.Etd.Value <= request.EndDateTime
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
                                                  && (f.Cancelled == null || f.Cancelled == false)
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
                                                               Size = (ca.Size.HasValue && ca.Size.Value != Core.Enums.AircraftSizes.NotSet
                                                                ? ca.Size
                                                                : (Core.Enums.AircraftSizes)ac.Size),
                                                               Oid = ca.Oid
                                                           }) on (f.CustomerAircraftId ?? 0) equals ca.Oid
                                                      where f.Fboid == fboId
                                                            && (f.Cancelled == null || f.Cancelled == false) 
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
                    User user = _context.User.Find(JwtManager.GetClaimedUserId(_HttpContextAccessor));
                    Fboairports airport =
                        await _context.Fboairports.Where(x => x.Fboid == request.FboId).FirstOrDefaultAsync();

                    if (airport == null)
                        return NotFound();

                    request.ICAO = airport.Icao;
                }

                var result = await _fuelerLinxService.GetTransactionsCountForAirport(new FBOLinxOrdersRequest()
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
                User user = _context.User.Find(JwtManager.GetClaimedUserId(_HttpContextAccessor));
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
                User user = _context.User.Find(JwtManager.GetClaimedUserId(_HttpContextAccessor));
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
        public async Task<IActionResult> GetCountOfOrderVolumesNearByAirport([FromRoute] int fboId, [FromBody] FBOLinxNearbyAirportsRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string icao = _context.Fboairports.Where(f => f.Fboid.Equals(fboId)).Select(f => f.Icao).FirstOrDefault();
                request.Icao = icao;

                FBOLinxNearbyAirportsResponse response = await _fuelerLinxService.GetTransactionsForNearbyAirports(request);
                ICollection<FBOLinxNearbyAirportsModel> volumes = response.Result;

                IEnumerable<NgxChartBarChartItemType> result = volumes.Select(f => new NgxChartBarChartItemType
                {
                    Name = f.Icao,
                    Value = f.AirportsCount
                });
                return Ok(result);
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
                    .CountAsync(f => f.Fboid.Equals(fboId) && (f.Cancelled == null || f.Cancelled == false) && f.Etd >= request.StartDateTime && f.Etd < request.EndDateTime.AddDays(1));

                string fbo = await _context.Fbos.Where(f => f.Oid.Equals(fboId)).Select(f => f.Fbo).FirstOrDefaultAsync();
                request.Fbo = fbo;

                FboLinxContractFuelVendorsCountResponse response = await _fuelerLinxService.GetContractFuelVendorsTransactionsCountForAirport(request);
                ICollection<FbolinxContractFuelVendorTransactionsCountAtAirport> fuelerlinxContractFuelVendorOrdersCount = response.Result;


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
                        chartItemType.Value = vendor.TransactionsCount;
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

        [HttpPost("analysis/fbo-fuel-vendor-sources/group/{groupId}")]
        public async Task<ActionResult<List<FBOLinxOrdersForMultipleAirportsResponse>>> GetFuelVendorSourcesByAirports([FromRoute] int groupId, [FromBody] FBOLinxOrdersForMultipleAirportsRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var locations = await (from f in _context.Fbos
                                   join fa in _context.Fboairports on f.Oid equals fa.Fboid
                                   where f.GroupId == groupId
                                   select new
                                   {
                                       fa.Icao,
                                       f.Fbo,
                                       f.Oid,
                                   }).ToListAsync();

                foreach (var location in locations)
                {
                    if (!request.IcaosFbos.ContainsKey(location.Icao))
                        request.IcaosFbos.Add(location.Icao, location.Fbo);
                }

                var fbosOrders = await (from fr in _context.FuelReq
                                        join f in _context.Fbos on fr.Fboid equals f.Oid
                                        where f.GroupId == groupId && (fr.Cancelled == null || fr.Cancelled == false) && fr.Etd >= request.StartDateTime && fr.Etd < request.EndDateTime.AddDays(1)
                                        group fr by new
                                        {
                                            f.Oid,
                                            f.Fbo,
                                        } into groupedResult
                                        select new
                                        {
                                            groupedResult.Key.Oid,
                                            groupedResult.Key.Fbo,
                                            DirectOrders = groupedResult.Count()
                                        }).ToListAsync();

                FboLinxContractFuelVendorsCountsByAirportsResponse response = await _fuelerLinxService.GetContractFuelVendorsTransactionsCountByAirports(request);
                ICollection<FbolinxContractFuelVendorTransactionsCountByAirport> fuelerlinxContractFuelVendorOrdersCount = response.Result;

                var result = locations.Select(fbo =>
                {
                    return new FBOLinxOrdersForMultipleAirportsResponse
                    {
                        DirectOrders = fbosOrders.Where(fr => fr.Oid == fbo.Oid).Select(s=>s.DirectOrders).FirstOrDefault(),
                        Icao = fbo.Icao,
                        VendorOrders = fuelerlinxContractFuelVendorOrdersCount.Where(f => f.Icao == fbo.Icao).ToList()
                    };
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/market-share-fbo-airport/fbo/{fboId}")]
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

                FboLinxFbosTransactionsCountResponse fuelerlinxFBOsOrdersCount = await _fuelerLinxService.GetFBOsTransactionsCountForAirport(request);

                List<NgxChartBarChartItemType> chartData = new List<NgxChartBarChartItemType>();

                int i = 1;
                foreach (GroupedTransactionCountByFBOAtAirport vendor in fuelerlinxFBOsOrdersCount.Result)
                {
                    if (vendor.Fbo == "Competitor FBO")
                    {
                        NgxChartBarChartItemType chartItemType = new NgxChartBarChartItemType();
                        chartItemType.Name = vendor.Fbo + " " + i;
                        chartItemType.Value = vendor.Count;
                        chartData.Add(chartItemType);
                        i++;
                    }
                    else
                    {
                        NgxChartBarChartItemType chartItemType = new NgxChartBarChartItemType();
                        chartItemType.Name = vendor.Fbo;
                        chartItemType.Value = vendor.Count;
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

        [HttpPost("analysis/market-share-fbos-airports/group/{groupId}")]
        public async Task<IActionResult> GetMarketShareFBOsByAirports([FromRoute] int groupId, [FromBody] FBOLinxGroupOrdersRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var icaos = await (from f in _context.Fbos
                                   join fa in _context.Fboairports on f.Oid equals fa.Fboid
                                   where f.GroupId == groupId
                                   select new
                                   {
                                       fa.Icao,
                                       f.Fbo,
                                   }).ToListAsync();

                request.Icaos = icaos.Select(f => f.Icao).ToList();
                request.Fbos = icaos.Select(f => f.Fbo).Distinct().ToList();

                FBOLinxGroupOrdersResponse fuelerlinxFBOsOrdersCount = await _fuelerLinxService.GetTransactionsCountForFbosAndAirports(request);

                var yourOrderCount = await (from fr in _context.FuelReq
                                            join fa in _context.Fboairports on fr.Fboid equals fa.Fboid
                                            join f in _context.Fbos on fa.Fboid equals f.Oid
                                            where request.Icaos.Contains(fa.Icao) && f.GroupId == groupId
                                            select fr).ToListAsync();

                var result = icaos.Select(f =>
                {
                    var order = fuelerlinxFBOsOrdersCount.Result.Where(o => o.Icao == f.Icao).FirstOrDefault();
                    var yourOrder = yourOrderCount.Count(y => y.Icao == f.Icao && (y.Cancelled == null || y.Cancelled == false) && y.Etd >= request.StartDateTime && y.Etd < request.EndDateTime.AddDays(1));
                    var airportOrder = order == null ? 0 : order?.AirportOrders;
                    var fboOrder = (order == null ? 0 : order?.FboOrders);
                    var marketShare = (double)(airportOrder == 0 ? 0 : (((double)fboOrder + (double)yourOrder) / (double)airportOrder) * 100);

                    return new
                    {
                        f.Icao,
                        AirportOrders = airportOrder,
                        FboOrders = fboOrder,
                        YourOrders = yourOrder,
                        MarketShare = Math.Round(marketShare)
                    };
                }).ToList();

                return Ok(result);
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
                                     where fr.Fboid == fboId && (fr.Cancelled == null || fr.Cancelled == false) && fr.Etd >= request.StartDateTime && fr.Etd <= request.EndDateTime
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
                var validTransactions = await _context.FuelReq.Where(fr =>
                    (fr.Cancelled == null || fr.Cancelled == false) &&fr.Etd >= request.StartDateTime && fr.Etd <= request.EndDateTime && fr.Fboid.HasValue && fr.Fboid.Value == fboId).ToListAsync();

                var fuelReqs = (from fr in validTransactions
                                      group fr by fr.CustomerId
                                into groupedFuelReqs
                                      select new
                                      {
                                          CustomerId = groupedFuelReqs.Key,
                                          TotalOrders = groupedFuelReqs.Count(),
                                          TotalVolume = groupedFuelReqs.Sum(fr => (fr.ActualVolume ?? 0) * (fr.ActualPpg ?? 0) > 0 ?
                                                       fr.ActualVolume * fr.ActualPpg :
                                                       (fr.QuotedVolume ?? 0) * (fr.QuotedPpg ?? 0)) ?? 0
                                      }).ToList();

                var pricingLogs = await (from cpl in _context.CompanyPricingLog
                                         join c in _context.Customers on cpl.CompanyId equals c.FuelerlinxId
                                         join cibg in _context.CustomerInfoByGroup on c.Oid equals cibg.CustomerId
                                         where cibg.GroupId == groupId && cpl.ICAO == icao
                                         select new
                                         {
                                             cibg.CustomerId,
                                             cpl.CreatedDate
                                         }).ToListAsync();
                int? customeridval = null;
                if (request.customerId.HasValue)
                {
                    customeridval = request.customerId;
                }

                var customers = await _context.CustomerInfoByGroup
                                        .Where(c => c.GroupId.Equals(groupId))
                                        .Include(x => x.Customer)
                                        .Where(x => (x.Customer != null && x.Customer.Suspended != true)&&(x.CustomerId== customeridval
                                        || customeridval == null)).Select(c => new
                                        {
                                            Oid = c.Oid,
                                            c.CustomerId,
                                            Company = c.Company.Trim(),
                                            Customer = c.Customer
                                        })
                                       .Distinct().ToListAsync(); 

                FBOLinxOrdersRequest fbolinxOrdersRequest = new FBOLinxOrdersRequest();
                fbolinxOrdersRequest.StartDateTime = request.StartDateTime;
                fbolinxOrdersRequest.EndDateTime = request.EndDateTime;
                fbolinxOrdersRequest.Icao = icao;

                FboLinxCustomerTransactionsCountAtAirportResponse response = await _fuelerLinxService.GetCustomerTransactionsCountForAirport(fbolinxOrdersRequest);
                ICollection<FbolinxCustomerTransactionsCountAtAirport> fuelerlinxCustomerOrdersCount = response.Result;

                var fbo = await _fboService.GetFbo(fboId);
                fbolinxOrdersRequest.Fbo = fbo.Fbo;

                FboLinxCustomerTransactionsCountAtAirportResponse fboTransactionsResponse = await _fuelerLinxService.GetCustomerFBOTransactionsCount(fbolinxOrdersRequest);
                ICollection<FbolinxCustomerTransactionsCountAtAirport> fuelerlinxCustomerFBOOrdersCount = fboTransactionsResponse.Result;

                List<AirportWatchHistoricalDataResponse> airportWatchHistoricalDataResponse = await _airportWatchService.GetArrivalsDepartures(groupId, fboId, new AirportWatchHistoricalDataRequest() { StartDateTime = request.StartDateTime, EndDateTime = request.EndDateTime });
                var groupedAirportWatchHistoricalDataResponse = airportWatchHistoricalDataResponse.Where(g => g.Status == "Arrival").GroupBy(ah => new { ah.CompanyId }).Select(a => new
                {
                    Company = a.Key,
                    VisitsToMyFbo = a.Count(f => f.VisitsToMyFbo > 0),
                    AirportVisis = a.Count(a => a.PastVisits > -1)
                }).ToList();

                if (customeridval == null)
                {
                    //Fill-in customers that don't exist in the group anymore
                    List<int> customerFuelerlinxIds = customers.Where(x => (x.Customer?.FuelerlinxId).GetValueOrDefault() != 0)
                        .Select(x => Math.Abs((x.Customer?.FuelerlinxId).GetValueOrDefault())).ToList();
                    var fuelerlinxCompanyIdsNotInGroup = fuelerlinxCustomerFBOOrdersCount.Where(x =>
                        !customerFuelerlinxIds.Contains(x.FuelerLinxCustomerId)).Select(x => x.FuelerLinxCustomerId).Where(x => x > 0).Distinct();
                    foreach (var fuelerlinxCompanyId in fuelerlinxCompanyIdsNotInGroup)
                    {
                        var existingCustomerRecord = await _context.Customers.FirstOrDefaultAsync(x =>
                            Math.Abs(x.FuelerlinxId.GetValueOrDefault()) == fuelerlinxCompanyId);
                        customers.Add(new { Oid = 0, CustomerId = (existingCustomerRecord?.Oid).GetValueOrDefault(), Company = existingCustomerRecord?.Company, Customer = existingCustomerRecord });
                    }
                }

                List<object> tableData = new List<object>();
                foreach (var customer in customers)
                {
                    var fuelerLinxCustomerID = Math.Abs((customer.Customer?.FuelerlinxId).GetValueOrDefault());
                    var selectedCompanyFuelReqs = fuelReqs.Where(f => f.CustomerId.Equals(customer.CustomerId)).FirstOrDefault();
                    var companyQuotes = pricingLogs.Where(c => c.CreatedDate >= request.StartDateTime && c.CreatedDate <= request.EndDateTime && c.CustomerId.Equals(customer.CustomerId)).Count();
                    var companyPricingLog = pricingLogs.Where(c => c.CustomerId.Equals(customer.CustomerId)).OrderByDescending(c => c.CreatedDate).FirstOrDefault();
                    
                    var totalOrders = 0;
                    if (fuelerlinxCustomerFBOOrdersCount != null)
                        totalOrders = fuelerlinxCustomerFBOOrdersCount.Where(c => c.FuelerLinxCustomerId == fuelerLinxCustomerID).Select(f => f.TransactionsCount).FirstOrDefault();
                    
                    var airportTotalOrders = 0;
                    if (fuelerlinxCustomerOrdersCount != null)
                        airportTotalOrders = fuelerlinxCustomerOrdersCount.Where(c => c.FuelerLinxCustomerId == fuelerLinxCustomerID).Select(f => f.TransactionsCount).FirstOrDefault();

                    var conversionRateTotalOrders = 0;
                    if (selectedCompanyFuelReqs != null && selectedCompanyFuelReqs.TotalOrders > 0)
                        conversionRateTotalOrders = selectedCompanyFuelReqs.TotalOrders;
                    if (totalOrders > 0)
                        conversionRateTotalOrders += totalOrders;

                    var airportVisits = groupedAirportWatchHistoricalDataResponse.Where(a => a.Company.CompanyId == customer.CustomerId).Select(a => a.AirportVisis).FirstOrDefault();

                    var visitsToFbo = groupedAirportWatchHistoricalDataResponse.Where(a => a.Company.CompanyId == customer.CustomerId).Select(a => a.VisitsToMyFbo).FirstOrDefault();

                    tableData.Add(new
                    {
                        customer.Oid,
                        customer.CustomerId,
                        customer.Company,
                        CompanyQuotesTotal = companyQuotes,
                        DirectOrders = selectedCompanyFuelReqs == null ? 0 : selectedCompanyFuelReqs.TotalOrders,
                        ConversionRate = selectedCompanyFuelReqs == null || companyQuotes == 0 ? 0 : Math.Round(decimal.Parse(selectedCompanyFuelReqs.TotalOrders.ToString()) / decimal.Parse(companyQuotes.ToString()) * 100, 2),
                        ConversionRateTotal = conversionRateTotalOrders == 0 || companyQuotes == 0 ? 0 : Math.Round(decimal.Parse(conversionRateTotalOrders.ToString()) / decimal.Parse(companyQuotes.ToString()) * 100, 2),
                        TotalOrders = totalOrders,
                        AirportOrders = airportTotalOrders,
                        CustomerBusiness = airportTotalOrders == 0 ? 0 : Math.Round(decimal.Parse(totalOrders.ToString()) / decimal.Parse(airportTotalOrders.ToString()) * 100, 2),
                        LastPullDate = companyPricingLog == null ? "N/A" : companyPricingLog.CreatedDate.ToString(),
                        AirportICAO = icao,
                        AirportVisits = airportVisits == null ? 0 : airportVisits,
                        VisitsToFbo = visitsToFbo == null ? 0 : visitsToFbo,
                        PercentVisits = visitsToFbo > 0 ? Math.Round(((double)visitsToFbo / (double)airportVisits * 100), 2) : 0
                    });
                }

                return Ok(tableData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("analysis/company-quoting-deal-statistics/group/{groupId}")]
        public async Task<IActionResult> GetCompanyStatisticsForGroup([FromRoute] int groupId, [FromBody] FuelReqsCompanyStatisticsForGroupRequest request = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<string> icaos = await _context.Fboairports.Where(f => request.FboIds.Contains(f.Fboid)).Select(f => f.Icao).ToListAsync();
                List<int> airportfbos = await _context.Fboairports.Where(f => icaos.Contains(f.Icao)).Select(f => f.Fboid).ToListAsync();
                var validTransactions = await _context.FuelReq.Where(fr =>
                    (fr.Cancelled == null || fr.Cancelled == false) && fr.Etd >= request.StartDateTime && fr.Etd <= request.EndDateTime && fr.Fboid > 0 &&
                    airportfbos.Contains(fr.Fboid ?? 0)).ToListAsync();

                var fuelReqs = (from fr in validTransactions
                                group fr by fr.CustomerId
                                into groupedFuelReqs
                                select new
                                {
                                    CustomerId = groupedFuelReqs.Key,
                                    TotalOrders = groupedFuelReqs.Count(),
                                    TotalVolume = groupedFuelReqs.Sum(fr => (fr.ActualVolume ?? 0) * (fr.ActualPpg ?? 0) > 0 ?
                                                 fr.ActualVolume * fr.ActualPpg :
                                                 (fr.QuotedVolume ?? 0) * (fr.QuotedPpg ?? 0)) ?? 0
                                }).ToList();

                var pricingLogs = await (from cpl in _context.CompanyPricingLog
                                       join c in _context.Customers on cpl.CompanyId equals c.FuelerlinxId
                                       join cibg in _context.CustomerInfoByGroup on c.Oid equals cibg.CustomerId
                                       where cibg.GroupId == groupId && icaos.Contains(cpl.ICAO)
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

                FBOLinxOrdersForMultipleAirportsRequest fbolinxOrdersRequest = new FBOLinxOrdersForMultipleAirportsRequest();
                fbolinxOrdersRequest.StartDateTime = request.StartDateTime;
                fbolinxOrdersRequest.EndDateTime = request.EndDateTime;

                foreach (var icao in icaos)
                {
                    if (!fbolinxOrdersRequest.IcaosFbos.ContainsKey(icao))
                        fbolinxOrdersRequest.IcaosFbos.Add(icao, icao);
                }

                FboLinxCustomerTransactionsCountAtAirportResponse response = await _fuelerLinxService.GetCustomerTransactionsCountForMultipleAirports(fbolinxOrdersRequest);
                ICollection<FbolinxCustomerTransactionsCountAtAirport> fuelerlinxCustomerOrdersCount = response.Result;

                List<object> tableData = new List<object>();
                foreach (var customer in customers)
                {
                    //var fuelerLinxCustomerID = _context.Customers.Where(c => c.Oid.Equals(customer.CustomerId)).Select(c => c.FuelerlinxId).FirstOrDefault();
                    var fuelerLinxCustomerID = customer.Customer?.FuelerlinxId;
                    var selectedCompanyFuelReqs = fuelReqs.Where(f => f.CustomerId.Equals(customer.CustomerId)).FirstOrDefault();
                    var companyQuotes = pricingLogs.Where(c => c.CreatedDate >= request.StartDateTime && c.CreatedDate <= request.EndDateTime && c.CustomerId.Equals(customer.CustomerId)).Count();
                    var companyPricingLog = pricingLogs.Where(c => c.CustomerId.Equals(customer.CustomerId)).OrderByDescending(c => c.CreatedDate).FirstOrDefault();
                    var totalOrders = fuelerlinxCustomerOrdersCount.Where(c => c.FuelerLinxCustomerId == fuelerLinxCustomerID).Select(f => f.TransactionsCount).FirstOrDefault();

                    tableData.Add(new
                    {
                        customer.Company,
                        CompanyQuotesTotal = companyQuotes,
                        DirectOrders = selectedCompanyFuelReqs == null ? 0 : selectedCompanyFuelReqs.TotalOrders,
                        ConversionRate = selectedCompanyFuelReqs == null || companyQuotes == 0 ? 0 : Math.Round(decimal.Parse(selectedCompanyFuelReqs.TotalOrders.ToString()) / decimal.Parse(companyQuotes.ToString()) * 100, 2),
                        TotalOrders = totalOrders == null ? 0 : totalOrders,
                        LastPullDate = companyPricingLog == null ? "N/A" : companyPricingLog.CreatedDate.ToString(),
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
                               where f.Fboid.Equals(fboId) && (f.Cancelled == null || f.Cancelled == false) && f.Etd.HasValue && f.Etd >= startDateTime && f.Etd <= endDateTime
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
