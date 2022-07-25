using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.Web.Services;
using Fuelerlinx.SDK;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FboMissedQuotesLogController : Controller
    {
        private MissedQuoteLogService _missedQuoteService;
        private FuelerLinxApiService _fuelerLinxApiService;
        private Services.FboService _fboService;
        private IFboService _iFboService;
        private readonly CustomerService _customerService;
        private readonly FuelReqService _fuelReqService;
        private readonly CustomerAircraftService _customerAircraftService;

        public FboMissedQuotesLogController(MissedQuoteLogService missedQuoteLogService, FuelerLinxApiService fuelerLinxApiService, Services.FboService fboService, CustomerService customerService, IFboService iFboService, FuelReqService fuelReqService)
        {
            _missedQuoteService = missedQuoteLogService;
            _fuelerLinxApiService = fuelerLinxApiService;
            _fboService = fboService;
            _customerService = customerService;
            _iFboService = iFboService;
            _fuelReqService = fuelReqService;
        }

        // GET: api/FboMissedQuotesLog/recent-missed-quotes/fbo/5
        [HttpGet("recent-missed-quotes/fbo/{fboId}")]
        public async Task<IActionResult> GetRecentMissedQuotes([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recentMissedQuotes  = await _missedQuoteService.GetRecentMissedQuotes(fboId, true);

            var fbo = await _fboService.GetFbo(fboId);
            var customersList = await _customerService.GetCustomersListByGroupAndFbo(fbo.GroupId.GetValueOrDefault(), fboId);

            var recentMissedQuotesGroupedList = recentMissedQuotes.GroupBy(r => r.CustomerId).Select(g => new
            {
                CustomerId = g.Key,
                MissedQuotesCount = g.Count(x => x.CustomerId > 0)
            }).ToList();

            var missedQuotesLogList = new List<MissedQuotesLogViewModel>();
            foreach (MissedQuoteLogDto missedQuoteLogDto in recentMissedQuotes)
            {
                if (missedQuotesLogList.Count == 5)
                    break;

                var customerName = customersList.Where(c => c.CompanyId == missedQuoteLogDto.CustomerId).Select(x => x.Company).FirstOrDefault();
                if (customerName != null && !missedQuotesLogList.Any(x => x.CustomerName == customerName))
                {
                    MissedQuotesLogViewModel missedQuotesLogViewModel = new MissedQuotesLogViewModel();
                    missedQuotesLogViewModel.CustomerName = customerName;
                    missedQuotesLogViewModel.CreatedDate = missedQuoteLogDto.CreatedDateString;
                    missedQuotesLogViewModel.MissedQuotesCount = recentMissedQuotesGroupedList.Where(g => g.CustomerId == missedQuoteLogDto.CustomerId).Select(m => m.MissedQuotesCount).FirstOrDefault();
                    missedQuotesLogList.Add(missedQuotesLogViewModel);
                }
            }

            return Ok(missedQuotesLogList);
        }

        // GET: api/FboMissedQuotesLog/recent-missed-orders/fbo/5
        [HttpGet("recent-missed-orders/fbo/{fboId}")]
        public async Task<IActionResult> GetRecentMissedOrders([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = await _fboService.GetFbo(fboId);

            var fbos = await _fboService.GetFbosByIcaos(fbo.fboAirport.Icao);

            var missedOrdersLogList = new List<MissedQuotesLogViewModel>();

            var customers = await _customerService.GetCustomersByGroupAndFbo(fbo.GroupId.GetValueOrDefault(), fboId);

            var allRecentFboLinxTransactions = new List<FuelReq>();
            foreach (var otherFbo in fbos.Where(f => f.Oid != fboId))
            {
                var recentTransactions = await _fuelReqService.GetRecentFuelRequestsForFbo(otherFbo.Oid);
                allRecentFboLinxTransactions.AddRange(recentTransactions);
            }

            var groupedAllRecentFboLinxTransactions = allRecentFboLinxTransactions.Where(a => a.Cancelled == false).GroupBy(t => t.CustomerId).Select(g => new
            {
                CustomerId = g.Key,
                MissedQuoteCount = g.Count(f => f.CustomerAircraftId > 0)
            }).ToList();

            foreach (var transaction in allRecentFboLinxTransactions.Where(a => a.Cancelled == false).OrderByDescending(f => f.DateCreated))
            {
                if (missedOrdersLogList.Count == 5)
                    break;

                var customer = customers.Where(c => c.Customer.Oid == transaction.CustomerId).FirstOrDefault();

                if (customer != null && !missedOrdersLogList.Any(x => x.CustomerName == customer.Company))
                {
                    MissedQuotesLogViewModel missedQuotesLogViewModel = new MissedQuotesLogViewModel();
                    missedQuotesLogViewModel.CustomerName = customer.Company;

                    var localDateTime = await _iFboService.GetAirportLocalDateTimeByUtcFboId(transaction.Eta.GetValueOrDefault(), fboId);
                    var localTimeZone = await _iFboService.GetAirportTimeZoneByFboId(fboId);
                    missedQuotesLogViewModel.CreatedDate = localDateTime.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;

                    missedQuotesLogViewModel.TailNumber = await _customerAircraftService.GetCustomerAircraftTailNumberByCustomerAircraftId(transaction.CustomerAircraftId.GetValueOrDefault());
                    missedQuotesLogViewModel.MissedQuotesCount = groupedAllRecentFboLinxTransactions.Where(g => g.CustomerId == customer.Customer.Oid).Select(m => m.MissedQuoteCount).FirstOrDefault();
                    missedQuotesLogViewModel.CustomerId = customer.Oid;
                    missedOrdersLogList.Add(missedQuotesLogViewModel);
                }
            }

            if (missedOrdersLogList.Count < 5)
            {
                FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _fuelerLinxApiService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = DateTime.UtcNow.Add(new TimeSpan(3, 0, 0, 0)), StartDateTime = DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0)), Icao = fbo.fboAirport.Icao, Fbo = fbo.Fbo, IsNotEqualToFbo = true });

                var groupedFuelerLinxContractFuelOrders = fuelerlinxContractFuelOrders.Result.GroupBy(t => t.CompanyId).Select(g => new
                {
                    CompanyId = g.Key,
                    MissedQuoteCount = g.Count(f => f.TailNumber != "")
                }).ToList();

                foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result.OrderByDescending(f => f.CreationDate))
                {
                    if (missedOrdersLogList.Count == 5)
                        break;

                    var customer = customers.Where(c => c.Customer.FuelerlinxId == transaction.CompanyId.GetValueOrDefault()).FirstOrDefault();

                    if (customer != null && !missedOrdersLogList.Any(x => x.CustomerName == customer.Company))
                    {
                        MissedQuotesLogViewModel missedQuotesLogViewModel = new MissedQuotesLogViewModel();
                        missedQuotesLogViewModel.CustomerName = customer.Company;

                        var localDateTime = await _iFboService.GetAirportLocalDateTimeByUtcFboId(transaction.ArrivalDateTime.GetValueOrDefault(), fboId);
                        var localTimeZone = await _iFboService.GetAirportTimeZoneByFboId(fboId);
                        missedQuotesLogViewModel.CreatedDate = localDateTime.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;

                        missedQuotesLogViewModel.TailNumber = transaction.TailNumber;
                        missedQuotesLogViewModel.MissedQuotesCount = groupedFuelerLinxContractFuelOrders.Where(g => g.CompanyId == customer.Customer.FuelerlinxId).Select(m => m.MissedQuoteCount).FirstOrDefault();
                        missedQuotesLogViewModel.CustomerId = customer.Oid;
                        missedOrdersLogList.Add(missedQuotesLogViewModel);
                    }
                }
            }

            return Ok(missedOrdersLogList);
        }


    }
}
