using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.DTO.UseCaseModels;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FuelPrices;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FBOLinx.ServiceLayer.BusinessServices.Integrations
{
    public interface IIntegrationStatusService : IBaseDTOService<IntegrationStatusDTO, DB.Models.IntegrationStatus>
    {
        Task UpdateIntegrationStatus(IntegrationStatusDTO integrationStatusDto);
    }

    public class IntegrationStatusService : BaseDTOService<IntegrationStatusDTO, DB.Models.IntegrationStatus, FboLinxContext>, IIntegrationStatusService
    {
        private IIntegrationStatusEntityService _integrationStatusEntityService;
        private readonly FboLinxContext _context;
        private IServiceScopeFactory _ScopeFactory;

        public IntegrationStatusService(IIntegrationStatusEntityService entityService, FboLinxContext context, IServiceScopeFactory scopeFactory) : base(entityService)
        {
            _integrationStatusEntityService = entityService;
            _context = context;
            _ScopeFactory = scopeFactory;
        }

        public async Task UpdateIntegrationStatus(IntegrationStatusDTO integrationStatusDto)
        {
            var integrationStatus = await _integrationStatusEntityService.GetSingleBySpec(new IntegrationStatusSpecification(integrationStatusDto.IntegrationPartnerId, integrationStatusDto.FboId));

            if (integrationStatus != null && integrationStatus.Oid > 0)
            {
                integrationStatus.IsActive = integrationStatus.IsActive;
                await _integrationStatusEntityService.UpdateAsync(integrationStatus);
            }
            else
                await AddAsync(integrationStatusDto);
        }
    }
}
