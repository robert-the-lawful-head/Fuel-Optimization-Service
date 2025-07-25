﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.Dto.Requests;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.PricingTemplate;

namespace FBOLinx.ServiceLayer.BusinessServices.PricingTemplate
{
    public interface IPricingTemplateService: IPricingTemplateGridService
    {
        Task FixCustomCustomerTypes(int groupId, int fboId);
        Task FixDefaultPricingTemplate(int fboId);
        Task<List<DB.Models.PricingTemplate>> GetAllPricingTemplatesForCustomerAsync(CustomerInfoByGroupDto customer, int fboId, int groupId, int pricingTemplateId = 0, bool isAnalytics = false);
        Task UpdatePricingTemplate(int id, PricingTemplateDto pricingTemplate);
        Task<bool> PutPricingTemplate(int id, PricingTemplateDto pricingTemplate);
        Task<PricingTemplateDto> PostPricingTemplate(PricingTemplateDto pricingTemplate);
        Task CopyPricingTemplate(PrincingTemplateRequest pricingTemplate);
        Task<PricingTemplateDto> GetDefaultTemplate(int fboId);
        Task<PricingTemplateDto> GetDefaultTemplateIncludeNullCheck(int fboId);
        Task<PricingTemplateDto> DeletePricingTemplate(PricingTemplateDto pricingTemplate, int oid, int fboId);
        Task<PricingTemplateDto> GetPricingTemplateById(int oid);
        Task<List<DB.Models.PricingTemplate>> GetStandardPricingTemplatesForAllCustomers(int fboId, int groupId);
        Task<List<CustomerAircraftsPricingTemplatesModel>> GetCustomerAircraftTemplates(int fboId, int groupId);
        Task<List<CustomerPricingTemplatesModel>> GetCustomerTemplates(int customerId = 0, int fboId = 0);
        Task<List<DB.Models.PricingTemplate>> GetAllPricingTemplatesForFbo(int fboId, int groupId);
    }
}