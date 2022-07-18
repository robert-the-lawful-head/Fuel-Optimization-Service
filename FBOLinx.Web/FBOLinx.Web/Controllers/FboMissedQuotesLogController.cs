using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
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

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FboMissedQuotesLogController : Controller
    {
        private MissedQuoteLogEntityService _missedQuoteService;
        private FuelerLinxApiService _fuelerLinxApiService;
        private Services.FboService _fboService;
        private IFboService _iFboService;
        private readonly CustomerService _customerService;

        public FboMissedQuotesLogController(MissedQuoteLogEntityService missedQuoteLogEntityService, FuelerLinxApiService fuelerLinxApiService, Services.FboService fboService, CustomerService customerService, IFboService iFboService)
        {
            _missedQuoteService = missedQuoteLogEntityService;
            _fuelerLinxApiService = fuelerLinxApiService;
            _fboService = fboService;
            _customerService = customerService;
            _iFboService = iFboService;
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

            FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _fuelerLinxApiService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = DateTime.UtcNow.Add(new TimeSpan(3, 0, 0, 0)), StartDateTime = DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0)), Icao = fbo.fboAirport.Icao, Fbo = null });

            var missedOrdersLogList = new List<MissedQuotesLogViewModel>();
            var groupedFuelerLinxContractFuelOrders = fuelerlinxContractFuelOrders.Result.GroupBy(t => t.CompanyId).Select(g => new
            {
                CompanyId = g.Key,
                MissedQuoteCount = g.Count(f => f.TailNumber != "")
            }).ToList();

            foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result.OrderByDescending(f => f.CreationDate))
            {
                if (missedOrdersLogList.Count == 5)
                    break;

                var customer = await _customerService.GetCustomerByFuelerLinxId(transaction.CompanyId.GetValueOrDefault());

                if (!missedOrdersLogList.Any(x => x.CustomerName == customer.Company))
                {
                    MissedQuotesLogViewModel missedQuotesLogViewModel = new MissedQuotesLogViewModel();
                    missedQuotesLogViewModel.CustomerName = customer.Company;

                    var localDateTime = await _iFboService.GetAirportLocalDateTimeByUtcFboId(transaction.ArrivalDateTime.GetValueOrDefault(), fboId);
                    var localTimeZone = await _iFboService.GetAirportTimeZoneByFboId(fboId);
                    missedQuotesLogViewModel.CreatedDate = localDateTime.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;

                    missedQuotesLogViewModel.TailNumber = transaction.TailNumber;
                    missedQuotesLogViewModel.MissedQuotesCount = groupedFuelerLinxContractFuelOrders.Where(g => g.CompanyId == customer.FuelerlinxId).Select(m => m.MissedQuoteCount).FirstOrDefault();
                    missedOrdersLogList.Add(missedQuotesLogViewModel);
                }
            }

            return Ok(missedOrdersLogList);
        }


    }
}
