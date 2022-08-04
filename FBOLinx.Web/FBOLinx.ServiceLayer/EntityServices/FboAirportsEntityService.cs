using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IFboAirportsEntityService : IRepository<Fboairports, FboLinxContext>
    {
        Task<FboAirportsDTO> GetFboAirportByFboId(int fboId);
    }

    public class FboAirportsEntityService : Repository<Fboairports, FboLinxContext>, IFboAirportsEntityService
    {
        private FboLinxContext _context;
        public FboAirportsEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FboAirportsDTO> GetFboAirportByFboId(int fboId)
        {
            var result = await (from fa in _context.Fboairports
                                where fa.Fboid == fboId
                                select new FboAirportsDTO { Icao = fa.Icao }).FirstOrDefaultAsync();
            return result;
        }
    }
}
