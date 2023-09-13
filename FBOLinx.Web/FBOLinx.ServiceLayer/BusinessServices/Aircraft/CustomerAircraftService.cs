using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.Extensions.Caching.Memory;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{
    public interface ICustomerAircraftService : IBaseDTOService<CustomerAircraftsDto, CustomerAircrafts>
    {
        Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsWithDetails(int groupId, int fboId = 0,
            int customerId = 0, List<string> tailNumbers = null, bool useCache = false);
        Task<List<CustomerAircraftsViewModel>> GetAircraftsList(int groupId, int fboId);
        Task<string> GetCustomerAircraftTailNumberByCustomerAircraftId(int customerAircraftId);
        void ClearCache(int groupId, int fboId = 0);
        Task<List<CustomerAircraftsDto>> GetCustomerAircrafts(int groupId);
    }

    public class CustomerAircraftService : BaseDTOService<CustomerAircraftsDto, DB.Models.CustomerAircrafts, FboLinxContext>, ICustomerAircraftService
    {
        private IAircraftService _AircraftService;
        private readonly IPricingTemplateService _pricingTemplateService;
        private IMemoryCache _MemoryCache;
        private readonly ICustomerInfoByGroupService _CustomerInfoByGroupService;
        private const string _AircraftWithDetailsCacheKey = "CustomerAircraft_CustomAircraftsWithDetails_";

        public CustomerAircraftService(ICustomerAircraftEntityService customerAircraftEntityService, IAircraftService aircraftService, 
            IPricingTemplateService pricingTemplateService,
            IMemoryCache memoryCache, ICustomerInfoByGroupService customerInfoByGroupService) : base(customerAircraftEntityService)
        {
            _MemoryCache = memoryCache;
            _CustomerInfoByGroupService = customerInfoByGroupService;
            _AircraftService = aircraftService;
            _pricingTemplateService = pricingTemplateService;
        }

        public async Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsWithDetails(int groupId, int fboId = 0, int customerId = 0, List<string> tailNumbers = null, bool useCache = false)
        {
            if (useCache)
                return await GetCustomAircraftWithDetailsFromCache(groupId, fboId, customerId, tailNumbers);
            return await GetCustomerAircraftWithDetailsFromDatabase(groupId, fboId, customerId, tailNumbers);
        }

        public async Task<List<CustomerAircraftsViewModel>> GetAircraftsList(int groupId, int fboId)
        {
            var result = await GetCustomerAircraftsViewModel(groupId);
            
            return result.OrderBy(x => x.TailNumber).ToList();
        }

        public async Task<string> GetCustomerAircraftTailNumberByCustomerAircraftId(int customerAircraftId)
        {
            var customerAircraft = await GetSingleBySpec(new CustomerAircraftSpecification(customerAircraftId));
            return customerAircraft?.TailNumber;
        }

        public void ClearCache(int groupId, int fboId = 0)
        {
            _MemoryCache.Remove(_AircraftWithDetailsCacheKey + groupId + "_" + fboId);
        }

        private async Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsViewModel(int groupId, int customerId = 0, List<string> tailNumbers = null)
        {
            List<CustomerAircraftsDto> aircrafts = null;           

            if (customerId > 0)
            {
                aircrafts =
                    await this.GetListbySpec(
                        new CustomerAircraftByGroupSpecification(new List<int>() { groupId }, customerId));

                aircrafts = aircrafts.Where(x =>
                    tailNumbers == null || tailNumbers.Count == 0 || tailNumbers.Contains(x.TailNumber)).ToList();
            }
            else
            {
                var customers = await _CustomerInfoByGroupService.GetCustomers(groupId, tailNumbers);
                aircrafts = customers.SelectMany(a => a.Customer.CustomerAircrafts).ToList();
            }

            return aircrafts?.Select(x => CustomerAircraftsViewModel.Cast(x)).ToList();
        }
        public async Task<List<CustomerAircraftsDto>> GetCustomerAircrafts(int groupId)
        {

            var customers = await _CustomerInfoByGroupService.GetCustomers(groupId);
            var aircrafts = customers.SelectMany(a => a.Customer.CustomerAircrafts).ToList();
            return aircrafts;
        }

        private async Task<List<CustomerAircraftsViewModel>> GetCustomAircraftWithDetailsFromCache(int groupId,
            int fboId = 0, int customerId = 0, List<string> tailNumbers = null)
        {
            var result = await _MemoryCache.GetOrCreateAsync(_AircraftWithDetailsCacheKey + groupId + "_" + fboId, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetCustomerAircraftWithDetailsFromDatabase(groupId, fboId);
            });

            return result?.Where(x => (tailNumbers == null || tailNumbers.Contains(x.TailNumber))
            && (customerId == 0 || x.CustomerId == customerId)).ToList();
        }
        private async Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftWithDetailsFromDatabase(int groupId, int fboId = 0, int customerId = 0, List<string> tailNumbers = null)
        {
            var pricingTemplates = await _pricingTemplateService.GetStandardPricingTemplatesForAllCustomers(fboId, groupId);
            var aircraftPricingTemplates = await _pricingTemplateService.GetCustomerAircraftTemplates(fboId, groupId);
            var allAircraft = await _AircraftService.GetAllAircrafts();

            var result = await GetCustomerAircraftsViewModel(groupId, customerId, tailNumbers);

            result.ForEach(x =>
            {
                var aircraftPricingTemplate = aircraftPricingTemplates.FirstOrDefault(pt => pt.CustomerAircraftId == x.Oid);
                if (aircraftPricingTemplate != null)
                {
                    x.PricingTemplateId = aircraftPricingTemplate?.Oid;
                    x.PricingTemplateName = aircraftPricingTemplate?.Name;
                }
                else if (customerId == 0)
                {
                    var pricingTemplate = pricingTemplates.FirstOrDefault(pt => pt.CustomerId == x.CustomerId);
                    x.PricingTemplateId = pricingTemplate?.Oid;
                    x.PricingTemplateName = pricingTemplate?.Name;
                    x.IsCompanyPricing = true;
                }

                var aircraft = allAircraft.FirstOrDefault(a => a.AircraftId == x.AircraftId);
                x.Make = aircraft?.Make;
                x.Model = aircraft?.Model;
                x.FuelCapacityGal = aircraft?.FuelCapacityGal;
                x.ICAOAircraftCode = aircraft?.AFSAircraft?.Where(x => !string.IsNullOrEmpty(x.Icao))?.FirstOrDefault()
                    ?.Icao;
                if (x.Size == AircraftSizes.NotSet && aircraft?.Size.GetValueOrDefault() != AircraftSizes.NotSet)
                    x.Size = aircraft?.Size;
            });

            return result;
        }
    }
}
