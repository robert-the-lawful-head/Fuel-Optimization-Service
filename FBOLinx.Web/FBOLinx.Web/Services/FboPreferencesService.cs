using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class FboPreferencesService
    {
        private readonly FboLinxContext _context;

        public FboPreferencesService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task<Array> GetFboProducts(int fboId)
        {
            var fboProducts = new List<string>();
            var fboPreferences = await _context.Fbopreferences.Where(f => f.Fboid == fboId).FirstOrDefaultAsync();
            if (fboPreferences.EnableJetA.GetValueOrDefault())
                fboProducts.Add("JetA");
            if (fboPreferences.EnableSaf.GetValueOrDefault())
                fboProducts.Add("SAF");

            return fboProducts.ToArray();
        }
    }
}
