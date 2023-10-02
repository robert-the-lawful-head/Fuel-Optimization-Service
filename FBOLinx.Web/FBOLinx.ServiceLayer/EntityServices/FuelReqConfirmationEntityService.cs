using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Extensions;
using FBOLinx.DB;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IFuelReqConfirmationEntityService : IRepository<FuelReqConfirmation, FboLinxContext>
    {
        Task<List<FuelReqConfirmation>> GetFuelReqConfirmationByIds(List<int> ids);
    }

    public class FuelReqConfirmationEntityService : Repository<FuelReqConfirmation, FboLinxContext>, IFuelReqConfirmationEntityService
    {
        private readonly FboLinxContext _context;

        public FuelReqConfirmationEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FuelReqConfirmation>> GetFuelReqConfirmationByIds(List<int> ids)
        {
            var fuelReqConfirmationList = await (from fuelReqConfirmation in _context.FuelReqConfirmation
                                          join id in _context.AsTable(ids) on fuelReqConfirmation.SourceId equals System.Convert.ToInt32(id.Value)
                                          select fuelReqConfirmation).ToListAsync();
            return fuelReqConfirmationList;
        }
    }
}
