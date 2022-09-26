using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{
    public interface ICustomerAircraftService : IBaseDTOService<CustomerAircraftDTO, CustomerAircrafts>
    {
        Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsWithDetails(int groupId, int fboId = 0, int customerId = 0);
        Task<List<CustomerAircraftsViewModel>> GetAircraftsList(int groupId, int fboId);
        Task<string> GetCustomerAircraftTailNumberByCustomerAircraftId(int customerAircraftId);
    }

    public class CustomerAircraftService : BaseDTOService<CustomerAircraftDTO, DB.Models.CustomerAircrafts, FboLinxContext>, ICustomerAircraftService
    {
        private FboLinxContext _Context;
        private AircraftService _AircraftService;
        private readonly IPricingTemplateService _pricingTemplateService;

        public CustomerAircraftService(ICustomerAircraftEntityService customerAircraftEntityService, FboLinxContext context, AircraftService aircraftService, IPricingTemplateService pricingTemplateService) : base(customerAircraftEntityService)
        {
            _AircraftService = aircraftService;
            _Context = context;
            _pricingTemplateService = pricingTemplateService;
        }

        public async Task<List<CustomerAircraftsViewModel>> GetCustomerAircraftsWithDetails(int groupId, int fboId = 0, int customerId = 0)
        {
            var pricingTemplates = await _pricingTemplateService.GetStandardPricingTemplatesForAllCustomers(fboId, groupId);

            var aircraftPricingTemplates = await _pricingTemplateService.GetCustomerAircraftTemplates(fboId, groupId);

            var allAircraft = await _AircraftService.GetAllAircrafts();

            var result = await GetCustomerAircrafts(groupId);

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
                if (x.Size == AircraftSizes.NotSet && aircraft?.Size.GetValueOrDefault() != AircraftSizes.NotSet)
                    x.Size = aircraft?.Size;
            });

            return result;
        }

        public async Task<List<CustomerAircraftsViewModel>> GetAircraftsList(int groupId, int fboId)
        {
            var allAircraft = await _AircraftService.GetAllAircrafts();

            List<CustomerAircraftsViewModel> result = await (
               from ca in _Context.CustomerAircrafts
               join cg in _Context.CustomerInfoByGroup on new { groupId, ca.CustomerId } equals new { groupId = cg.GroupId, cg.CustomerId }
               join c in _Context.Customers on cg.CustomerId equals c.Oid
               join cct in _Context.CustomCustomerTypes on new { fboId, CustomerId = c.Oid } equals new { fboId = cct.Fboid, cct.CustomerId }
               where ca.GroupId == groupId && cct.Fboid == fboId && !string.IsNullOrEmpty(ca.TailNumber)
               select new CustomerAircraftsViewModel
               {
                   TailNumber = ca.TailNumber,
               })
               .Distinct()
               .OrderBy(x => x.TailNumber)
               .ToListAsync();

            return result;
        }

        private async Task<List<CustomerAircraftsViewModel>> GetCustomerAircrafts(int groupId)
        {
            var result = await GetListbySpec(new CustomerAircraftsByGroupSpecification(groupId));

            List<CustomerAircraftsViewModel> result = await (
               from ca in _Context.CustomerAircrafts
               join cg in _Context.CustomerInfoByGroup on new { groupId, ca.CustomerId } equals new { groupId = cg.GroupId, cg.CustomerId }
               join c in _Context.Customers on cg.CustomerId equals c.Oid
               where ca.GroupId == groupId && (!c.Suspended.HasValue || !c.Suspended.Value)
               select new CustomerAircraftsViewModel
               {
                   Oid = ca.Oid,
                   GroupId = ca.GroupId,
                   CustomerId = ca.CustomerId,
                   Company = cg.Company,
                   AircraftId = ca.AircraftId,
                   TailNumber = ca.TailNumber,
                   Size = ca.Size.HasValue && ca.Size != AircraftSizes.NotSet ? ca.Size : (AircraftSizes.NotSet),
                   BasedPaglocation = ca.BasedPaglocation,
                   NetworkCode = ca.NetworkCode,
                   AddedFrom = ca.AddedFrom ?? 0,
                   IsFuelerlinxNetwork = c.FuelerlinxId > 0
               })
               .OrderBy(x => x.TailNumber)
               .ToListAsync();

            return result;
        }

        public async Task<string> GetCustomerAircraftTailNumberByCustomerAircraftId(int customerAircraftId)
        {
            var customerAircraft = await _Context.CustomerAircrafts.Where(a => a.Oid == customerAircraftId).FirstOrDefaultAsync();
            return customerAircraft.TailNumber;
        }
    }
}
