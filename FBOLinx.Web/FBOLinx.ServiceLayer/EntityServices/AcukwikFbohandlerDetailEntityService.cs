using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IAcukwikFbohandlerDetailEntityService : IRepository<AcukwikFbohandlerDetail, FboLinxContext>
    {
        Task<AcukwikAirport> GetAcukwikAirport(int fboAcukwikId);
    }

    public class AcukwikFbohandlerDetailEntityService : Repository<AcukwikFbohandlerDetail, DegaContext>, IAcukwikFbohandlerDetailEntityService
    {
        private readonly DegaContext _Context;

        public AcukwikFbohandlerDetailEntityService(DegaContext context) : base(context)
        {
            _Context = context;
        }

        public async Task<AcukwikAirport> GetAcukwikAirport(int fboAcukwikId)
        {
            var acukwikAirport= await (from afh in _Context.AcukwikFbohandlerDetail
                                       join aa in _Context.AcukwikAirports on afh.AirportId equals aa.Oid
                                       where afh.HandlerId == fboAcukwikId
                                       select aa).FirstOrDefaultAsync();

            return acukwikAirport;
        }
    }
}
