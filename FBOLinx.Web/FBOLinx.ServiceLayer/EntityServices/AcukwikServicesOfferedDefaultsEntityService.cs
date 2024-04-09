using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IAcukwikServicesOfferedDefaultsEntityService : IRepository<AcukwikServicesOfferedDefaults, FboLinxContext>
    {
        Task<List<AcukwikServicesOfferedDefaults>> GetAllAcukwikServicesOfferedDefaults();
    }
    public class AcukwikServicesOfferedDefaultsEntityService : Repository<AcukwikServicesOfferedDefaults, FboLinxContext>, IAcukwikServicesOfferedDefaultsEntityService
    {
        private FboLinxContext _context;

        public AcukwikServicesOfferedDefaultsEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AcukwikServicesOfferedDefaults>> GetAllAcukwikServicesOfferedDefaults()
        {
            var acukwikServicesOfferedDefaults = await _context.AcukwikServicesOfferedDefaults.ToListAsync();
            return acukwikServicesOfferedDefaults;
        }
    }
}
