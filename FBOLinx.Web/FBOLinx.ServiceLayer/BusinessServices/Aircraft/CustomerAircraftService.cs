using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{
    public interface ICustomerAircraftService : IBaseDTOService<CustomerAircraftsDto, CustomerAircrafts>
    {
        Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsWithDetails(int groupId, int fboId = 0, int customerId = 0);
        Task<List<CustomerAircraftsViewModel>> GetAircraftsList(int groupId, int fboId);
        Task<string> GetCustomerAircraftTailNumberByCustomerAircraftId(int customerAircraftId);
    }

    public class CustomerAircraftService : BaseDTOService<CustomerAircraftsDto, DB.Models.CustomerAircrafts, FboLinxContext>, ICustomerAircraftService
    {
        private AircraftService _AircraftService;
        private readonly IPricingTemplateService _pricingTemplateService;

        public CustomerAircraftService(ICustomerAircraftEntityService customerAircraftEntityService, AircraftService aircraftService, IPricingTemplateService pricingTemplateService) : base(customerAircraftEntityService)
        {
            _AircraftService = aircraftService;
            _pricingTemplateService = pricingTemplateService;
        }

        public async Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsWithDetails(int groupId, int fboId = 0, int customerId = 0)
        {
            var pricingTemplates = await _pricingTemplateService.GetStandardPricingTemplatesForAllCustomers(fboId, groupId);

            var aircraftPricingTemplates = await _pricingTemplateService.GetCustomerAircraftTemplates(fboId, groupId);

            var allAircraft = await _AircraftService.GetAllAircrafts();

            var result = await GetCustomerAircraftsViewModel(groupId);

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

        public async Task<List<CustomerAircraftsViewModel>> GetAircraftsList(int groupId, int fboId)
        {
            var result = await GetCustomerAircraftsViewModel(groupId);
            
            return result.OrderBy(x => x.TailNumber).ToList();
        }

        private async Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsViewModel(int groupId)
        {
            var result = await GetListbySpec(new CustomerAircraftsByGroupSpecification(groupId));
            return result?.Select(x => CustomerAircraftsViewModel.Cast(x)).ToList();
        }

        public async Task<string> GetCustomerAircraftTailNumberByCustomerAircraftId(int customerAircraftId)
        {
            var customerAircraft = await GetSingleBySpec(new CustomerAircraftSpecification(customerAircraftId));
            return customerAircraft.TailNumber;
        }
    }
}
