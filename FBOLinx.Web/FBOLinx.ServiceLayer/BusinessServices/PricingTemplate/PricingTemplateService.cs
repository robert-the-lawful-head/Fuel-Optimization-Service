
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.Dto.Requests;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.PricingTemplate;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.PricingTemplate
{
    public class PricingTemplateService : IPricingTemplateService
    {        
        private IPricingTemplateEntityService _pricingTemplateEntityService;
        private ICustomerTypesEntityService _customerTypesEntityService;
        private ICustomerMarginsEntityService _customerMarginsEntityService;
        private ICustomerAircraftEntityService _customerAircraftEntityService;
        private ICustomCustomerTypeService _customCustomerTypeService;
        private ICustomerMarginService _customerMarginService;
        private IFbolinxPricingTemplateAttachmentsEntityService _fbolinxPricingTemplateAttachmentsEntityService;
        private FboLinxContext _context;
        public PricingTemplateService(
            IPricingTemplateEntityService pricingTemplateEntityService,
            ICustomerTypesEntityService customerTypesEntityService,
            ICustomerMarginsEntityService customerMarginsEntityService,
            ICustomerAircraftEntityService customerAircraftEntityService,
            ICustomCustomerTypeService customCustomerTypeService,
            ICustomerMarginService customerMarginService,
            IFbolinxPricingTemplateAttachmentsEntityService fbolinxPricingTemplateAttachmentsEntityService,
            FboLinxContext context)
        {
            _pricingTemplateEntityService = pricingTemplateEntityService;
            _customerTypesEntityService = customerTypesEntityService;
            _customerMarginsEntityService = customerMarginsEntityService;
            _customerAircraftEntityService = customerAircraftEntityService;
            _customCustomerTypeService = customCustomerTypeService;
            _customerMarginService = customerMarginService;
            _fbolinxPricingTemplateAttachmentsEntityService = fbolinxPricingTemplateAttachmentsEntityService;
            _context = context;
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

        public async Task<List<DB.Models.PricingTemplate>> GetAllPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0,bool isAnalytics = false)
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
            if (!isAnalytics || (isAnalytics && aircraftPricesResult.Count > 0))
            {
                var customerAircrafts = await _customerAircraftEntityService.Where(x => x.CustomerId == customer.CustomerId && x.GroupId == groupId).ToListAsync();

                standardTemplates.ForEach(x => x.TailNumbers = customerAircrafts.Where(c => !string.IsNullOrEmpty(c.TailNumber) && !aircraftPricesResult.Any(a => a.TailNumbers != null && a.TailNumbers.Contains(c.TailNumber))).Select(c => c.TailNumber.Trim()).ToList());
            }
            else
                standardTemplates.ForEach(x => x.TailNumbers = new List<string>() { "All Tails" });

            return result;
        }

        public async Task<List<DB.Models.PricingTemplate>> GetStandardPricingTemplatesForAllCustomers(int fboId, int groupId)
        {
            List<DB.Models.PricingTemplate> result = new List<DB.Models.PricingTemplate>();

            var standardTemplates = await _pricingTemplateEntityService.GetStandardTemplatesForAllCustomers(fboId, groupId);
            result.AddRange(standardTemplates);
            return result;
        }

        public async Task<PricingTemplateDto> GetPricingTemplateById(int oid)
        {
            var result = await _pricingTemplateEntityService.FindAsync(oid);
            return result.Map<PricingTemplateDto>();
        }
        public async Task<bool> PutPricingTemplate(int id, PricingTemplateDto pricingTemplate)
        { 
            var pricingTemplateEntity = pricingTemplate.Map<DB.Models.PricingTemplate>();
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
                var otherDefaults = await _pricingTemplateEntityService.Where(x =>
                    x.Fboid ==  pricingTemplate.Fboid && (x.Default ?? false) && x.Oid != pricingTemplate.Oid).ToListAsync();
                foreach (var otherDefault in otherDefaults)
                {
                    otherDefault.Default = false;
                    await _pricingTemplateEntityService.UpdateAsync(otherDefault);
                }
            }
        }
        public async Task UpdatePricingTemplate(int id, PricingTemplateDto pricingTemplate)
        {
            var pricingTemplateEntity = pricingTemplate.Map<DB.Models.PricingTemplate>();
            await FixOtherDefaults(pricingTemplateEntity);

            await _pricingTemplateEntityService.AddAsync(pricingTemplateEntity);
        }

        public async Task<PricingTemplateDto> PostPricingTemplate(PricingTemplateDto pricingTemplate)
        {
            var pricingTemplateEntity = pricingTemplate.Map<DB.Models.PricingTemplate>();

            await FixOtherDefaults(pricingTemplateEntity);

            pricingTemplateEntity = await _pricingTemplateEntityService.AddAsync(pricingTemplateEntity);

            return pricingTemplateEntity.Map<PricingTemplateDto>(); 
        }
        public async Task<PricingTemplateDto> GetDefaultTemplate(int fboId)
        {
            var pricingTemplate =  await _pricingTemplateEntityService.FirstOrDefaultAsync(s => s.Fboid == fboId && s.Default == true);
            return pricingTemplate.Map<PricingTemplateDto>();
        }
        public async Task<PricingTemplateDto> GetDefaultTemplateIncludeNullCheck(int fboId)
        {
            var pricingTemplate = await _pricingTemplateEntityService.FirstOrDefaultAsync(p => p.Fboid.Equals(fboId) && (p.Default ?? false));

            return pricingTemplate.Map<PricingTemplateDto>();
        }

        public async Task<PricingTemplateDto> DeletePricingTemplate(PricingTemplateDto pricingTemplate,int oid, int fboId)
        {

            await _pricingTemplateEntityService.DeleteAsync(pricingTemplate.Oid);


            var defaultPricingTemplate = await GetDefaultTemplateIncludeNullCheck(fboId);

            if (defaultPricingTemplate == null) return pricingTemplate;

            await _customCustomerTypeService.SaveCustomersTypes(fboId, oid, defaultPricingTemplate.Oid);

            return pricingTemplate;

        }

        public async Task CopyPricingTemplate(PrincingTemplateRequest pricingTemplate)
        {
            var pricingTemplateCopy = await _pricingTemplateEntityService.CopyPricingTemplate(pricingTemplate.currentPricingTemplateId, pricingTemplate.name);

            if (pricingTemplateCopy.Oid == 0)
                return;

            await _customerMarginService.CreateCustomerMargins(pricingTemplate.currentPricingTemplateId, pricingTemplateCopy.Oid);
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

        public async Task<string> GetFileAttachment(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fbolinxPricingTemplateAttachmentsEntityService.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();

            if (pricingTemplateFile != null)
            {
                var fileBase64 = Convert.ToBase64String(pricingTemplateFile.FileData, 0, pricingTemplateFile.FileData.Length);
                return "data:" + pricingTemplateFile.ContentType + ";base64," + fileBase64;
            }

            return "";
        }

        public async Task<FbolinxPricingTemplateFileAttachmentDto> GetFileAttachmentObject(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fbolinxPricingTemplateAttachmentsEntityService.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();
            if(pricingTemplateFile == null) return null;
            return pricingTemplateFile.Map<FbolinxPricingTemplateFileAttachmentDto>();
        }

        public async Task<string> GetFileAttachmentName(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fbolinxPricingTemplateAttachmentsEntityService.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();

            if (pricingTemplateFile == null || pricingTemplateFile.Oid == 0)
                return "";
            
            return pricingTemplateFile.FileName;
        }

        public async Task<bool> UploadFileAttachment(FbolinxPricingTemplateAttachmentsRequest request)
        {
            var fileAsArray = Convert.FromBase64String(request.FileData);

            var existingRecord = await _fbolinxPricingTemplateAttachmentsEntityService.Where(f => f.PricingTemplateId == request.PricingTemplateId).FirstOrDefaultAsync();

            if (existingRecord != null && existingRecord.Oid > 0)
            {
                existingRecord.FileData = fileAsArray;
                existingRecord.FileName = request.FileName;
                existingRecord.ContentType = request.ContentType;
                await _fbolinxPricingTemplateAttachmentsEntityService.UpdateAsync(existingRecord);
            }
            else
            {
                FbolinxPricingTemplateFileAttachment fboLinxFileData = new FbolinxPricingTemplateFileAttachment();
                fboLinxFileData.FileData = fileAsArray;
                fboLinxFileData.FileName = request.FileName;
                fboLinxFileData.ContentType = request.ContentType;
                fboLinxFileData.PricingTemplateId = request.PricingTemplateId;
                await _fbolinxPricingTemplateAttachmentsEntityService.AddAsync(fboLinxFileData);
            }
            return true;
        }

        public async Task<bool> DeleteFileAttachment(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fbolinxPricingTemplateAttachmentsEntityService.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();
            var result = _fbolinxPricingTemplateAttachmentsEntityService.DeleteAsync(pricingTemplateFile.Oid);

            if (result == null) return false;

            return true;
        }

        public async Task<List<CustomerAircraftsPricingTemplatesModel>> GetCustomerAircraftTemplates(int fboId, int groupId)
        {
            var aircraftPricingTemplates = await (
                                    from ap in _context.AircraftPrices
                                    join ca in _context.CustomerAircrafts on ap.CustomerAircraftId equals ca.Oid
                                    join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                    into leftJoinPt
                                    from pt in leftJoinPt.DefaultIfEmpty()
                                    where ca.GroupId == groupId && pt.Fboid == fboId && fboId > 0
                                    select new CustomerAircraftsPricingTemplatesModel
                                    {
                                        Oid = ap == null ? 0 : pt.Oid,
                                        Name = ap == null ? "" : pt.Name,
                                        CustomerAircraftId = ap == null ? 0 : ap.CustomerAircraftId
                                    }).ToListAsync();
            return aircraftPricingTemplates;
        }
    }
}