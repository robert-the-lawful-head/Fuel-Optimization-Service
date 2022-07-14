using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{
    public class CustomerAircraftService
    {
        private FboLinxContext _Context;
        private AircraftService _AircraftService;
        private readonly IPricingTemplateService _pricingTemplateService;

        public CustomerAircraftService(FboLinxContext context, AircraftService aircraftService, IPricingTemplateService pricingTemplateService)
        {
            _AircraftService = aircraftService;
            _Context = context;
            _pricingTemplateService = pricingTemplateService;
        }

        public async Task<List<CustomerAircraftsViewModel>> GetCustomerAircrafts(int groupId, int fboId = 0, int customerId = 0)
        {
            var pricingTemplates = await _pricingTemplateService.GetStandardPricingTemplatesForAllCustomers(fboId, groupId);

            var aircraftPricingTemplates = await (
                                   from pt in _Context.PricingTemplate
                                   join ap in _Context.AircraftPrices on pt.Oid equals ap.PriceTemplateId
                                   into leftJoinAp
                                   from ap in leftJoinAp.DefaultIfEmpty()
                                   where pt.Fboid == fboId && fboId > 0
                                   select new
                                   {
                                       Oid = pt == null ? 0 : pt.Oid,
                                       Name = pt == null ? "" : pt.Name,
                                       CustomerAircraftId = ap == null ? 0 : ap.CustomerAircraftId
                                   }).ToListAsync();

            var allAircraft = await _AircraftService.GetAllAircrafts();

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

            result.ForEach(x =>
            {
                var aircraftPricingTemplate = aircraftPricingTemplates.FirstOrDefault(pt => pt.CustomerAircraftId == x.AircraftId);
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

        public async Task<string> GetCustomerAircraftTailNumberByCustomerAircraftId(int customerAircraftId)
        {
            var customerAircraft = await _Context.CustomerAircrafts.Where(a => a.Oid == customerAircraftId).FirstOrDefaultAsync();
            return customerAircraft.TailNumber;
        }
    }
}
