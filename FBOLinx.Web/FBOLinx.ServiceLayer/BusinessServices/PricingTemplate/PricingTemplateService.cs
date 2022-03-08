
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.Dto.Requests;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.ServiceLayer.EntityServices;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.PricingTemplate
{
    public class PricingTemplateService : IPricingTemplateService, IPricingTemplateGridService
    {
        private CustomerInfoByGroupEntityService _customerInfoByGroupEntityService;
        
        private IPricingTemplateEntityService _pricingTemplateEntityService;
        private ICustomerTypesEntityService _customerTypesEntityService;
        private ICustomerMarginsEntityService _customerMarginsEntityService;
        private ICustomerAircraftEntityService _customerAircraftEntityService;

        public PricingTemplateService(
            CustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            IPricingTemplateEntityService pricingTemplateEntityService,
            ICustomerTypesEntityService customerTypesEntityService,
            ICustomerMarginsEntityService customerMarginsEntityService,
            ICustomerAircraftEntityService customerAircraftEntityService)
        {
            _customerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _pricingTemplateEntityService = pricingTemplateEntityService;
            _customerTypesEntityService = customerTypesEntityService;
            _customerMarginsEntityService = customerMarginsEntityService;
            _customerAircraftEntityService = customerAircraftEntityService;
        }
        public async Task FixCustomCustomerTypes(int groupId, int fboId)
        {
            if (groupId == 0 || fboId == 0) return;

            var defaultPricingTemplate = await _pricingTemplateEntityService.FirstOrDefaultAsync(x => x.Fboid == fboId && x.Default == true);

            if (defaultPricingTemplate == null) return;

            await _customerTypesEntityService.FixAndSaveCustomCustomerTypes(fboId, groupId, defaultPricingTemplate.Oid);

        }

        public async Task FixDefaultPricingTemplate(int fboId)
        {
            var existingPricingTemplates =
                   await _pricingTemplateEntityService.Where(x => x.Fboid == fboId).ToListAsync();
            if (existingPricingTemplates != null && existingPricingTemplates.Count != 0)
                return;

            //Add a default pricing template - project #1c5383
            var newTemplate = new DB.Models.PricingTemplate()
            {
                Default = true,
                Fboid = fboId,
                Name = "Posted Retail",
                MarginType = MarginTypes.RetailMinus,
                Notes = ""
            };

            await _pricingTemplateEntityService.AddAsync(newTemplate);

            await _customerMarginsEntityService.AddDefaultCustomerMargins(newTemplate.Oid, 1, 500);
            await _customerMarginsEntityService.AddDefaultCustomerMargins(newTemplate.Oid, 501, 750);
            await _customerMarginsEntityService.AddDefaultCustomerMargins(newTemplate.Oid, 751, 1000);
            await _customerMarginsEntityService.AddDefaultCustomerMargins(newTemplate.Oid, 1001, 99999);
        }

        public async Task<List<DB.Models.PricingTemplate>> GetAllPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0)
        {
            List<DB.Models.PricingTemplate> result = new List<DB.Models.PricingTemplate>();

            var standardTemplates = await _pricingTemplateEntityService.GetStandardPricingTemplatesForCustomerAsync(customer, fboId, groupId, pricingTemplateId);

            var aircraftPricesResult = await _pricingTemplateEntityService.GetTailSpecificPricingTemplatesForCustomerAsync(customer, fboId, groupId, pricingTemplateId);

            result.AddRange(standardTemplates);

            //Set the applicable tail numbers for the aircraft-specific templates
            foreach (DB.Models.PricingTemplate aircraftPricingTemplate in aircraftPricesResult)
            {
                if (standardTemplates.Any(x => x.Oid == aircraftPricingTemplate.Oid))
                    continue;
                var tailNumberList = await _customerAircraftEntityService.GetTailNumbers(aircraftPricingTemplate.Oid, customer.CustomerId, groupId);
 
                if (tailNumberList == null || tailNumberList.Count == 0)
                    continue;
                aircraftPricingTemplate.Name += " - " + string.Join(",", tailNumberList);
                aircraftPricingTemplate.TailNumbers = tailNumberList;
                result.Add(aircraftPricingTemplate);
            }

            //Set the applicable tail numbers for the standard/default templates
            var customerAircrafts = await _customerAircraftEntityService.Where(x => x.CustomerId == customer.CustomerId && x.GroupId == groupId).ToListAsync();

            standardTemplates.ForEach(x => x.TailNumbers = customerAircrafts.Where(c => !string.IsNullOrEmpty(c.TailNumber) && !aircraftPricesResult.Any(a => a.TailNumbers != null && a.TailNumbers.Contains(c.TailNumber))).Select(c => c.TailNumber.Trim()).ToList());

            return result;
        }

        public async Task<PricingTemplateDto> GetPricingTemplateById(int oid)
        {
            var result = await _pricingTemplateEntityService.FindAsync(oid);
            return result.Adapt<PricingTemplateDto>();
        }
        public async Task<bool> PutPricingTemplate(int id, PricingTemplateDto pricingTemplate)
        { 
            var pricingTemplateEntity = pricingTemplate.Adapt<DB.Models.PricingTemplate>();
            await FixOtherDefaults(pricingTemplateEntity);
            try
            {
                await _pricingTemplateEntityService.UpdateAsync(pricingTemplateEntity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricingTemplateExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }
        private bool PricingTemplateExists(int id)
        {
            return _pricingTemplateEntityService.Where(e => e.Oid == id).Any();
        }

        private async Task FixOtherDefaults(DB.Models.PricingTemplate pricingTemplate)
        {
            // to do: implement unit of work pattern with repository
            if (pricingTemplate.Default.GetValueOrDefault())
            {
                var otherDefaults = _pricingTemplateEntityService.Where(x =>
                    x.Fboid == pricingTemplate.Fboid && (x.Default ?? false) && x.Oid != pricingTemplate.Oid);
                foreach (var otherDefault in otherDefaults)
                {
                    otherDefault.Default = false;
                    await _pricingTemplateEntityService.UpdateAsync(otherDefault);
                }
            }
        }
        public async Task UpdatePricingTemplate(int id, PricingTemplateDto pricingTemplate)
        {
            var pricingTemplateEntity = pricingTemplate.Adapt<DB.Models.PricingTemplate>();
            await FixOtherDefaults(pricingTemplateEntity);

            await _pricingTemplateEntityService.AddAsync(pricingTemplateEntity);
        }

        public async Task<PricingTemplateDto> PostPricingTemplate(PricingTemplateDto pricingTemplate)
        {
            var pricingTemplateEntity = pricingTemplate.Adapt<DB.Models.PricingTemplate>();

            await FixOtherDefaults(pricingTemplateEntity);

            await _pricingTemplateEntityService.AddAsync(pricingTemplateEntity);

            return pricingTemplate.Adapt<PricingTemplateDto>(); 
        }
        public async Task<PricingTemplateDto> GetDefaultTemplate(int fboId)
        {
            var pricingTemplate =  await _pricingTemplateEntityService.FirstOrDefaultAsync(s => s.Fboid == fboId && s.Default == true);
            return pricingTemplate.Adapt<PricingTemplateDto>();
        }
        public async Task<PricingTemplateDto> GetDefaultTemplateIncludeNullCheck(int fboId)
        {
            var pricingTemplate = await _pricingTemplateEntityService.FirstOrDefaultAsync(p => p.Fboid.Equals(fboId) && (p.Default ?? false));

            return pricingTemplate.Adapt<PricingTemplateDto>();
        }

        public async Task DeletePricingTemplate(PricingTemplateDto pricingTemplate)
        {
            await _pricingTemplateEntityService.DeleteAsync(pricingTemplate.Oid);
        }

        public async Task<PricingTemplateDto> CopyPricingTemplate(PrincingTemplateRequest pricingTemplate)
        {
            var pricingTemplateCopy = await _pricingTemplateEntityService.CopyPricingTemplate(pricingTemplate.currentPricingTemplateId, pricingTemplate.name);

            return pricingTemplateCopy.Adapt<PricingTemplateDto>();
        }
        public async Task<List<PricingTemplateGrid>> GetTemplatesWithEmailContent(int fboId, int groupId)
        {
            return await _pricingTemplateEntityService.GetPricingTemplatesWithEmailContent(fboId, groupId);
        }
        public async Task<List<PricingTemplateGrid>> GetCostPlusPricingTemplates(int fboId)
        {
            return await _pricingTemplateEntityService.GetDefualtPricingTemplateGridByFboId(fboId);
        }

        public async Task<List<PricingTemplateGrid>> GetDefaultPricingTemplateByFboId(int fboId)
        {
            return await _pricingTemplateEntityService.GetDefualtPricingTemplateGridByFboId(fboId);
        }
        public async Task<List<PricingTemplateGrid>> GetPricingTemplates(int fboId, int groupId)
        {
            return await _pricingTemplateEntityService.GetPricingTemplateGrid(fboId, groupId);
        }
    }
}