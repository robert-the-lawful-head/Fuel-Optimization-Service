using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Microsoft.Extensions.Caching.Memory;
using Azure.Core;
using FBOLinx.ServiceLayer.DTO.Responses.Analitics;
using Microsoft.Extensions.Logging;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using System.Web;
using Microsoft.AspNetCore.Http;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using FBOLinx.ServiceLayer.DTO.Requests.FuelReq;
using FBOLinx.ServiceLayer.BusinessServices.DateAndTime;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.BusinessServices.ServiceOrders;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.DTO.Requests.ServiceOrder;
using FBOLinx.DB.Specifications.User;
using FBOLinx.ServiceLayer.BusinessServices.Orders;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.DTO.Requests.Integrations;
using SendGrid.Helpers.Mail;
using Itenso.TimePeriod;
using FBOLinx.DB.Specifications.OrderDetails;
using FBOLinx.DB.Specifications.Customers;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelRequests
{
    public interface IFuelReqService : IBaseDTOService<FuelReqDto, FuelReq>
    {
        Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrdersForTailNumber(int groupId, int fboId,
            string tailNumber,
            bool useCache = false);
        Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrders(int groupId, int fboId,
            bool useCache = false);
        Task<List<FuelReqDto>> GetDirectOrdersForFbo(int fboId, DateTime? startDateTime = null,
            DateTime? endDateTime = null);

        Task<List<FuelReqDto>> GetDirectAndContractOrdersByGroupAndFbo(int groupId, int fboId,
            DateTime startDateTime, DateTime endDateTime);
        Task<List<ChartDataResponse>> GetCustomersBreakdown(int fboId, int groupId, int? customerId, DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<FuelReqTotalsProjection>> GetValidFuelRequestTotals(int fboId, DateTime startDateTime, DateTime endDateTime);
        IQueryable<ValidCustomersProjection> GetValidCustomers(int groupId, int? customeridval);
        Task<ICollection<FbolinxCustomerTransactionsCountAtAirport>> GetCustomerTransactionsCountForAirport(string icao, DateTime startDateTime, DateTime endDateTime, string fbo);
        Task<ICollection<FbolinxCustomerTransactionsCountAtAirport>> GetfuelerlinxCustomerFBOOrdersCount(string fbo, string icao, DateTime startDateTime, DateTime endDateTime);
        int GetairportTotalOrders(int fuelerLinxCustomerID, ICollection<FbolinxCustomerTransactionsCountAtAirport> fuelerlinxCustomerOrdersCount);
        Task<bool> SendFuelOrderNotificationEmail(int handlerId, int fuelerLinxTransactionId, int fuelerlinxCompanyId, SendOrderNotificationRequest request);
        Task AddServiceOrder(ServiceOrderRequest request, FbosDto fbo);
        Task CheckAndSendFuelOrderUpdateEmail(FuelReqRequest fuelerlinxTransaction);
    }

    public class FuelReqService : BaseDTOService<FuelReqDto, DB.Models.FuelReq, FboLinxContext>, IFuelReqService
    {
        private int _CacheLifeSpanInMinutes = 10;
        private string _UpcomingOrdersCacheKeyPrefix = "UpcomingOrders_ByGroupAndFbo_";
        private int _HoursToLookBackForUpcomingOrders = 12;
        private int _HoursToLookForwardForUpcomingOrders = 48;

        private FuelReqEntityService _FuelReqEntityService;
        private readonly FuelerLinxApiService _fuelerLinxService;
        private readonly FboLinxContext _context;
        private IFboEntityService _FboEntityService;
        private ICustomerInfoByGroupEntityService _CustomerInfoByGroupEntityService;
        private IMemoryCache _MemoryCache;
        private IAirportTimeService _AirportTimeService;
        private readonly AcukwikAirportEntityService _AcukwikAirportEntityService;
        private IMailService _MailService;
        private readonly IAuthService _AuthService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFboContactsEntityService _FboContactsEntityService;
        private IAirportService _AirportService;
         private readonly IServiceOrderService _serviceOrderService;
        private readonly DateTimeService _dateTimeService;
        private readonly IServiceOrderItemService _serviceOrderItemService;
        private readonly ICustomerInfoByGroupService _customerInfoByGroupService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly ICustomerAircraftService _customerAircraftService;
        private readonly IAircraftService _aircraftService;
        private readonly IFboService _fboService;
        private readonly IRepository<FuelReqConfirmation, FboLinxContext> _fuelReqConfirmationRepo;
        private readonly ICustomerService _customerService;

        public FuelReqService(FuelReqEntityService fuelReqEntityService, FuelerLinxApiService fuelerLinxService, FboLinxContext context,
            IFboEntityService fboEntityService,
            ICustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            IMemoryCache memoryCache,
            IAirportTimeService airportTimeService,
            AcukwikAirportEntityService acukwikAirportEntityService,
            IMailService mailService,
            IAuthService authService,
            IHttpContextAccessor httpContextAccessor,
            IFboContactsEntityService fboContactsEntityService,
            IAirportService airportService,
            IServiceOrderService serviceOrderService,
            DateTimeService dateTimeService,
            IServiceOrderItemService serviceOrderItemService,
            ICustomerInfoByGroupService customerInfoByGroupService,
            IOrderDetailsService orderDetailsService,
            ICustomerAircraftService customerAircraftService,
            IAircraftService aircraftService,
            IFboService fboService,
            IRepository<FuelReqConfirmation, FboLinxContext> fuelReqConfirmationRepo, ICustomerService customerService) : base(fuelReqEntityService)
        {
            _AirportService = airportService;
            _serviceOrderService = serviceOrderService;
            _dateTimeService = dateTimeService;
            _serviceOrderItemService = serviceOrderItemService;
            _customerInfoByGroupService = customerInfoByGroupService;
            _orderDetailsService = orderDetailsService;
            _customerAircraftService = customerAircraftService;
            _aircraftService = aircraftService;
            _fboService = fboService;
            _AcukwikAirportEntityService = acukwikAirportEntityService;
            _AirportTimeService = airportTimeService;
            _MemoryCache = memoryCache;
            _CustomerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _FboEntityService = fboEntityService;
            _FuelReqEntityService = fuelReqEntityService;
            _fuelerLinxService = fuelerLinxService;
            _context = context;
            _MailService = mailService;
            _AuthService = authService;
            _httpContextAccessor = httpContextAccessor;
            _FboContactsEntityService = fboContactsEntityService;
            _fuelReqConfirmationRepo = fuelReqConfirmationRepo;
            _customerService = customerService;
        }

        public async Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrdersForTailNumber(int groupId, int fboId,
            string tailNumber,
            bool useCache = true)
        {
            tailNumber = tailNumber.Trim().ToUpper();
            var upcomingOrders = await GetUpcomingDirectAndContractOrders(groupId, fboId, useCache);
            return upcomingOrders?.Where(x => x.TailNumber?.ToUpper() == tailNumber).ToList();
        }

        public async Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrders(int groupId, int fboId,
            bool useCache = true)
        {
            List<FuelReqDto> result = null;
            if (useCache)
            {
                result = await GetUpcomingOrdersFromCache(groupId, fboId);
            }

            if (result == null)
            {
                var startDateTime = await _AirportTimeService.GetAirportLocalDateTime(fboId, DateTime.UtcNow.AddHours(-_HoursToLookBackForUpcomingOrders));
                var endDateTime = await _AirportTimeService.GetAirportLocalDateTime(fboId,
                    DateTime.UtcNow.AddHours(_HoursToLookForwardForUpcomingOrders));
                result = await GetDirectAndContractOrdersByGroupAndFbo(groupId, fboId, startDateTime, endDateTime);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_CacheLifeSpanInMinutes));
                _MemoryCache.Set(_UpcomingOrdersCacheKeyPrefix + groupId + "_" + fboId, result, cacheEntryOptions);
            }

            result?.RemoveAll(x => x.Cancelled.GetValueOrDefault());

            return result;
        }

        public async Task<List<FuelReqDto>> GetDirectOrdersForFbo(int fboId, DateTime? startDateTime = null, DateTime? endDateTime = null)
        {
            var startDate = startDateTime == null ? DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0)) : startDateTime;
            var endDate = endDateTime == null ? DateTime.UtcNow.Add(new TimeSpan(3, 0, 0, 0)) : endDateTime;

            var fboRecord = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));
            if (fboRecord == null)
                return new List<FuelReqDto>();

            var customers =
                await _CustomerInfoByGroupEntityService.GetListBySpec(
                    new CustomerInfoByGroupByGroupIdSpecification(fboRecord.GroupId));

            return await GetDirectOrdersFromDatabase(fboId, startDate, endDate, customers);
        }

        public async Task<List<FuelReqDto>> GetDirectAndContractOrdersByGroupAndFbo(int groupId, int fboId, DateTime startDateTime, DateTime endDateTime)
        {
            var result = new List<FuelReqDto>();
            var fboRecord = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));
            if (fboRecord == null)
                return result;

            var customers =
                await _CustomerInfoByGroupEntityService.GetListBySpec(
                    new CustomerInfoByGroupByGroupIdSpecification(groupId));


            FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _fuelerLinxService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = endDateTime, StartDateTime = startDateTime, Icao = fboRecord.FboAirport?.Icao, Fbo = fboRecord.Fbo});

            List<FuelReqDto> fuelReqsFromFuelerLinx = new List<FuelReqDto>();

            var fuelReqConfirmationList = _fuelReqConfirmationRepo.Get();

            foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result)
            {
                var airport = await _AirportService.GetGeneralAirportInformation(transaction.Icao);

                var isConfirmed = fuelReqConfirmationList.Any(x => x.SourceId == transaction.Id);
                var fuelRequest = FuelReqDto.Cast(transaction, customers.Where(x => x.Customer?.FuelerlinxId == transaction.CompanyId).Select(x => x.Company).FirstOrDefault(), airport, isConfirmed );
                fuelRequest.Fboid = fboId;
                fuelRequest.Cancelled = transaction.InvoiceStatus == TransactionInvoiceStatuses.Cancelled ? true : false;

                fuelReqsFromFuelerLinx.Add(fuelRequest);
            }

            var directOrders = await GetDirectOrdersFromDatabase(fboId, startDateTime, endDateTime, customers);

            foreach (FuelReqDto order in directOrders)
            {
                var airport = await _AirportService.GetGeneralAirportInformation(order.Icao);
                FuelReqDto.SetAirportLocalTimes(order, airport);
                order.IsConfirmed = fuelReqConfirmationList.Any(x => x.SourceId == order.SourceId);
                order.Fboid = fboId;
            }

            var serviceOrders = await _serviceOrderService.GetListbySpec(new ServiceOrderByFboSpecification(fboId, startDateTime, endDateTime));

            foreach(ServiceOrderDto item in serviceOrders)
            {
                var fuelreq = new FuelReqDto()
                {
                    Oid = 0,
                    ActualPpg = 0,
                    ActualVolume = 0,
                    Archived = false,
                    Cancelled = false,
                    CustomerId = item.CustomerInfoByGroup?.CustomerId,
                    DateCreated = item.ServiceDateTimeUtc,//check this property
                    DispatchNotes = string.Empty,
                    Eta = item.ArrivalDateTimeLocal,
                    Etd = item.DepartureDateTimeLocal,
                    Icao = string.Empty,
                    Notes = string.Empty,
                    QuotedPpg = 0,
                    QuotedVolume = 0,
                    Source = string.Empty,
                    SourceId = item.FuelerLinxTransactionId,
                    TimeStandard = null,
                    TailNumber = string.Empty,
                    FboName = string.Empty,
                    Email = string.Empty,
                    PhoneNumber = item.CustomerInfoByGroup?.Customer?.MainPhone,
                    FuelOn = string.Empty,
                    CustomerName = item.CustomerInfoByGroup?.Customer?.Username,
                    IsConfirmed = false,
                };
                result.Add(fuelreq);
            }

            result.AddRange(fuelReqsFromFuelerLinx);
            result.AddRange(directOrders);

            return result;
        }
        private async Task<List<FuelReqDto>> GetUpcomingOrdersFromCache(int groupId, int fboId)
        {
            try
            {
                List<FuelReqDto> result = null;
                if (_MemoryCache.TryGetValue(_UpcomingOrdersCacheKeyPrefix + groupId + "_" + fboId, out result))
                    return result;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<List<FuelReqDto>> GetDirectOrdersFromDatabase(int fboId, DateTime? startDate, DateTime? endDate, List<CustomerInfoByGroup> customers)
        {
            var requests = await GetListbySpec(new FuelReqByFboAndDateSpecification(fboId, startDate.Value, endDate.Value));

            requests.ForEach(x =>
                x.CustomerName = customers.FirstOrDefault(c => c.CustomerId == x.CustomerId.GetValueOrDefault())?.Company);
            return requests;
        }

        public async Task<IEnumerable<FuelReqTotalsProjection>> GetValidFuelRequestTotals(int fboId, DateTime startDateTime, DateTime endDateTime)
        {
            var validTransactions = await _context.FuelReq.Where(fr =>
            (fr.Cancelled == null || fr.Cancelled == false) && fr.Etd >= startDateTime && fr.Etd <= endDateTime && fr.Fboid.HasValue && fr.Fboid.Value == fboId).ToListAsync();

            return (from fr in validTransactions
                    group fr by fr.CustomerId
            into groupedFuelReqs
                    select new FuelReqTotalsProjection()
                    {
                        CustomerId = groupedFuelReqs.Key,
                        TotalOrders = groupedFuelReqs.Count(),
                        TotalVolume = groupedFuelReqs.Sum(fr => (fr.ActualVolume ?? 0) * (fr.ActualPpg ?? 0) > 0 ?
                                     fr.ActualVolume * fr.ActualPpg :
                                     (fr.QuotedVolume ?? 0) * (fr.QuotedPpg ?? 0)) ?? 0
                    });
        }
        public IQueryable<ValidCustomersProjection> GetValidCustomers(int groupId, int? customeridval)
        {
            return _context.CustomerInfoByGroup
                                   .Where(c => c.GroupId.Equals(groupId))
                                   .Include(x => x.Customer)
                                   .Where(x => (x.Customer != null && x.Customer.Suspended != true) && (x.CustomerId == customeridval
                                   || customeridval == null)).Select(c => new ValidCustomersProjection()
                                   {
                                       Oid = c.Oid,
                                       CustomerId = c.CustomerId,
                                       Company = c.Company.Trim(),
                                       Customer = c.Customer
                                   })
                                  .Distinct();
        }
        public async Task<ICollection<FbolinxCustomerTransactionsCountAtAirport>> GetCustomerTransactionsCountForAirport(string icao, DateTime startDateTime, DateTime endDateTime, string fbo)
        {
            FBOLinxOrdersRequest fbolinxOrdersRequest = new FBOLinxOrdersRequest();
            fbolinxOrdersRequest.StartDateTime = startDateTime;
            fbolinxOrdersRequest.EndDateTime = endDateTime;
            fbolinxOrdersRequest.Icao = icao;
            if (fbo != null)
                fbolinxOrdersRequest.Fbo = fbo;

            FboLinxCustomerTransactionsCountAtAirportResponse response = await _fuelerLinxService.GetCustomerTransactionsCountForAirport(fbolinxOrdersRequest);
            return response.Result;
        }
        public async Task<ICollection<FbolinxCustomerTransactionsCountAtAirport>> GetfuelerlinxCustomerFBOOrdersCount(string fbo, string icao, DateTime startDateTime, DateTime endDateTime)
        {
            FBOLinxOrdersRequest fbolinxOrdersRequest = new FBOLinxOrdersRequest();
            fbolinxOrdersRequest.StartDateTime = startDateTime;
            fbolinxOrdersRequest.EndDateTime = endDateTime;
            fbolinxOrdersRequest.Icao = icao;
            fbolinxOrdersRequest.Fbo = fbo;

            var response = await _fuelerLinxService.GetCustomerFBOTransactionsCount(fbolinxOrdersRequest);

            return response.Result;
        }
        public int GetairportTotalOrders(int fuelerLinxCustomerID, ICollection<FbolinxCustomerTransactionsCountAtAirport> fuelerlinxCustomerOrdersCount)
        {
            var airportTotalOrders = 0;
            if (fuelerlinxCustomerOrdersCount != null)
                airportTotalOrders = fuelerlinxCustomerOrdersCount.Where(c => c.FuelerLinxCustomerId == fuelerLinxCustomerID).Select(f => f.TransactionsCount).FirstOrDefault();
            return airportTotalOrders;
        }
        public async Task<List<ChartDataResponse>> GetCustomersBreakdown(int fboId, int groupId, int? customerId, DateTime startDateTime, DateTime endDateTime)
        {
            string icao = await _context.Fboairports.Where(f => f.Fboid.Equals(fboId)).Select(f => f.Icao).FirstOrDefaultAsync();
            var fbo = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));

            var fuelReqs = await GetValidFuelRequestTotals(fboId, startDateTime, endDateTime);

            var customers = await GetValidCustomers(groupId, customerId).ToListAsync();

            var fuelerlinxCustomerOrdersCount = await GetCustomerTransactionsCountForAirport(icao, startDateTime, endDateTime, fbo.Fbo);

            List<ChartDataResponse> chartData = new List<ChartDataResponse>();
            foreach (var customer in customers)
            {
                var fuelerLinxCustomerID = Math.Abs((customer.Customer?.FuelerlinxId).GetValueOrDefault());
                var selectedCompanyFuelReqs = fuelReqs.Where(f => f.CustomerId.Equals(customer.CustomerId)).FirstOrDefault();

                chartData.Add(new ChartDataResponse()
                {
                    Name = customer.Company,
                    Orders = GetairportTotalOrders(fuelerLinxCustomerID, fuelerlinxCustomerOrdersCount),
                    Volume = selectedCompanyFuelReqs?.TotalVolume ?? 0
                });
            }
            return chartData;
        }

        public async Task<bool> SendFuelOrderNotificationEmail(int handlerId, int fuelerlinxTransactionId, int fuelerlinxCompanyId, SendOrderNotificationRequest request)
        {
            var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(handlerId));

            // Get order details
            OrderDetailsDto orderDetails = await _orderDetailsService.GetSingleBySpec(new OrderDetailsByFuelerLinxTransactionIdSpecification(fuelerlinxTransactionId));

            var canSendEmail = false;
            if (fbo == null || (orderDetails != null && !orderDetails.FuelVendor.ToLower().Contains("fbolinx") && fbo != null && fbo.Preferences != null && fbo.Preferences.Oid > 0 && ((fbo.Preferences.OrderNotificationsEnabled.HasValue && fbo.Preferences.OrderNotificationsEnabled.Value) || (fbo.Preferences.OrderNotificationsEnabled == null))))
                canSendEmail = true;
            else if (fbo == null || (orderDetails != null && orderDetails.FuelVendor.ToLower().Contains("fbolinx") && fbo != null && fbo.Preferences != null && fbo.Preferences.Oid > 0 && ((fbo.Preferences.DirectOrderNotificationsEnabled.HasValue && fbo.Preferences.DirectOrderNotificationsEnabled.Value) || (fbo.Preferences.DirectOrderNotificationsEnabled == null))))
                canSendEmail = true;

            // SEND EMAIL IF SETTING IS ON OR NOT FBOLINX CUSTOMER
            if (canSendEmail)
            {
                var authentication = await _AuthService.CreateAuthenticatedLink(handlerId);

                if (authentication.FboEmails != "FBO not found" && authentication.FboEmails != "No email found")
                {
                    var customer = new CustomersDto();
                    var customerAircraftId = 0;
                    var arrivalDateTime = "";
                    var departureDateTime = "";
                    var quotedVolume = 0.0;

                    // Get fuel request and set customer info
                    customer = await _customerService.GetSingleBySpec(new CustomerByFuelerLinxIdSpecification(fuelerlinxCompanyId));

                    FuelReqDto fuelReq = await GetSingleBySpec(new FuelReqBySourceIdSpecification(fuelerlinxTransactionId));
                    if (fuelReq != null)
                    {
                        customerAircraftId = fuelReq.CustomerAircraftId.GetValueOrDefault();
                        arrivalDateTime = fuelReq.Eta.ToString() + " " + fuelReq.TimeStandard;
                        departureDateTime = fuelReq.Etd.ToString() + " " + fuelReq.TimeStandard;
                        quotedVolume = fuelReq.QuotedVolume.GetValueOrDefault();
                    }

                    // Get service request
                    ServiceOrderDto serviceOrder = await _serviceOrderService.GetSingleBySpec(new ServiceOrderByFuelerLinxTransactionIdSpecification(fuelerlinxTransactionId));
                    var serviceNames = new List<string>();
                    if (serviceOrder != null)
                    {
                        serviceNames = serviceOrder.ServiceOrderItems.Select(s => s.ServiceName).ToList();

                        if (customerAircraftId == 0)
                            customerAircraftId = serviceOrder.CustomerAircraftId;

                        arrivalDateTime = serviceOrder.ArrivalDateTimeUtc.ToString() + " (Zulu)";
                        departureDateTime = serviceOrder.DepartureDateTimeUtc.ToString() + " (Zulu)";
                    }

                    if (customer.Oid == 0)
                    {
                        quotedVolume = request.QuotedVolume;
                    }

                    // Get customer aircraft info
                    var customerAircrafts = await _customerAircraftService.GetListbySpec(new CustomerAircraftByGroupSpecification(new List<int> { fbo.GroupId }, customer.Oid));
                    var customerAircraft = new CustomerAircraftsDto();

                    if (customerAircraftId == 0)
                        customerAircraft = customerAircrafts.Where(c => c.TailNumber == request.TailNumber && c.CustomerId == customer.Oid).FirstOrDefault();
                    else
                        customerAircraft = customerAircrafts.Where(c => c.Oid == customerAircraftId).FirstOrDefault();

                    var aircraft = await _aircraftService.GetSingleBySpec(new DB.Specifications.Aircraft.AircraftSpecification(new List<int>() { customerAircraft.AircraftId }));
                    
                    var link = "https://" + _httpContextAccessor.HttpContext.Request.Host + "/outside-the-gate-layout/auth?token=" + HttpUtility.UrlEncode(authentication.AccessToken);

                    var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridAutomatedFuelOrderNotificationTemplateData
                    {
                        aircraftTailNumber = customerAircraft.TailNumber,
                        fboName = authentication.Fbo,
                        flightDepartment = customer.Company,
                        aircraftMakeModel = aircraft.Make + " " + aircraft.Model,
                        airportICAO = fbo.FboAirport.Icao,
                        arrivalDate = arrivalDateTime == "" ? request.Arrival : arrivalDateTime,
                        departureDate = departureDateTime == "" ? request.Departure : departureDateTime,
                        fuelVolume = quotedVolume.ToString(),
                        fuelVendor = orderDetails.FuelVendor.ToLower().Contains("fbolinx") ? fbo.Fbo : orderDetails.FuelVendor,
                        paymentMethod = orderDetails.PaymentMethod,
                        services = string.Join(", ", serviceNames),
                        callSign = request.CallSign,
                        buttonUrl = link
                    };

                    var fboContacts = await GetFboContacts(fbo.Oid);
                    var userContacts = await GetUserContacts(fbo);

                    var fboEmails = authentication.FboEmails + (fboContacts == "" ? "" : ";" + fboContacts) + (userContacts == "" ? "" : ";" + userContacts);

                    var result = await GenerateFuelOrderMailMessage(authentication.Fbo, fboEmails, dynamicTemplateData);

                    return result;
                }
                return false;
            }
            return false;
        }

        public async Task AddServiceOrder(ServiceOrderRequest request, FbosDto fbo)
        {
            var customersWithTail = await _customerInfoByGroupService.GetCustomers(fbo.GroupId, new List<string>() { request.TailNumber });
            var customer = customersWithTail.Where(c => c.Customer.FuelerlinxId == request.CompanyId).FirstOrDefault();
            var customerAircraft = customer.Customer.CustomerAircrafts.Where(c => c.TailNumber == request.TailNumber).FirstOrDefault();

            var serviceReq = new ServiceOrderDto()
            {
                Fboid = fbo.Oid,
                CustomerAircraftId = customerAircraft.Oid,
                AssociatedFuelOrderId = request.FboLinxFuelOrderId,
                FuelerLinxTransactionId = request.SourceId,
                ServiceOn = request.FuelOn == "Arrival" ? Core.Enums.ServiceOrderAppliedDateTypes.Arrival : Core.Enums.ServiceOrderAppliedDateTypes.Departure,
                CustomerInfoByGroupId = customer.Oid,
                GroupId = customer.GroupId
            };

            if (request.TimeStandard == "Local" || request.TimeStandard == "L")
            {
                var etaUtc = await _dateTimeService.ConvertLocalTimeToUtc(fbo.Oid, request.Eta.GetValueOrDefault());
                var etdUtc = await _dateTimeService.ConvertLocalTimeToUtc(fbo.Oid, request.Etd.GetValueOrDefault());

                serviceReq.ServiceDateTimeUtc = serviceReq.ServiceOn == Core.Enums.ServiceOrderAppliedDateTypes.Arrival ? etaUtc : etdUtc;
                serviceReq.ArrivalDateTimeUtc = etaUtc;
                serviceReq.DepartureDateTimeUtc = etdUtc;
            }
            else
            {
                serviceReq.ServiceDateTimeUtc = serviceReq.ServiceOn == Core.Enums.ServiceOrderAppliedDateTypes.Arrival ? request.Eta.GetValueOrDefault() : request.Etd.GetValueOrDefault();
                serviceReq.ArrivalDateTimeUtc = request.Eta;
                serviceReq.DepartureDateTimeUtc = request.Etd;
            }

            serviceReq = await _serviceOrderService.AddNewOrder(serviceReq);

            var serviceOrderItems = new List<ServiceOrderItemDto>();
            foreach (string serviceOrderName in request.ServiceNames)
            {
                ServiceOrderItemDto serviceOrderItem = new ServiceOrderItemDto();
                serviceOrderItem.ServiceOrderId = serviceReq.Oid;
                serviceOrderItem.ServiceName = serviceOrderName;
                serviceOrderItem.IsCompleted = false;
                serviceOrderItems.Add(serviceOrderItem);
            }
            await _serviceOrderItemService.BulkInsert(serviceOrderItems);
        }

        public async Task CheckAndSendFuelOrderUpdateEmail(FuelReqRequest fuelerlinxTransaction)
        {
            // Get order details
            OrderDetailsDto orderDetails = await _orderDetailsService.GetSingleBySpec(new OrderDetailsByFuelerLinxTransactionIdSpecification(fuelerlinxTransaction.SourceId.GetValueOrDefault()));
            
            if (orderDetails != null && orderDetails.Oid > 0)
            {
                var sendEmail = false;
                var requestStatus = "updated";
                var isUpdated = false;
                var fuelReq = await GetSingleBySpec(new FuelReqBySourceIdSpecification(fuelerlinxTransaction.SourceId.GetValueOrDefault()));

                if (!fuelerlinxTransaction.IsCancelled)
                {
                    var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(fuelerlinxTransaction.FboHandlerId));
                    var customer = await _customerService.GetSingleBySpec(new CustomerByFuelerLinxIdSpecification(fuelerlinxTransaction.CompanyId.GetValueOrDefault()));
                    var customerAircrafts = await _customerAircraftService.GetAircraftsList(fbo.GroupId, fbo.Oid);
                    var customerAircraft = customerAircrafts.Where(c => c.TailNumber == fuelerlinxTransaction.TailNumber && c.CustomerId == customer.Oid).FirstOrDefault();

                    if (customerAircraft != null && orderDetails.CustomerAircraftId != customerAircraft.Oid)
                    {
                        orderDetails.CustomerAircraftId = customerAircraft.Oid;
                        sendEmail = true;
                        isUpdated = true;
                    }

                    if (orderDetails.Eta.GetValueOrDefault() != fuelerlinxTransaction.Eta)
                    {
                        TimeSpan ts = fuelerlinxTransaction.Eta.GetValueOrDefault() - orderDetails.Eta.GetValueOrDefault();
                        if (Math.Abs(ts.TotalMinutes) > 60)
                            sendEmail = true;
                        orderDetails.Eta = fuelerlinxTransaction.Eta;
                        isUpdated = true;
                    }

                    if (orderDetails.FuelVendor != fuelerlinxTransaction.FuelVendor)
                    {
                        orderDetails.FuelVendor = fuelerlinxTransaction.FuelVendor;
                        sendEmail = true;
                        isUpdated = true;
                    }

                    if (isUpdated)
                    {
                        orderDetails.DateTimeUpdated = DateTime.UtcNow;
                        await _orderDetailsService.UpdateAsync(orderDetails);
                    }

                    if (fuelReq != null && fuelReq.Oid > 0)
                    {
                        fuelReq.CustomerAircraftId = orderDetails.CustomerAircraftId;
                        fuelReq.Eta = orderDetails.Eta;
                        await UpdateAsync(fuelReq);
                    }

                    if (sendEmail && orderDetails.DateTimeEmailSent != null && DateTime.UtcNow - orderDetails.DateTimeEmailSent.Value < TimeSpan.FromHours(4) && DateTime.UtcNow - orderDetails.Eta.GetValueOrDefault() > TimeSpan.FromMinutes(30))
                        sendEmail = false;
                }
                else
                {
                    sendEmail = true;
                    requestStatus = "cancelled";
                }

                if (sendEmail)
                {
                    var success = await SendFuelOrderUpdateEmail(orderDetails.FuelVendor, fuelerlinxTransaction.FboHandlerId, requestStatus, orderDetails.FuelerLinxTransactionId);
                    if (success)
                    {
                        orderDetails.IsEmailSent = true;
                        orderDetails.DateTimeEmailSent = DateTime.UtcNow;
                        await _orderDetailsService.UpdateAsync(orderDetails);
                    }
                }
            }
        }

        private async Task<bool> SendFuelOrderUpdateEmail(string fuelVendor, int fboAcukwikHandlerId, string requestStatus, int sourceId)
        {
            var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(fboAcukwikHandlerId));

            var canSendEmail = false;
            if (fbo == null || (!fuelVendor.ToLower().Contains("fbolinx") && fbo != null && fbo.Preferences != null && fbo.Preferences.Oid > 0 && ((fbo.Preferences.OrderNotificationsEnabled.HasValue && fbo.Preferences.OrderNotificationsEnabled.Value) || (fbo.Preferences.OrderNotificationsEnabled == null))))
                canSendEmail = true;
            else if (fbo == null || fuelVendor.ToLower().Contains("fbolinx") && fbo != null && fbo.Preferences != null && fbo.Preferences.Oid > 0 && ((fbo.Preferences.DirectOrderNotificationsEnabled.HasValue && fbo.Preferences.DirectOrderNotificationsEnabled.Value) || (fbo.Preferences.DirectOrderNotificationsEnabled == null)))
                canSendEmail = true;

            // SEND EMAIL IF SETTING IS ON OR NOT FBOLINX CUSTOMER
            if (canSendEmail)
            {
                var authentication = await _AuthService.CreateAuthenticatedLink(fbo.AcukwikFBOHandlerId.GetValueOrDefault());

                if (authentication.FboEmails != "FBO not found" && authentication.FboEmails != "No email found")
                {
                    var link = "https://" + _httpContextAccessor.HttpContext.Request.Host + "/outside-the-gate-layout/auth?token=" + HttpUtility.UrlEncode(authentication.AccessToken) + "&id=" + sourceId; ;

                    var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridFuelRequestUpdateOrCancellationTemplateData
                    {
                        buttonUrl = link,
                        requestStatus = requestStatus
                    };

                    var fboContacts = await GetFboContacts(fbo.Oid);
                    var userContacts = await GetUserContacts(fbo);

                    var fboEmails = authentication.FboEmails + (fboContacts == "" ? "" : ";" + fboContacts) + (userContacts == "" ? "" : ";" + userContacts);

                    var result = await GenerateFuelOrderMailMessage(authentication.Fbo, fboEmails, null, dynamicTemplateData);

                    return result;
                }
                return false;
            }
            return false;
        }

        private async Task<string> GetFboContacts(int fboId)
        {
            var fboContacts = await _FboContactsEntityService.GetFboContactsByFboId(fboId);
            var fboContactsToEmail = fboContacts.Where(f => f.CopyOrders.HasValue && f.CopyOrders.Value)
                                       .Select(f => new Contacts()
                                       {
                                           Email = f.Email,
                                       })
                                       .ToList();

            if (fboContactsToEmail.Count == 0)
                return "";
            return System.String.Join(";", fboContacts);
        }

        private async Task<string> GetUserContacts(FbosDto fbo)
        {
            List<string> alertEmailAddressesUsers = await _context.User.Where(x => x.GroupId == fbo.GroupId && (x.FboId == 0 || x.FboId == fbo.Oid) && x.CopyOrders.HasValue && x.CopyOrders.Value).Select(x => x.Username).AsNoTracking().ToListAsync();
            if (alertEmailAddressesUsers.Count == 0)
                return "";
            return System.String.Join(";", alertEmailAddressesUsers);
        }

        private async Task<bool> GenerateFuelOrderMailMessage(string fbo, string fboEmails, SendGridAutomatedFuelOrderNotificationTemplateData dynamicAutomatedFuelOrderNotificationTemplateData = null, SendGridFuelRequestUpdateOrCancellationTemplateData dynamicFuelRequestUpdateOrCancellationTemplateDate = null)
        {
            try
            {
                //Add email content to MailMessage
                FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
                mailMessage.From = new MailAddress("donotreply@fbolinx.com");
                foreach (string fboEmail in fboEmails.Split(';'))
                {
                    if (_MailService.IsValidEmailRecipient(fboEmail))
                        mailMessage.To.Add(fboEmail);
                }

                if (dynamicAutomatedFuelOrderNotificationTemplateData != null)
                    mailMessage.SendGridAutomatedFuelOrderNotificationTemplateData = dynamicAutomatedFuelOrderNotificationTemplateData;
                else if (dynamicFuelRequestUpdateOrCancellationTemplateDate != null)
                    mailMessage.SendGridFuelRequestUpdateOrCancellationTemplateData = dynamicFuelRequestUpdateOrCancellationTemplateDate;

                //Send email
                var result = _MailService.SendAsync(mailMessage).Result;

                return result;
            }
            catch (System.Exception exception)
            {
                return false;
            }
        }
    }
}