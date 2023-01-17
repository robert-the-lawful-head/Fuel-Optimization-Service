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
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Airport;

namespace FBOLinx.ServiceLayer.BusinessServices.MissedOrderLog
{
    public interface IMissedOrderLogService : IBaseDTOService<MissedQuoteLogDTO, DB.Models.MissedQuoteLog>
    {
        Task<List<MissedQuotesLogViewModel>> GetMissedOrders(int fboId, DateTime startDateTime, DateTime endDateTime, bool isRecent = false);
    }

    public class MissedOrderLogService : BaseDTOService<MissedQuoteLogDTO, DB.Models.MissedQuoteLog, FboLinxContext>, IMissedOrderLogService
    {
        private IFboEntityService _FboEntityService;
        private IFboAirportsService _FboAirportsService;
        private readonly ICustomerAircraftService _CustomerAircraftService;
        private readonly ICustomerInfoByGroupService _CustomerInfoByGroupService;
        private readonly IFuelReqService _FuelReqService;
        private FuelerLinxApiService _FuelerLinxApiService;
        private IPricingTemplateEntityService _PricingTemplateEntityService;
        private IAirportTimeService _AirportTimeService;

        public MissedOrderLogService(IMissedQuoteLogEntityService entityService, IFboEntityService fboEntityService, IFboAirportsService iFboAirportsService,
            ICustomerAircraftService customerAircraftService, ICustomerInfoByGroupService customerInfoByGroupService, IFuelReqService fuelReqService, FuelerLinxApiService fuelerLinxApiService, IPricingTemplateEntityService pricingTemplateEntityService,
            IAirportTimeService airportTimeService) : base(entityService)
        {
            _AirportTimeService = airportTimeService;
            _EntityService = entityService;
            _FboEntityService = fboEntityService;
            _FboAirportsService = iFboAirportsService;
            _CustomerAircraftService = customerAircraftService;
            _CustomerInfoByGroupService = customerInfoByGroupService;
            _FuelReqService = fuelReqService;
            _FuelerLinxApiService = fuelerLinxApiService;
            _PricingTemplateEntityService = pricingTemplateEntityService;
        }

        public async Task<List<MissedQuotesLogViewModel>> GetMissedOrders(int fboId, DateTime startDateTime, DateTime endDateTime, bool isRecent = false)
        {
            var fbo = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));

            if (fbo == null)
                return new List<MissedQuotesLogViewModel>();

            var fbos = await _FboEntityService.GetFbosByIcaos(fbo.FboAirport?.Icao);

            var missedOrdersLogList = new List<MissedQuotesLogViewModel>();

            var customers = await _CustomerInfoByGroupService.GetCustomersByGroupAndFbo(fbo.GroupId.GetValueOrDefault(), fboId);

            var customerAircraftsPricingTemplates = await _PricingTemplateEntityService.GetCustomerAircrafts(fbo.GroupId.GetValueOrDefault(), fboId);

            var allFboLinxTransactions = new List<FuelReqDto>();
            foreach (var otherFbo in fbos.Where(f => f.Oid != fboId))
            {
                var recentTransactions = await _FuelReqService.GetDirectOrdersForFbo(otherFbo.Oid, startDateTime, endDateTime);
                allFboLinxTransactions.AddRange(recentTransactions);
            }

            var localTimeZone = await _AirportTimeService.GetAirportTimeZone(fbo.FboAirport?.Icao);

            var groupedAllFboLinxTransactions = allFboLinxTransactions.Where(a => a.Cancelled == null || a.Cancelled == false).GroupBy(t => t.CustomerId).Select(g => new
            {
                CustomerId = g.Key,
                MissedQuoteCount = g.Count(f => f.CustomerAircraftId > 0)
            }).ToList();

