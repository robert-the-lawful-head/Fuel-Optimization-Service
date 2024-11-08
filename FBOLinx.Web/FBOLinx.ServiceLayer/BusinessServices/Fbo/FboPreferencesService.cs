using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Fbo
{
    public interface IFboPreferencesService : IBaseDTOService<FboPreferencesDTO, DB.Models.Fbopreferences>
    {
        public Task<Array> GetFboProducts(int fboId);
        Task<Fbopreferences> GetByFboId(int fboId);
        Task<string> GetDecimalPrecisionStringFormat(int? fboId);

    }
    public class FboPreferencesService : BaseDTOService<FboPreferencesDTO, DB.Models.Fbopreferences, FboLinxContext>, IFboPreferencesService
    {

        public FboPreferencesService(IFboPreferencesEntityService entityService) : base(entityService)
        {

        }

        public async Task<Array> GetFboProducts(int fboId)
        {
            var fboProducts = new List<string>();
            var fboPreferences = await GetSingleBySpec(new FboPreferencesByFboIdSpecification(fboId));

            if (fboPreferences == null)
            {
                fboPreferences = new FboPreferencesDTO() { Fboid = fboId, EnableJetA = true, EnableSaf = false };
                await AddAsync(fboPreferences);
            }

            if (fboPreferences.EnableJetA.GetValueOrDefault())
                fboProducts.Add("JetA");
            if (fboPreferences.EnableSaf.GetValueOrDefault())
                fboProducts.Add("SAF");

            return fboProducts.ToArray();
        }
        public async Task<Fbopreferences> GetByFboId(int fboId)
        {
            var result = this._EntityService.Where(x => x.Fboid == fboId);
            return await result.FirstOrDefaultAsync();

        }
        public async Task<string> GetDecimalPrecisionStringFormat(int? fboId)
        {
            var decimalPrecision = (fboId == null) ? 4 : (await this.GetByFboId((int)fboId))?.DecimalPrecision;

            return $"{{0:C{decimalPrecision}}}";
        }
    }
}
