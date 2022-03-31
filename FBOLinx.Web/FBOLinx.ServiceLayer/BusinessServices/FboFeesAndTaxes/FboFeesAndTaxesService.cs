using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.FboFeesAndTaxesService
{
    public interface IFboFeesAndTaxesService
    {
        Task<List<FboFeesAndTaxes>> GetFboFeesAndTaxes(int fboId);
    }

    public class FboFeesAndTaxesService : IFboFeesAndTaxesService
    {
        private readonly FboLinxContext _context;
        private int _fboId;

        public FboFeesAndTaxesService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task<List<FboFeesAndTaxes>> GetFboFeesAndTaxes(int fboId)
        {
            var result = await _context.FbofeesAndTaxes.Where(x => x.Fboid == fboId).ToListAsync();
            return result;
        }
    }
}
