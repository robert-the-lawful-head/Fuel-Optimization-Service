using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.MissedOrderLog
{
    public interface IMissedOrderLogService : IBaseDTOService<MissedQuoteLogDTO, DB.Models.MissedQuoteLog>
    {
        Task<List<MissedQuotesLogViewModel>> GetMissedOrders(int fboId, DateTime startDateTime, DateTime endDateTime, bool isRecent = false);
    }

    public class MissedOrderLogService : BaseDTOService<MissedQuoteLogDTO, DB.Models.MissedQuoteLog, FboLinxContext>, IMissedOrderLogService
    {
        private IFboService _FboService; 
        private IFboEntityService _FboEntityService;
        private IFboAirportsService _FboAirportsService;
        private readonly ICustomerAircraftService _CustomerAircraftService;
        private readonly CustomerService _CustomerService;
        private readonly IFuelReqService _FuelReqService;
        private FuelerLinxApiService _FuelerLinxApiService;
        private IPricingTemplateEntityService _PricingTemplateEntityService;

        public MissedOrderLogService(IMissedQuoteLogEntityService entityService, IFboService fboService, IFboEntityService fboEntityService, IFboAirportsService iFboAirportsService,
            ICustomerAircraftService customerAircraftService, CustomerService customerService, IFuelReqService fuelReqService, FuelerLinxApiService fuelerLinxApiService, IPricingTemplateEntityService pricingTemplateEntityService) : base(entityService)
        {
            _EntityService = entityService;
            _FboService = fboService;
            _FboEntityService = fboEntityService;
            _FboAirportsService = iFboAirportsService;
            _CustomerAircraftService = customerAircraftService;
            _CustomerService = customerService;
            _FuelReqService = fuelReqService;
            _FuelerLinxApiService = fuelerLinxApiService;
            _PricingTemplateEntityService = pricingTemplateEntityService;
        }

        public async Task<List<MissedQuotesLogViewModel>> GetMissedOrders(int fboId, DateTime startDateTime, DateTime endDateTime, bool isRecent = false)
        {
            var fbo = await _FboEntityService.GetFboByFboId(fboId);

            var fboAirport = await _FboAirportsService.GetFboAirportsByFboId(fboId);

            var fbos = await _FboEntityService.GetFbosByIcaos(fboAirport.Icao);

            var missedOrdersLogList = new List<MissedQuotesLogViewModel>();

            var customers = await _CustomerService.GetCustomersByGroupAndFbo(fbo.GroupId.GetValueOrDefault(), fboId);

            var customerAircraftsPricingTemplates = await _PricingTemplateEntityService.GetCustomerAircrafts(fbo.GroupId.GetValueOrDefault(), fboId);

            var allFboLinxTransactions = new List<FuelReq>();
            foreach (var otherFbo in fbos.Where(f => f.Oid != fboId))
            {
                var recentTransactions = await _FuelReqService.GetFuelOrdersForFbo(otherFbo.Oid, startDateTime, endDateTime);
                allFboLinxTransactions.AddRange(recentTransactions);
            }

            var localTimeZone = await _FboService.GetAirportTimeZoneByFboId(fboId);

            var groupedAllFboLinxTransactions = allFboLinxTransactions.Where(a => a.Cancelled == null || a.Cancelled == false).GroupBy(t => t.CustomerId).Select(g => new
            {
                CustomerId = g.Key,
                MissedQuoteCount = g.Count(f => f.CustomerAircraftId > 0)
            }).ToList();

            foreach (var transaction in allFboLinxTransactions.Where(a => a.Cancelled == null || a.Cancelled == false).OrderByDescending(f => f.DateCreated))
            {
                var customer = customers.Where(c => c.Customer.Oid == transaction.CustomerId).FirstOrDefault();

                if (customer != null && !missedOrdersLogList.Any(x => x.CustomerName == customer.Company))
                {
                    MissedQuotesLogViewModel missedQuotesLogViewModel = new MissedQuotesLogViewModel();
                    missedQuotesLogViewModel.CustomerName = customer.Company;

                    var localDateTimeCreatedDate = await _FboService.GetAirportLocalDateTimeByUtcFboId(transaction.DateCreated.GetValueOrDefault(), fboId);
                    missedQuotesLogViewModel.CreatedDate = localDateTimeCreatedDate.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;
                    var localDateTimeEta = await _FboService.GetAirportLocalDateTimeByUtcFboId(transaction.Eta.GetValueOrDefault(), fboId);
                    missedQuotesLogViewModel.Eta = localDateTimeEta.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone; ;
                    var localDateTimeEtd = await _FboService.GetAirportLocalDateTimeByUtcFboId(transaction.Etd.GetValueOrDefault(), fboId);
                    missedQuotesLogViewModel.Etd = localDateTimeEtd.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;

                    missedQuotesLogViewModel.Volume = transaction.QuotedVolume.GetValueOrDefault();
                    missedQuotesLogViewModel.TailNumber = await _CustomerAircraftService.GetCustomerAircraftTailNumberByCustomerAircraftId(transaction.CustomerAircraftId.GetValueOrDefault());
                    var customerAircraftPricingTemplate = customerAircraftsPricingTemplates.Where(c => c.TailNumber == missedQuotesLogViewModel.TailNumber).FirstOrDefault();
                    missedQuotesLogViewModel.ItpMarginTemplate = customerAircraftPricingTemplate.PricingTemplateName;
                    missedQuotesLogViewModel.CustomerInfoByGroupId = customer.Oid;
                    missedQuotesLogViewModel.MissedQuotesCount = groupedAllFboLinxTransactions.Where(g => g.CustomerId == customer.Customer.Oid).Select(m => m.MissedQuoteCount).FirstOrDefault();
                    missedOrdersLogList.Add(missedQuotesLogViewModel);
                }
            }

            if (isRecent && missedOrdersLogList.Count >= 5)
                return missedOrdersLogList.OrderByDescending(m => m.CreatedDate).Take(5).ToList();

            FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _FuelerLinxApiService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = endDateTime, StartDateTime = startDateTime, Icao = fboAirport.Icao, Fbo = fbo.Fbo, IsNotEqualToFbo = true });

            var groupedFuelerLinxContractFuelOrders = fuelerlinxContractFuelOrders.Result.GroupBy(t => t.CompanyId).Select(g => new
            {
                CompanyId = g.Key,
                MissedQuoteCount = g.Count(f => f.TailNumber != "")
            }).ToList();

            foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result.OrderByDescending(f => f.CreationDate))
            {
                var customer = customers.Where(c => c.Customer.FuelerlinxId == transaction.CompanyId.GetValueOrDefault()).FirstOrDefault();

                if (customer != null && !missedOrdersLogList.Any(x => x.CustomerName == customer.Company))
                {
                    MissedQuotesLogViewModel missedQuotesLogViewModel = new MissedQuotesLogViewModel();
                    missedQuotesLogViewModel.CustomerName = customer.Company;

                    var localDateTimeCreatedDate = await _FboService.GetAirportLocalDateTimeByUtcFboId(transaction.CreationDate.GetValueOrDefault(), fboId);
                    missedQuotesLogViewModel.CreatedDate = localDateTimeCreatedDate.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;
                    var localDateTimeEta = await _FboService.GetAirportLocalDateTimeByUtcFboId(transaction.CreationDate.GetValueOrDefault(), fboId);
                    missedQuotesLogViewModel.Eta = localDateTimeEta.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone; ;
                    var localDateTimeEtd = await _FboService.GetAirportLocalDateTimeByUtcFboId(transaction.DepartureDateTime.GetValueOrDefault(), fboId);
                    missedQuotesLogViewModel.Etd = localDateTimeEtd.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;

                    missedQuotesLogViewModel.TailNumber = transaction.TailNumber;
                    missedQuotesLogViewModel.CustomerInfoByGroupId = customer.Oid;
                    missedQuotesLogViewModel.MissedQuotesCount = groupedFuelerLinxContractFuelOrders.Where(g => g.CompanyId == customer.Customer.FuelerlinxId).Select(m => m.MissedQuoteCount).FirstOrDefault();
                    missedOrdersLogList.Add(missedQuotesLogViewModel);
                }
            }

            missedOrdersLogList = missedOrdersLogList.OrderByDescending(m => m.CreatedDate).ToList();

            if (isRecent)
                return missedOrdersLogList.Take(5).ToList();

            return missedOrdersLogList;
        }
    }
}
