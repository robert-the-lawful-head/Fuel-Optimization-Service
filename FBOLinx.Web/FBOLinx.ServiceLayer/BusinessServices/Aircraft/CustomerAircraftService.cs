using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{
    public interface ICustomerAircraftService : IBaseDTOService<CustomerAircraftsDto, CustomerAircrafts>
    {
        Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsWithDetails(int groupId, int fboId = 0,
            int customerId = 0, List<string> tailNumbers = null, bool useCache = false);
        Task<List<CustomerAircraftsViewModel>> GetAircraftsList(int groupId, int fboId);
        Task<string> GetCustomerAircraftTailNumberByCustomerAircraftId(int customerAircraftId);
        void ClearCache(int groupId, int fboId = 0);
    }

    public class CustomerAircraftService : BaseDTOService<CustomerAircraftsDto, DB.Models.CustomerAircrafts, FboLinxContext>, ICustomerAircraftService
    {
        private AircraftService _AircraftService;
        private readonly IPricingTemplateService _pricingTemplateService;
        private IMemoryCache _MemoryCache;
        private readonly ICustomerService _CustomerService;
        private const string _AircraftWithDetailsCacheKey = "CustomerAircraft_CustomAircraftsWithDetails_";

        public CustomerAircraftService(ICustomerAircraftEntityService customerAircraftEntityService, AircraftService aircraftService, 
            IPricingTemplateService pricingTemplateService,
            IMemoryCache memoryCache, ICustomerService customerService) : base(customerAircraftEntityService)
        {
            _MemoryCache = memoryCache;
            _CustomerService = customerService;
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

        private async Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsViewModel(int groupId, List<string> tailNumbers = null)
        {
            var customers = await _CustomerService.GetCustomers(groupId, tailNumbers);
            var aircrafts = customers.SelectMany(a => a.CustomerAircrafts).Where(c => c.GroupId == groupId && c.CustomerId > 0 && c.Customer.CustomerInfoByGroup != null).ToList();
            return aircrafts?.Select(x => CustomerAircraftsViewModel.Cast(x)).ToList();
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

            var result = await GetCustomerAircraftsViewModel(groupId, tailNumbers);

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
