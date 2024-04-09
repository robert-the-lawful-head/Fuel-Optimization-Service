using FBOLinx.DB.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IIntegrationPartnersEntityService : IRepository<IntegrationPartners, FboLinxContext>
    {
    }

    public class IntegrationPartnersEntityService : Repository<IntegrationPartners, FboLinxContext>, IIntegrationPartnersEntityService
    {
        private readonly FboLinxContext _context;

        public IntegrationPartnersEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }
    }
}
