using FBOLinx.DB.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IIntegrationUpdatePricingLogEntityService : IRepository<IntegrationUpdatePricingLog, FboLinxContext>
    {
    }

    public class IntegrationUpdatePricingLogEntityService : Repository<IntegrationUpdatePricingLog, FboLinxContext>, IIntegrationUpdatePricingLogEntityService
    {
        private readonly FboLinxContext _context;

        public IntegrationUpdatePricingLogEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }
    }
}
