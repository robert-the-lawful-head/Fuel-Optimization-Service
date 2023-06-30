using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Models.Dega;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IAcukwikServicesOfferedEntityService : IRepository<AcukwikServicesOffered, FboLinxContext>
    {
        Task<AcukwikServicesOffered> FindByComposeKeyAsync(int handlerId, int serviceOfferedId);
        Task<AcukwikServicesOffered> FindByComposeKeyAsync(int composeKey);
        IQueryable<AcukwikServicesOffered> FindByComposeKeyIQueryable(int composeKey);
    }
    public class AcukwikServicesOfferedEntityService : Repository<AcukwikServicesOffered, DegaContext>, IAcukwikServicesOfferedEntityService
    {
        public AcukwikServicesOfferedEntityService(DegaContext context) : base(context)
        {
        }

        public async Task<AcukwikServicesOffered> FindByComposeKeyAsync(int handlerId, int serviceOfferedId)
        {
            return await this.dbSet.FindAsync(handlerId, serviceOfferedId);
        }

        public async Task<AcukwikServicesOffered> FindByComposeKeyAsync(int composeKey)
        {
            return await this.dbSet.FirstOrDefaultAsync(x => int.Parse(x.HandlerId.ToString()+x.ServiceOfferedId.ToString()) == composeKey);
        }
        public IQueryable<AcukwikServicesOffered> FindByComposeKeyIQueryable(int composeKey)
        {
            return this.dbSet.Where(x => (x.HandlerId.ToString() + x.ServiceOfferedId.ToString()) == composeKey.ToString());
        }
    }
}