            foreach (var transaction in allFboLinxTransactions.Where(a => a.Cancelled == null || a.Cancelled == false).OrderByDescending(f => f.DateCreated))
            {
                var customer = customers.Where(c => c.CustomerId == transaction.CustomerId).FirstOrDefault();

                if (customer != null && !missedOrdersLogList.Any(x => x.CustomerName == customer.Company))
                {
                    MissedQuotesLogViewModel missedQuotesLogViewModel = new MissedQuotesLogViewModel();
                    missedQuotesLogViewModel.CustomerName = customer.Company;

                    var localDateTimeCreatedDate = await _AirportTimeService.GetAirportLocalDateTime(fbo.FboAirport?.Icao, transaction.DateCreated.GetValueOrDefault());
                    missedQuotesLogViewModel.CreatedDate = localDateTimeCreatedDate.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;
                    var localDateTimeEta = await _AirportTimeService.GetAirportLocalDateTime(fbo.FboAirport?.Icao, transaction.Eta.GetValueOrDefault());
                    missedQuotesLogViewModel.Eta = localDateTimeEta.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone; ;
                    var localDateTimeEtd = await _AirportTimeService.GetAirportLocalDateTime(fbo.FboAirport?.Icao, transaction.Etd.GetValueOrDefault());
                    missedQuotesLogViewModel.Etd = localDateTimeEtd.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;

                    missedQuotesLogViewModel.Volume = transaction.QuotedVolume.GetValueOrDefault();
                    missedQuotesLogViewModel.TailNumber = await _CustomerAircraftService.GetCustomerAircraftTailNumberByCustomerAircraftId(transaction.CustomerAircraftId.GetValueOrDefault());
                    var customerAircraftPricingTemplate = customerAircraftsPricingTemplates.Where(c => c.TailNumber == missedQuotesLogViewModel.TailNumber).FirstOrDefault();
                    missedQuotesLogViewModel.ItpMarginTemplate = customerAircraftPricingTemplate?.PricingTemplateName;
                    missedQuotesLogViewModel.CustomerInfoByGroupId = customer.Oid;
                    missedQuotesLogViewModel.MissedQuotesCount = groupedAllFboLinxTransactions.Where(g => g.CustomerId == customer.CustomerId).Select(m => m.MissedQuoteCount).FirstOrDefault();
                    missedOrdersLogList.Add(missedQuotesLogViewModel);
                }
            }

            if (isRecent && missedOrdersLogList.Count >= 5)
                return missedOrdersLogList.OrderByDescending(m => m.CreatedDate).Take(5).ToList();

            FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _FuelerLinxApiService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = endDateTime, StartDateTime = startDateTime, Icao = fbo.FboAirport?.Icao, Fbo = fbo.Fbo, IsNotEqualToFbo = true });

            var groupedFuelerLinxContractFuelOrders = fuelerlinxContractFuelOrders.Result.GroupBy(t => t.CompanyId).Select(g => new
            {
                CompanyId = g.Key,
                MissedQuoteCount = g.Count(f => f.TailNumber != "")
            }).ToList();

            foreach (Fuelerlinx.SDK.TransactionDTO transaction in fuelerlinxContractFuelOrders.Result.OrderByDescending(f => f.CreationDate))
            {
                var customer = customers.Where(c => c.Customer.FuelerlinxId == transaction.CompanyId.GetValueOrDefault()).FirstOrDefault();

                if (customer != null && !missedOrdersLogList.Any(x => x.CustomerName == customer.Company))
                {
                    MissedQuotesLogViewModel missedQuotesLogViewModel = new MissedQuotesLogViewModel();
                    missedQuotesLogViewModel.CustomerName = customer.Company;

                    var localDateTimeCreatedDate = await _AirportTimeService.GetAirportLocalDateTime(fbo.FboAirport?.Icao, transaction.CreationDate.GetValueOrDefault());
                    missedQuotesLogViewModel.CreatedDate = localDateTimeCreatedDate.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;
                    var localDateTimeEta = await _AirportTimeService.GetAirportLocalDateTime(fbo.FboAirport?.Icao, transaction.ArrivalDateTime.GetValueOrDefault());
                    missedQuotesLogViewModel.Eta = localDateTimeEta.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone; ;
                    var localDateTimeEtd = await _AirportTimeService.GetAirportLocalDateTime(fbo.FboAirport?.Icao, transaction.DepartureDateTime.GetValueOrDefault());
                    missedQuotesLogViewModel.Etd = localDateTimeEtd.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;

                    missedQuotesLogViewModel.Volume = transaction.DispatchedVolume.Amount;
                    missedQuotesLogViewModel.TailNumber = transaction.TailNumber;
                    var customerAircraftPricingTemplate = customerAircraftsPricingTemplates.Where(c => c.TailNumber == missedQuotesLogViewModel.TailNumber).FirstOrDefault();
                    if (customerAircraftPricingTemplate != null)
                        missedQuotesLogViewModel.ItpMarginTemplate = customerAircraftPricingTemplate.PricingTemplateName;
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
