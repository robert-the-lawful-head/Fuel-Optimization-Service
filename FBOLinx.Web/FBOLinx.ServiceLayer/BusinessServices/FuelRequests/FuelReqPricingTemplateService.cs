using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Microsoft.Extensions.Caching.Memory;
using Azure.Core;
using FBOLinx.ServiceLayer.DTO.Responses.Analitics;
using Microsoft.Extensions.Logging;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using System.Web;
using Microsoft.AspNetCore.Http;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using FBOLinx.ServiceLayer.DTO.Requests.FuelReq;
using FBOLinx.ServiceLayer.BusinessServices.DateAndTime;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.BusinessServices.ServiceOrders;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.DTO.Requests.ServiceOrder;
using FBOLinx.DB.Specifications.ServiceOrder;
using FBOLinx.ServiceLayer.BusinessServices.Orders;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelRequests
{
    public interface IFuelReqPricingTemplateService : IBaseDTOService<FuelReqPricingTemplateDto, FuelReqPricingTemplate>
    {
        
    }

    public class FuelReqPricingTemplateService : BaseDTOService<FuelReqPricingTemplateDto, DB.Models.FuelReqPricingTemplate, FboLinxContext>, IFuelReqPricingTemplateService
    {
        private readonly FboLinxContext _context;
        private readonly FuelReqPricingTemplateEntityService _fuelReqPricingTemplateEntityService;

        public FuelReqPricingTemplateService(FuelReqPricingTemplateEntityService fuelReqPricingTemplateEntityService, FboLinxContext context) : base(fuelReqPricingTemplateEntityService)
        {
            _context = context;
            _fuelReqPricingTemplateEntityService = fuelReqPricingTemplateEntityService;
        }
    }
}