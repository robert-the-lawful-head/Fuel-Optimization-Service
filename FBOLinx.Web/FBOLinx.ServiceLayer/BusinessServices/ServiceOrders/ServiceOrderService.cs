using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.ServiceOrders
{
    public interface IServiceOrderService : IBaseDTOService<ServiceOrderDto, DB.Models.ServiceOrder>
    {
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
            //if (serviceOrder.CustomerInfoByGroup != null && serviceOrder.CustomerInfoByGroupId == 0)
            //{
            //    var customerInfoByGroup = await _CustomerInfoByGroupService.AddNewCustomerInfoByGroup(serviceOrder.CustomerInfoByGroup);
            //    serviceOrder.CustomerInfoByGroupId = customerInfoByGroup.Oid;
            //}

            if (serviceOrder.CustomerAircraft != null)
            {

            }
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
            await result.PopulateLocalTimes(_AirportTimeService);
            return result;
        }

        
    }
}
