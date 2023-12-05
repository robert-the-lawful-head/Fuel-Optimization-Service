using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.ServiceOrders
{
    public interface IServiceOrderService : IBaseDTOService<ServiceOrderDto, DB.Models.ServiceOrder>
    {
        Task<ServiceOrderDto> AddNewOrder(ServiceOrderDto serviceOrder);
    }

    public class ServiceOrderService : BaseDTOService<ServiceOrderDto, DB.Models.ServiceOrder, FboLinxContext>, IServiceOrderService
    {
        private IAirportTimeService _AirportTimeService;
        private ICustomerAircraftService _CustomerAircraftService;
        private ICustomerInfoByGroupService _CustomerInfoByGroupService;

        public ServiceOrderService(IRepository<ServiceOrder, FboLinxContext> entityService,
            IAirportTimeService airportTimeService,
            ICustomerAircraftService customerAircraftService,
            ICustomerInfoByGroupService customerInfoByGroupService) : base(entityService)
        {
            _CustomerInfoByGroupService = customerInfoByGroupService;
            _CustomerAircraftService = customerAircraftService;
            _AirportTimeService = airportTimeService;
        }

        public async Task<ServiceOrderDto> AddNewOrder(ServiceOrderDto serviceOrder)
        {
            if (serviceOrder.CustomerInfoByGroup != null && serviceOrder.CustomerInfoByGroupId == 0)
            {
                serviceOrder.CustomerInfoByGroup = await _CustomerInfoByGroupService.AddNewCustomerInfoByGroup(serviceOrder.CustomerInfoByGroup);
                serviceOrder.CustomerInfoByGroupId = serviceOrder.CustomerInfoByGroup.Oid;
            }

            if (serviceOrder.CustomerAircraft != null && serviceOrder.CustomerAircraftId == 0)
            {
                serviceOrder.CustomerAircraft.CustomerId = serviceOrder.CustomerInfoByGroup.CustomerId;
                var customerAircraft = await _CustomerAircraftService.AddAsync(serviceOrder.CustomerAircraft);
                serviceOrder.CustomerAircraftId = customerAircraft.Oid;
            }

            if (serviceOrder.CustomerInfoByGroupId == 0)
            {
                var customerAircraft = await _CustomerAircraftService.GetSingleBySpec(new CustomerAircraftSpecification(serviceOrder.CustomerAircraftId));

                var customerInfoByGroup = await _CustomerInfoByGroupService.GetSingleBySpec(new CustomerInfoByGroupCustomerIdGroupIdSpecification(customerAircraft.CustomerId, serviceOrder.GroupId));
                serviceOrder.CustomerInfoByGroupId = customerInfoByGroup.Oid;

                serviceOrder.ArrivalDateTimeUtc = await _AirportTimeService.GetZuluDateTimeFromAirportLocalDateTime(serviceOrder.Fboid, serviceOrder.ArrivalDateTimeUtc);
                serviceOrder.DepartureDateTimeUtc = await _AirportTimeService.GetZuluDateTimeFromAirportLocalDateTime(serviceOrder.Fboid, serviceOrder.DepartureDateTimeUtc);
                serviceOrder.ServiceDateTimeUtc = await _AirportTimeService.GetZuluDateTimeFromAirportLocalDateTime(serviceOrder.Fboid, serviceOrder.ServiceDateTimeUtc);
            }

            //if (serviceOrder.ArrivalDateTimeUtc == null)
            //{
            //    // Find fuel order to fill in information for the service order
            //    var fuelReqs = await _fuelReqService.GetDirectAndContractOrdersByGroupAndFbo(serviceOrder.GroupId, serviceOrder.Fboid, DateTime.UtcNow, DateTime.UtcNow.AddMonths(3));
            //    var fuelReq = fuelReqs.Where(f => f.SourceId == serviceOrder.FuelerLinxTransactionId).FirstOrDefault();

            //    if (fuelReq != null)
            //    {
            //        serviceOrder.ArrivalDateTimeUtc = await _AirportTimeService.GetZuluDateTimeFromAirportLocalDateTime(serviceOrder.Fboid, fuelReq.Eta);
            //        serviceOrder.DepartureDateTimeUtc = await _AirportTimeService.GetZuluDateTimeFromAirportLocalDateTime(serviceOrder.Fboid, fuelReq.Etd);
            //        serviceOrder.CustomerAircraftId = fuelReq.CustomerAircraftId.GetValueOrDefault();
            //        serviceOrder.ServiceOn = fuelReq.FuelOn == "Departure" ? Core.Enums.ServiceOrderAppliedDateTypes.Departure : Core.Enums.ServiceOrderAppliedDateTypes.Arrival;
            //        serviceOrder.ServiceDateTimeUtc = fuelReq.FuelOn == "Departure" ? serviceOrder.DepartureDateTimeUtc.GetValueOrDefault() : serviceOrder.ArrivalDateTimeUtc.GetValueOrDefault();

            //        var customerInfoByGroup = await _CustomerInfoByGroupService.GetSingleBySpec(new CustomerInfoByGroupCustomerIdGroupIdSpecification(fuelReq.CustomerId.GetValueOrDefault(), serviceOrder.GroupId));
            //        serviceOrder.CustomerInfoByGroupId = customerInfoByGroup.Oid;
            //    }
            //}

            serviceOrder.CustomerAircraft = null;
            serviceOrder.CustomerInfoByGroup = null;

            return await AddAsync(serviceOrder);
        }

        public override async Task<List<ServiceOrderDto>> GetListbySpec(ISpecification<ServiceOrder> spec)
        {
            var result = await base.GetListbySpec(spec);
            foreach (var serviceOrder in result)
            {
                await serviceOrder.PopulateLocalTimes(_AirportTimeService);
            }

            return result;
        }

        public override async Task<ServiceOrderDto> GetSingleBySpec(ISpecification<ServiceOrder> spec)
        {
            var result = await base.GetSingleBySpec(spec);
            if (result != null)
                await result.PopulateLocalTimes(_AirportTimeService);
            return result;
        }
    }
}
