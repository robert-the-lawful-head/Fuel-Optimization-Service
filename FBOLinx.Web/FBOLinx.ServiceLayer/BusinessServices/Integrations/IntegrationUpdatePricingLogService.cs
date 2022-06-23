using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Integrations
{
    public class IntegrationUpdatePricingLogService
    {
        private IntegrationUpdatePricingLogEntityService _integrationPricingUpdateLogEntityService;
        public IntegrationUpdatePricingLogService(IntegrationUpdatePricingLogEntityService integrationPricingUpdateLogEntityService)
        {
            _integrationPricingUpdateLogEntityService = integrationPricingUpdateLogEntityService;
        }

        public async Task<IntegrationUpdatePricingLogDto> InsertLog(IntegrationUpdatePricingLogDto integrationUpdatePricingLog)
        {
            var integrationUpdatePricingLogEntity = integrationUpdatePricingLog.Map<DB.Models.IntegrationUpdatePricingLog>();

            await _integrationPricingUpdateLogEntityService.AddAsync(integrationUpdatePricingLogEntity);

            integrationUpdatePricingLog.Id = integrationUpdatePricingLogEntity.Id;
            return integrationUpdatePricingLog;
        }

        public async Task UpdateLog(IntegrationUpdatePricingLogDto integrationUpdatePricingLog)
        {
            var integrationUpdatePricingLogEntity = integrationUpdatePricingLog.Map<DB.Models.IntegrationUpdatePricingLog>();

            await _integrationPricingUpdateLogEntityService.UpdateAsync(integrationUpdatePricingLogEntity);
        }
    }
}
