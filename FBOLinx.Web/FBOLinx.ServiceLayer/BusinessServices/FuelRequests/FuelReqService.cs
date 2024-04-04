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
using EllipticCurve.Utils;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Azure.Data.Tables;

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
        Task<bool> SendFuelOrderNotificationEmail(int handlerId, int fuelerlinxTransactionId, int fuelerlinxCompanyId, SendOrderNotificationRequest request, bool isCancelled = false, FuelReqDto fuelReq = null);
        Task AddServiceOrder(ServiceOrderRequest request, FbosDto fbo);
        Task CheckAndSendFuelOrderUpdateEmail(FuelReqRequest fuelerlinxTransaction);
        Task<FuelReqDto> CreateFuelOrder(FuelReqDto request);
        Task<FuelReqDto> GetContractOrder(int fboId, int fuelerLinxTransactionId);
    }

    public class FuelReqService : BaseDTOService<FuelReqDto, DB.Models.FuelReq, FboLinxContext>, IFuelReqService
    {
        private int _CacheLifeSpanInMinutes = 10;
        private string _UpcomingOrdersCacheKeyPrefix = "UpcomingOrders_ByGroupAndFbo_";
        private int _HoursToLookBackForUpcomingOrders = 12;
        private int _HoursToLookForwardForUpcomingOrders = 12;

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
        private readonly IFuelReqConfirmationEntityService _fuelReqConfirmationEntityService;
        private readonly ICustomerService _customerService;
        private readonly IOrderDetailsEntityService _orderDetailsEntityService;

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
            IFuelReqConfirmationEntityService fuelReqConfirmationEntityService, ICustomerService customerService, IOrderDetailsEntityService orderDetailsEntityService) : base(fuelReqEntityService)
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
            _fuelReqConfirmationEntityService = fuelReqConfirmationEntityService;
            _customerService = customerService;
            _orderDetailsEntityService = orderDetailsEntityService;
        }

        public async Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrdersForTailNumber(int groupId, int fboId,
            string tailNumber,
            bool useCache = true)
        {
            try
            {
                tailNumber = tailNumber.Trim().ToUpper();
                var upcomingOrders = await GetUpcomingDirectAndContractOrders(groupId, fboId, useCache);
                return upcomingOrders?.Where(x => x.TailNumber?.ToUpper() == tailNumber).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrders(int groupId, int fboId,
            bool useCache = true)
        {
            List<FuelReqDto> result = null;
            try
            {
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
            }
            catch(Exception ex)
            {

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

            var orders = await GetDirectOrdersFromDatabase(fboId, startDate, endDate, customers);

            //var result = new List<FuelReqsGridViewModel>();
            //if (orders != null)
            //{
            //    foreach (FuelReqDto fuelReq in orders)
            //    {
            //        FuelReqsGridViewModel fuelReqViewModel = new FuelReqsGridViewModel();
            //        fuelReqViewModel.Cast(fuelReq);
            //        result.Add(fuelReqViewModel);
            //    }
            //}
            //return result;
            return orders;
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

            var serviceOrders = await _serviceOrderService.GetListbySpec(new ServiceOrderByFboSpecification(fboId, startDateTime, endDateTime));

            try
            {
                //Direct orders
                var directOrders = await GetDirectOrdersFromDatabase(fboId, startDateTime, endDateTime, customers);
                var directOrderIds = directOrders.Select(s => s.SourceId.GetValueOrDefault()).ToList();
                var orderDetails = await _orderDetailsEntityService.GetOrderDetailsByIds(directOrderIds);
                var orderConfirmations = await _fuelReqConfirmationEntityService.GetFuelReqConfirmationByIds(directOrderIds);

                var airport = new GeneralAirportInformation();
                if (directOrders.Count > 0)
                    airport = await _AirportService.GetGeneralAirportInformation(directOrders[0].Icao);
                else
                    airport = await _AirportService.GetGeneralAirportInformation(fboRecord.FboAirport.Icao);

                foreach (FuelReqDto order in directOrders)
                {
                    var transactionOrderDetails = orderDetails.Where(o => (order.SourceId > 0 && order.SourceId == o.FuelerLinxTransactionId) || (o.AssociatedFuelOrderId == order.Oid)).FirstOrDefault();
                    if (order.TimeStandard == "Z")
                    {
                        order.Eta = FuelReqDto.GetAirportLocalTime(order.Eta.GetValueOrDefault(), airport);
                        order.Etd = FuelReqDto.GetAirportLocalTime(order.Etd.GetValueOrDefault(), airport);
                    }
                    order.IsConfirmed = orderConfirmations.Any(x => x.SourceId == order.SourceId);
                    order.Fboid = fboId;
                    order.ServiceOrder = serviceOrders.Where(s => (s.FuelerLinxTransactionId > 0 && s.FuelerLinxTransactionId == order.SourceId) || (s.AssociatedFuelOrderId > 0 && s.AssociatedFuelOrderId == order.Oid)).FirstOrDefault();
                    order.PaymentMethod = orderDetails.Where(o => (o.FuelerLinxTransactionId == order.SourceId) || (o.AssociatedFuelOrderId == order.Oid)).Select(d => d.PaymentMethod).FirstOrDefault();
                    if (transactionOrderDetails != null)
                    {
                        if (!order.Archived.GetValueOrDefault())
                            order.Archived = transactionOrderDetails.IsArchived;
                        if (transactionOrderDetails.IsOkToEmail != null)
                            order.ShowConfirmationButton = transactionOrderDetails.IsOkToEmail.GetValueOrDefault();
                    }
                    result.Add(order);
                }

                //Contract orders
                FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _fuelerLinxService.GetContractFuelRequests(new FBOLinxOrdersRequest()
                { EndDateTime = endDateTime, StartDateTime = startDateTime, Icao = fboRecord.FboAirport?.Icao, Fbo = fboRecord.Fbo });
                var fuelerlinxContractFuelOrdersIds = fuelerlinxContractFuelOrders.Result.Where(s => s.Id > 0).Select(s => s.Id).ToList();
                orderDetails = await _orderDetailsEntityService.GetOrderDetailsByIds(fuelerlinxContractFuelOrdersIds);
                orderConfirmations = await _fuelReqConfirmationEntityService.GetFuelReqConfirmationByIds(fuelerlinxContractFuelOrdersIds);

                if ((airport == null || airport.AirportId == 0) && fuelerlinxContractFuelOrders.Result.Count > 0)
                    airport = await _AirportService.GetGeneralAirportInformation(fuelerlinxContractFuelOrders.Result.FirstOrDefault().Icao);

                foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result)
                {
                    if (transaction.DispatchedVolume.Amount > 0 && !directOrders.Any(d => d.SourceId == transaction.Id))
                    {
                        var isConfirmed = orderConfirmations.Any(x => x.SourceId == transaction.Id);
                        var fuelRequest = FuelReqDto.Cast(transaction, customers.Where(x => x.Customer?.FuelerlinxId == transaction.CompanyId).Select(x => x.Company).FirstOrDefault(), airport, isConfirmed);
                        fuelRequest.Cancelled = transaction.InvoiceStatus == TransactionInvoiceStatuses.Cancelled ? true : false;
                        fuelRequest.ServiceOrder = serviceOrders.Where(s => s.FuelerLinxTransactionId > 0 && s.FuelerLinxTransactionId == fuelRequest.SourceId).FirstOrDefault();
                        fuelRequest.PaymentMethod = orderDetails.Where(o => o.FuelerLinxTransactionId == fuelRequest.SourceId).Select(d => d.PaymentMethod).FirstOrDefault();
                        fuelRequest.Fboid = fboId;
                        fuelRequest.Source = fuelRequest.Source.Replace("Directs: Custom", "Flight Dept.");

                        var transactionOrderDetails = orderDetails.Where(o => o.FuelerLinxTransactionId == fuelRequest.SourceId).FirstOrDefault();
                        if (transactionOrderDetails != null)
                        {
                            fuelRequest.Archived = transactionOrderDetails.IsArchived;
                            if (transactionOrderDetails.IsOkToEmail != null)
                                fuelRequest.ShowConfirmationButton = transactionOrderDetails.IsOkToEmail.GetValueOrDefault();
                        }
                        fuelRequest.CustomerId = customers.Where(c => c.Customer.FuelerlinxId == fuelRequest.CustomerId).FirstOrDefault().CustomerId;
                        result.Add(fuelRequest);
                    }
                }

                //Service orders
                List<FuelReqDto> serviceOrdersList = new List<FuelReqDto>();
                var serviceOrderIds = serviceOrders.Where(s => s.FuelerLinxTransactionId > 0).Select(s => s.FuelerLinxTransactionId.GetValueOrDefault()).ToList();
                orderDetails = await _orderDetailsEntityService.GetOrderDetailsByIds(serviceOrderIds);
                orderConfirmations = await _fuelReqConfirmationEntityService.GetFuelReqConfirmationByIds(serviceOrderIds);
                var customerAircrafts = await _customerAircraftService.GetAircraftsList(groupId, fboId);

                foreach (ServiceOrderDto item in serviceOrders)
                {
                    if (!result.Any(f => f.SourceId == item.FuelerLinxTransactionId))
                    {
                        var transactionOrderDetails = orderDetails.Where(o => (o.FuelerLinxTransactionId == item.FuelerLinxTransactionId) || (o.AssociatedFuelOrderId == item.AssociatedFuelOrderId)).FirstOrDefault();

                        var fuelreq = new FuelReqDto()
                        {
                            Oid = item.Oid,
                            ActualPpg = 0,
                            ActualVolume = 0,
                            Archived = transactionOrderDetails.IsArchived,
                            Cancelled = transactionOrderDetails.IsCancelled,
                            CustomerId = item.CustomerInfoByGroup?.CustomerId,
                            //DateCreated = item.ServiceDateTimeUtc,//check this property
                            DispatchNotes = string.Empty,
                            Eta = item.ArrivalDateTimeLocal,
                            Etd = item.DepartureDateTimeLocal,
                            Icao = string.Empty,
                            Notes = string.Empty,
                            QuotedPpg = 0,
                            QuotedVolume = 0,
                            Source = "Service only",
                            SourceId = item.FuelerLinxTransactionId,
                            TimeStandard = null,
                            TailNumber = customerAircrafts.Where(c => c.Oid == item.CustomerAircraftId).Select(a => a.TailNumber).FirstOrDefault(),
                            FboName = string.Empty,
                            Email = string.Empty,
                            PhoneNumber = item.CustomerInfoByGroup?.MainPhone,
                            FuelOn = string.Empty,
                            CustomerName = item.CustomerInfoByGroup?.Company,
                            IsConfirmed = orderConfirmations.Any(x => x.SourceId == item.FuelerLinxTransactionId),
                            PaymentMethod = transactionOrderDetails.PaymentMethod,
                            ServiceOrder = item,
                            ShowConfirmationButton = transactionOrderDetails.IsOkToEmail.GetValueOrDefault()
                        };
                        serviceOrdersList.Add(fuelreq);
                    }
                }

                // Cancelled contract orders
                orderDetails = await _orderDetailsEntityService.GetListBySpec(new OrderDetailsByFboHandlerIdSpecifications(fboRecord.AcukwikFBOHandlerId.GetValueOrDefault()));
                orderDetails = orderDetails.Where(o => o.IsCancelled == true).ToList();

                foreach (OrderDetails item in orderDetails)
                {
                    if (!result.Any(f => f.SourceId == item.FuelerLinxTransactionId) && !directOrders.Any(d => d.SourceId == item.FuelerLinxTransactionId) && !serviceOrdersList.Any(r => r.SourceId == item.FuelerLinxTransactionId))
                    {
                        var fuelreq = new FuelReqDto()
                        {
                            Oid = item.Oid,
                            ActualPpg = 0,
                            ActualVolume = 0,
                            Archived = false,
                            Cancelled = true,
                            CustomerId = customerAircrafts.Where(c => c.Oid == item.CustomerAircraftId).Select(a => a.CustomerId).FirstOrDefault(),
                            //DateCreated = item.ServiceDateTimeUtc,//check this property
                            DispatchNotes = string.Empty,
                            Eta = item.Eta,
                            Icao = string.Empty,
                            Notes = string.Empty,
                            QuotedPpg = 0,
                            QuotedVolume = 0,
                            Source = string.Empty,
                            SourceId = item.FuelerLinxTransactionId,
                            TimeStandard = null,
                            TailNumber = customerAircrafts.Where(c => c.Oid == item.CustomerAircraftId).Select(a => a.TailNumber).FirstOrDefault(),
                            FboName = string.Empty,
                            Email = string.Empty,
                            FuelOn = string.Empty,
                            IsConfirmed = orderConfirmations.Any(x => x.SourceId == item.FuelerLinxTransactionId),
                            PaymentMethod = orderDetails.Where(o => o.FuelerLinxTransactionId == item.FuelerLinxTransactionId).Select(d => d.PaymentMethod).FirstOrDefault(),
                            ShowConfirmationButton = false
                        };
                        result.Add(fuelreq);
                    }
                }

                foreach (FuelReqDto fuelReq in result)
                {
                    if (string.IsNullOrEmpty(fuelReq.CustomerName))
                        fuelReq.CustomerName = customers.Where(c => c.CustomerId == fuelReq.CustomerId).Select(cu => cu.Company).FirstOrDefault();

                    if (fuelReq.ServiceOrder == null)
                    {
                        fuelReq.ServiceOrder = new ServiceOrderDto();
                        fuelReq.ServiceOrder.ServiceOrderItems = new List<ServiceOrderItemDto>();
                    }
                }

                result.AddRange(serviceOrdersList);
                return result;
            }
            catch(Exception ex)
            {

            }
            return null;
        }

        public async Task<FuelReqDto> GetContractOrder(int fboId, int fuelerLinxTransactionId)
        {
            var result = new FuelReqDto();
            var fboRecord = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));
            if (fboRecord == null)
                return result;

            var airport = await _AirportService.GetGeneralAirportInformation(fboRecord.FboAirport.Icao);

            var customers =
               await _CustomerInfoByGroupEntityService.GetListBySpec(
                   new CustomerInfoByGroupByGroupIdSpecification(fboRecord.GroupId));

            var customerAircrafts = await _customerAircraftService.GetAircraftsList(fboRecord.GroupId, fboRecord.Oid);

            FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _fuelerLinxService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = DateTime.Now, StartDateTime = DateTime.Now, Icao = fboRecord.FboAirport.Icao, Fbo = fboRecord.Fbo, FuelerLinxTransactionId = fuelerLinxTransactionId });

            if (fuelerlinxContractFuelOrders.Result.Count > 0)
            {
                var transaction = fuelerlinxContractFuelOrders.Result.FirstOrDefault();
                var fuelRequest = FuelReqDto.Cast(transaction, customers.Where(x => x.Customer?.FuelerlinxId == transaction.CompanyId).Select(x => x.Company).FirstOrDefault(), airport);
                fuelRequest.Cancelled = transaction.InvoiceStatus == TransactionInvoiceStatuses.Cancelled ? true : false;
                fuelRequest.Fboid = fboId;
                fuelRequest.Source = fuelRequest.Source.Replace("Directs: Custom", "Flight Dept.");
                fuelRequest.CustomerId = customers.Where(c => c.Customer.FuelerlinxId == fuelRequest.CustomerId).FirstOrDefault().CustomerId;
                fuelRequest.CustomerAircraftId = customerAircrafts.Where(c => c.TailNumber == fuelRequest.TailNumber && c.CustomerId==fuelRequest.CustomerId).FirstOrDefault().Oid;
                result = fuelRequest;
            }

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

        public async Task<bool> SendFuelOrderNotificationEmail(int handlerId, int fuelerlinxTransactionId, int fuelerlinxCompanyId, SendOrderNotificationRequest request, bool isCancelled = false, FuelReqDto fuelReq = null)
        {
            var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(handlerId));

            // Get order details
            OrderDetailsDto orderDetails = await _orderDetailsService.GetSingleBySpec(new OrderDetailsByFuelerLinxTransactionIdFboHandlerIdSpecification(fuelerlinxTransactionId, handlerId));

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

                    if (fuelReq == null)
                        fuelReq = await GetSingleBySpec(new FuelReqBySourceIdFboIdSpecification(fuelerlinxTransactionId, fbo.Oid));
                    if (fuelReq != null)
                    {
                        customerAircraftId = fuelReq.CustomerAircraftId.GetValueOrDefault();
                        arrivalDateTime = fuelReq.Eta.ToString() + " " + (fuelReq.TimeStandard == "L" ? "(Local)" : "(Zulu)");
                        departureDateTime = fuelReq.Etd.ToString() + " " + (fuelReq.TimeStandard == "L" ? "(Local)" : "(Zulu)");
                        quotedVolume = fuelReq.QuotedVolume.GetValueOrDefault();
                    }

                    // Get service request
                    ServiceOrderDto serviceOrder = await _serviceOrderService.GetSingleBySpec(new ServiceOrderByFuelerLinxTransactionIdFboIdSpecification(fuelerlinxTransactionId, fbo.Oid));
                    var serviceNames = new List<string>();
                    if (serviceOrder != null)
                    {
                        serviceNames = serviceOrder.ServiceOrderItems.Select(s => s.ServiceName + (s.ServiceNote != null && s.ServiceNote != "" ? ": " + s.ServiceNote + "\n" : "")).ToList();

                        if (customerAircraftId == 0)
                            customerAircraftId = serviceOrder.CustomerAircraftId;

                        if (arrivalDateTime == "")
                        {
                            if (orderDetails.TimeStandard == "Z")
                            {
                                arrivalDateTime = serviceOrder.ArrivalDateTimeUtc.ToString() + " (Zulu)";
                                departureDateTime = serviceOrder.DepartureDateTimeUtc.ToString() + " (Zulu)";
                            }
                            else
                            {
                                arrivalDateTime = await _AirportTimeService.GetAirportLocalDateTime(fbo.Oid, serviceOrder.ArrivalDateTimeUtc) + " (Local)";
                                departureDateTime = await _AirportTimeService.GetAirportLocalDateTime(fbo.Oid, serviceOrder.DepartureDateTimeUtc) + " (Local)";
                            }
                        }
                    }

                    if (customer.Oid == 0 || fuelReq == null || fuelReq.Oid == 0)
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

                    //var link = "https://" + _httpContextAccessor.HttpContext.Request.Host + "/outside-the-gate-layout/auth?token=" + HttpUtility.UrlEncode(authentication.AccessToken);
                    //var link = "https://fbolinx.com/#about-content";

                    // SERVICES
                    var services = new List<ServicesForSendGrid>();
                    if (serviceNames != null && serviceNames.Count > 0)
                    {
                        int serviceCount = 1;
                        foreach (var service in serviceNames)
                        {
                            if (service.StartsWith("Fuel") && quotedVolume > 0)
                            {
                                var vendor = "";
                                if (orderDetails.FuelVendor == "Directs: Custom")
                                    vendor = "Flight Dept.";
                                else if (orderDetails.FuelVendor.ToLower() == "fbolinx")
                                    vendor = fbo.Fbo + " (FBOLinx Direct)";
                                else
                                    vendor = orderDetails.FuelVendor;
                                services.Add(new ServicesForSendGrid { service = serviceCount + ". " + service + " " + "via " + vendor});
                            }
                            else
                                services.Add(new ServicesForSendGrid { service = serviceCount + ". " + service });
                            serviceCount++;
                        }
                    }

                    // FLIGHT DEPARTMENT INFO
                    var flightDepartmentInfo = new List<FlightDepartmentInfoForSendGrid>();
                    if (fuelReq != null && !string.IsNullOrEmpty(fuelReq.Email))
                        flightDepartmentInfo.Add(new FlightDepartmentInfoForSendGrid { info = "Email: " + fuelReq.Email }); 
                    else
                        flightDepartmentInfo.Add(new FlightDepartmentInfoForSendGrid { info = "Email: " + orderDetails.ConfirmationEmail });
                    if (fuelReq != null && !string.IsNullOrEmpty(fuelReq.PhoneNumber))
                        flightDepartmentInfo.Add(new FlightDepartmentInfoForSendGrid { info = "Phone: " + fuelReq.PhoneNumber });
                    if (!string.IsNullOrEmpty(request.CallSign))
                        flightDepartmentInfo.Add(new FlightDepartmentInfoForSendGrid { info = "Call Sign: " + request.CallSign });
                    if (customerAircraft != null)
                        flightDepartmentInfo.Add(new FlightDepartmentInfoForSendGrid { info = "Aircraft: " + aircraft.Make + " " + aircraft.Model });
                    if (!string.IsNullOrEmpty(orderDetails.PaymentMethod))
                        flightDepartmentInfo.Add(new FlightDepartmentInfoForSendGrid { info = "Payment Method: " + orderDetails.PaymentMethod });

                    var dynamicCancellationTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridAutomatedFuelOrderCancellationTemplateData();
                    var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridAutomatedFuelOrderNotificationTemplateData();

                    if (isCancelled) //CANCELLATION NOTIFICATION
                    {
                        dynamicCancellationTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridAutomatedFuelOrderCancellationTemplateData
                        {
                            fboName = authentication.Fbo,
                            flightDepartment = customer.Company,
                            tailNumber = customerAircraft.TailNumber,
                            airportICAO = fbo.FboAirport.Icao,
                            arrivalDate = arrivalDateTime == "" ? request.Arrival : arrivalDateTime,
                            departureDate = departureDateTime == "" ? request.Departure : departureDateTime,
                            services = services,
                            flightDepartmentInfo = flightDepartmentInfo,
                        };
                    }
                    else //AUTOMATED ORDER NOTIFICATION
                    {
                        dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridAutomatedFuelOrderNotificationTemplateData
                        {
                            fboName = authentication.Fbo,
                            flightDepartment = customer.Company,
                            tailNumber = customerAircraft.TailNumber,
                            airportICAO = fbo.FboAirport.Icao,
                            arrivalDate = arrivalDateTime == "" ? request.Arrival : arrivalDateTime,
                            departureDate = departureDateTime == "" ? request.Departure : departureDateTime,
                            services = services,
                            flightDepartmentInfo = flightDepartmentInfo,
                        };
                    }

                    var fboContacts = await GetFboContacts(fbo.Oid);
                    var userContacts = await GetUserContacts(fbo);

                    var fboEmails = authentication.FboEmails + (fboContacts == "" ? "" : ";" + fboContacts) + (userContacts == "" ? "" : ";" + userContacts);

                    var result = await GenerateFuelOrderMailMessage(authentication.Fbo, fboEmails, dynamicTemplateData.airportICAO != null ? dynamicTemplateData : null, dynamicCancellationTemplateData.airportICAO != null ? dynamicCancellationTemplateData : null);

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

            var serviceReq = await _serviceOrderService.GetSingleBySpec(new ServiceOrderByFuelerLinxTransactionIdFboIdSpecification(request.SourceId.GetValueOrDefault(), fbo.Oid));

            if (serviceReq == null || serviceReq.Oid == 0)
            {
                serviceReq = new ServiceOrderDto()
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
            }

            var serviceOrderItems = new List<ServiceOrderItemDto>();
            foreach (var service in request.Services)
            {
                ServiceOrderItemDto serviceOrderItem = new ServiceOrderItemDto();
                serviceOrderItem.ServiceOrderId = serviceReq.Oid;
                serviceOrderItem.ServiceName = service.ServiceName;
                serviceOrderItem.IsCompleted = false;
                serviceOrderItem.ServiceNote = service.Note;
                serviceOrderItems.Add(serviceOrderItem);
            }

            //// Add default "Fuel" service if it doesn't already exist
            //if (serviceReq.ServiceOrderItems == null || serviceReq.ServiceOrderItems.Count == 0)
            //{
            //    ServiceOrderItemDto fuelServiceOrderItem = new ServiceOrderItemDto();
            //    fuelServiceOrderItem.ServiceOrderId = serviceReq.Oid;
            //    fuelServiceOrderItem.ServiceName = "Fuel 0 gal.";
            //    fuelServiceOrderItem.IsCompleted = false;
            //    serviceOrderItems.Add(fuelServiceOrderItem);
            //}

            await _serviceOrderItemService.BulkInsert(serviceOrderItems);
        }

        public async Task<FuelReqDto> CreateFuelOrder(FuelReqDto request)
        {
            var customerAircraft = await _customerAircraftService.GetSingleBySpec(new CustomerAircraftSpecification(request.CustomerAircraftId.GetValueOrDefault()));
            var fbo = await _fboService.GetFbo(request.Fboid.GetValueOrDefault());
            var icao = fbo.FboAirport.Icao;

            request.Icao = icao;
            request.CustomerId = customerAircraft.CustomerId;

            request = await AddAsync(request);

            var customer = await _customerInfoByGroupService.GetSingleBySpec(new CustomerInfoByGroupByCustomerIdSpecification(request.CustomerId.GetValueOrDefault()));
            request.CustomerName = customer.Company;
            request.TailNumber = customerAircraft.TailNumber;
            request.CustomerId = customer.CustomerId;

            var orderDetails = new OrderDetailsDto();
            orderDetails.ConfirmationEmail = request.Email;
            orderDetails.FuelVendor = "FBO Custom";
            orderDetails.QuotedVolume = request.QuotedVolume;
            orderDetails.Eta = request.Eta;
            orderDetails.FboHandlerId = fbo.AcukwikFBOHandlerId;
            orderDetails.CustomerAircraftId = customerAircraft.Oid;
            orderDetails.AssociatedFuelOrderId = request.Oid;
            orderDetails = await _orderDetailsService.AddAsync(orderDetails);

            return request;
        }

        public async Task CheckAndSendFuelOrderUpdateEmail(FuelReqRequest fuelerlinxTransaction)
        {
            // Get order details
            List<OrderDetailsDto> orderDetailsList = await _orderDetailsService.GetListbySpec(new OrderDetailsByFuelerLinxTransactionIdSpecification(fuelerlinxTransaction.SourceId.GetValueOrDefault()));
            var orderDetails = orderDetailsList.Where(o => o.FboHandlerId == fuelerlinxTransaction.FboHandlerId).FirstOrDefault();
            var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(fuelerlinxTransaction.FboHandlerId));
            var fuelReq = await GetSingleBySpec(new FuelReqBySourceIdFboIdSpecification(fuelerlinxTransaction.SourceId.GetValueOrDefault(), fbo.Oid));
            //var customer = await _customerService.GetSingleBySpec(new CustomerByFuelerLinxIdSpecification(fuelerlinxTransaction.CompanyId.GetValueOrDefault()));
            //var customerAircrafts = await _customerAircraftService.GetAircraftsList(fbo.GroupId, fbo.Oid);
            //var customerAircraft = customerAircrafts.Where(c => c.TailNumber == fuelerlinxTransaction.TailNumber && c.CustomerId == customer.Oid).FirstOrDefault();

            var sendEmail = false;
            var requestStatus = "updated";

            if ((fuelReq != null && fuelReq.Cancelled.GetValueOrDefault()) || (orderDetails != null && orderDetails.IsCancelled.GetValueOrDefault()) || fuelerlinxTransaction.IsCancelled)
            {
                if (fuelReq != null)
                {
                    fuelReq.Cancelled = true;
                    await UpdateAsync(fuelReq);
                }

                orderDetails.IsCancelled = true;
                await _orderDetailsService.UpdateAsync(orderDetails);

                sendEmail = true;
                requestStatus = "cancelled";
            }

            //if (!fuelerlinxTransaction.IsCancelled && orderDetails != null && ((fuelReq != null && fuelReq.Oid > 0 && !fuelerlinxTransaction.IsCancelled) || ((fuelReq == null || fuelReq.Oid == 0 && !orderDetails.IsCancelled.GetValueOrDefault()))))
            //{
            //    //Update fuel service line item for directs
            //    if (fuelReq != null && fuelReq.Oid > 0)
            //    {
            //        var serviceOrder = await _serviceOrderService.GetSingleBySpec(new ServiceOrderByFuelerLinxTransactionIdFboIdSpecification(fuelerlinxTransaction.SourceId.GetValueOrDefault(), fbo.Oid));

            //        var fuelServiceLineItem = serviceOrder.ServiceOrderItems.Where(s => s.ServiceName.StartsWith("Fuel ")).FirstOrDefault();
            //        fuelServiceLineItem.ServiceName = "Fuel " + fuelReq.QuotedVolume + " gal" + (fuelReq.QuotedVolume > 1 ? "s" : "" +  "@ " + fuelReq.QuotedPpg.GetValueOrDefault().ToString("C"));
            //        await _serviceOrderItemService.UpdateAsync(fuelServiceLineItem);
            //    }

            //    var isUpdated = false;

            //    if (orderDetails.FboHandlerId.GetValueOrDefault() == fuelerlinxTransaction.FboHandlerId)
            //    {
            //        if (customerAircraft != null && orderDetails.CustomerAircraftId != customerAircraft.Oid)
            //        {
            //            orderDetails.CustomerAircraftId = customerAircraft.Oid;
            //            sendEmail = true;
            //            isUpdated = true;
            //        }

            //        if (orderDetails.Eta.GetValueOrDefault() != fuelerlinxTransaction.Eta)
            //        {
            //            TimeSpan ts = fuelerlinxTransaction.Eta.GetValueOrDefault() - orderDetails.Eta.GetValueOrDefault();
            //            if (Math.Abs(ts.TotalMinutes) > 60)
            //                sendEmail = true;
            //            orderDetails.Eta = fuelerlinxTransaction.Eta;
            //            isUpdated = true;
            //        }

            //        if (orderDetails.FuelVendor != fuelerlinxTransaction.FuelVendor)
            //        {
            //            orderDetails.FuelVendor = fuelerlinxTransaction.FuelVendor;
            //            sendEmail = true;
            //            isUpdated = true;
            //        }

            //        if (isUpdated)
            //        {
            //            orderDetails.DateTimeUpdated = DateTime.UtcNow;
            //            await _orderDetailsService.UpdateAsync(orderDetails);
            //        }

            //        if (fuelReq != null && fuelReq.Oid > 0)
            //        {
            //            fuelReq.CustomerAircraftId = orderDetails.CustomerAircraftId;
            //            fuelReq.Eta = orderDetails.Eta;
            //            await UpdateAsync(fuelReq);
            //        }

            //        if (orderDetails.DateTimeEmailSent != null && DateTime.UtcNow - orderDetails.DateTimeEmailSent.Value < TimeSpan.FromHours(4) && DateTime.UtcNow - orderDetails.Eta.GetValueOrDefault() > TimeSpan.FromMinutes(30))
            //            sendEmail = false;
            //    }
            //    else // FBO was changed
            //    {
            //        orderDetails = orderDetailsList.Where(o => !o.IsCancelled.GetValueOrDefault()).FirstOrDefault();

            //        fuelerlinxTransaction.FboHandlerId = await FboChangedHandler(orderDetails, customerAircraft, fbo, fuelerlinxTransaction);
            //        orderDetails = await _orderDetailsService.GetSingleBySpec(new OrderDetailsByFuelerLinxTransactionIdFboHandlerIdSpecification(fuelerlinxTransaction.SourceId.GetValueOrDefault(), fuelerlinxTransaction.FboHandlerId));
            //        sendEmail = true;
            //        requestStatus = "cancelled";
            //    }
            //}
            //else  // FBO was changed
            //{
            //    orderDetails = orderDetailsList.Where(o => !o.IsCancelled.GetValueOrDefault()).FirstOrDefault();

            //    fuelerlinxTransaction.FboHandlerId = await FboChangedHandler(orderDetails, customerAircraft, fbo, fuelerlinxTransaction);
            //    orderDetails = await _orderDetailsService.GetSingleBySpec(new OrderDetailsByFuelerLinxTransactionIdFboHandlerIdSpecification(fuelerlinxTransaction.SourceId.GetValueOrDefault(), fuelerlinxTransaction.FboHandlerId));
            //    sendEmail = true;
            //    requestStatus = "cancelled";
            //}

            if ((fuelerlinxTransaction.IsOkToSendEmail != null && fuelerlinxTransaction.IsOkToSendEmail == true) && sendEmail && fuelerlinxTransaction.FboHandlerId > 0)
            {
                var success = await SendFuelOrderNotificationEmail(fuelerlinxTransaction.FboHandlerId, fuelerlinxTransaction.SourceId.GetValueOrDefault(), fuelerlinxTransaction.CompanyId.GetValueOrDefault(), new SendOrderNotificationRequest { QuotedVolume = fuelReq == null ? orderDetails.QuotedVolume.GetValueOrDefault() : fuelReq.QuotedVolume.GetValueOrDefault() }, requestStatus == "cancelled" ? true : false, fuelReq);
                //var success = await SendFuelOrderUpdateEmail(orderDetails.FuelVendor, fuelerlinxTransaction.FboHandlerId, requestStatus, orderDetails.FuelerLinxTransactionId);
                if (success)
                {
                    orderDetails.IsEmailSent = true;
                    orderDetails.DateTimeEmailSent = DateTime.UtcNow;
                    await _orderDetailsService.UpdateAsync(orderDetails);
                }
            }
        }

        private async Task<int> FboChangedHandler(OrderDetailsDto orderDetails, CustomerAircraftsViewModel customerAircraft, FbosDto fbo, FuelReqRequest fuelerlinxTransaction)
        {
            var oldFboHandlerId = orderDetails.FboHandlerId;
            if (oldFboHandlerId != null)
            {

                var oldFbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(oldFboHandlerId.GetValueOrDefault()));

                var fuelReq = await GetSingleBySpec(new FuelReqBySourceIdFboIdSpecification(fuelerlinxTransaction.SourceId.GetValueOrDefault(), oldFbo.Oid));

                // Update old FuelReq record to cancelled for current FBO, add new record for new FBO if it doesn't exist
                if (fuelReq != null && fuelReq.Oid > 0)
                {
                    fuelReq.Cancelled = true;
                    await UpdateAsync(fuelReq);

                    fuelReq.Oid = 0;
                    fuelReq.Cancelled = false;
                    fuelReq.Fboid = fbo.Oid;
                    fuelReq.CustomerAircraftId = customerAircraft.Oid;
                    await AddAsync(fuelReq);
                }

                // Add new ServiceOrder record for new FBO if it doesn't exist
                ServiceOrderDto serviceOrder = await _serviceOrderService.GetSingleBySpec(new ServiceOrderByFuelerLinxTransactionIdFboIdSpecification(fuelerlinxTransaction.SourceId.GetValueOrDefault(), oldFbo.Oid));
                if (serviceOrder != null && serviceOrder.Oid > 0)
                {
                    serviceOrder.Oid = 0;
                    serviceOrder.Fboid = fbo.Oid;
                    serviceOrder.GroupId = fbo.GroupId;
                    await _serviceOrderService.AddAsync(serviceOrder);
                }

                // Update old OrderDetails record for current FBO
                orderDetails.IsCancelled = true;
                orderDetails.DateTimeUpdated = DateTime.UtcNow;
                await _orderDetailsService.UpdateAsync(orderDetails);

                // Add new OrderDetails record for new FBO
                orderDetails.Oid = 0;
                orderDetails.IsCancelled = false;
                orderDetails.FboHandlerId = fbo.AcukwikFBOHandlerId;
                orderDetails.OldFboHandlerId = oldFboHandlerId;
                orderDetails.CustomerAircraftId = customerAircraft.Oid;
                await _orderDetailsService.AddAsync(orderDetails);

                // Send new FBO an email
                SendOrderNotificationRequest request = new SendOrderNotificationRequest();
                request.QuotedVolume = fuelReq == null ? 0 : fuelReq.QuotedVolume.Value;
                request.TailNumber = fuelReq == null ? customerAircraft.TailNumber : fuelReq.TailNumber;
                request.Arrival = fuelReq == null ? (serviceOrder.ArrivalDateTimeUtc.ToString() + " (Zulu)") : (fuelReq.Eta.ToString() + " " + fuelReq.TimeStandard);
                request.Departure = fuelReq == null ? (serviceOrder.DepartureDateTimeUtc.ToString() + " (Zulu)") : (fuelReq.Etd.ToString() + " " + fuelReq.TimeStandard);
                await SendFuelOrderNotificationEmail(fuelerlinxTransaction.FboHandlerId, fuelerlinxTransaction.SourceId.GetValueOrDefault(), fuelerlinxTransaction.CompanyId.GetValueOrDefault(), request);

                return oldFboHandlerId.GetValueOrDefault();
            }
            return 0;
        }

        private async Task<bool> SendFuelOrderUpdateEmail(string fuelVendor, int fboAcukwikHandlerId, string requestStatus, int sourceId)
        {
            //var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(fboAcukwikHandlerId));

            //var canSendEmail = false;
            //if (fbo == null || (!fuelVendor.ToLower().Contains("fbolinx") && fbo != null && fbo.Preferences != null && fbo.Preferences.Oid > 0 && ((fbo.Preferences.OrderNotificationsEnabled.HasValue && fbo.Preferences.OrderNotificationsEnabled.Value) || (fbo.Preferences.OrderNotificationsEnabled == null))))
            //    canSendEmail = true;
            //else if (fbo == null || fuelVendor.ToLower().Contains("fbolinx") && fbo != null && fbo.Preferences != null && fbo.Preferences.Oid > 0 && ((fbo.Preferences.DirectOrderNotificationsEnabled.HasValue && fbo.Preferences.DirectOrderNotificationsEnabled.Value) || (fbo.Preferences.DirectOrderNotificationsEnabled == null)))
            //    canSendEmail = true;

            //// SEND EMAIL IF SETTING IS ON OR NOT FBOLINX CUSTOMER
            //if (canSendEmail)
            //{
            //    var authentication = await _AuthService.CreateAuthenticatedLink(fbo.AcukwikFBOHandlerId.GetValueOrDefault());

            //    if (authentication.FboEmails != "FBO not found" && authentication.FboEmails != "No email found")
            //    {
            //        var link = "https://" + _httpContextAccessor.HttpContext.Request.Host + "/outside-the-gate-layout/auth?token=" + HttpUtility.UrlEncode(authentication.AccessToken) + "&id=" + sourceId; ;

            //        var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridFuelRequestUpdateOrCancellationTemplateData
            //        {
            //            buttonUrl = link,
            //            requestStatus = requestStatus
            //        };

            //        var fboContacts = await GetFboContacts(fbo.Oid);
            //        var userContacts = await GetUserContacts(fbo);

            //        var fboEmails = authentication.FboEmails + (fboContacts == "" ? "" : ";" + fboContacts) + (userContacts == "" ? "" : ";" + userContacts);

            //        var result = await GenerateFuelOrderMailMessage(authentication.Fbo, fboEmails, null, dynamicTemplateData);

            //        return result;
            //    }
            //    return false;
            //}
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

        private async Task<bool> GenerateFuelOrderMailMessage(string fbo, string fboEmails, SendGridAutomatedFuelOrderNotificationTemplateData dynamicAutomatedFuelOrderNotificationTemplateData = null, SendGridAutomatedFuelOrderCancellationTemplateData dynamicFuelOrderCancellationTemplateDate = null)
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
                else if (dynamicFuelOrderCancellationTemplateDate != null)
                    mailMessage.SendGridFuelOrderCancellationTemplateData = dynamicFuelOrderCancellationTemplateDate;

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