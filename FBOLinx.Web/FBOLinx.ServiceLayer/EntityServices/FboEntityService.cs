using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IFboEntityService : IRepository<Fbos, FboLinxContext>
    {
        Task<List<FbosDto>> GetFbosByIcaos(string icaos);
    }

    public class FboEntityService : Repository<Fbos, FboLinxContext>, IFboEntityService
    {
        private FboLinxContext _context;
        public FboEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FbosDto>> GetFbosByIcaos(string icaos)
        {
            var fbos = await (from f in _context.Fbos
                              join fa in _context.Fboairports on f.Oid equals fa.Fboid
                              where icaos.Contains(fa.Icao) && f.GroupId > 1 && f.Active == true
                              select new FbosDto { Oid = f.Oid, Fbo = f.Fbo, GroupId = f.GroupId }).ToListAsync();
            return fbos;
        }
    }
}
