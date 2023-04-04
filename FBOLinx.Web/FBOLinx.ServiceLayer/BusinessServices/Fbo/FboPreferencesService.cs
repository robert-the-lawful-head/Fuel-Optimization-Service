using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Fbo
{
    public interface IFboPreferencesService : IBaseDTOService<FboPreferencesDTO, DB.Models.Fbopreferences>
    {
        public Task<Array> GetFboProducts(int fboId);
        Task<FboPreferencesDTO> GetPreferenceForFbo(int fboId);
    }
    public class FboPreferencesService : BaseDTOService<FboPreferencesDTO, DB.Models.Fbopreferences, FboLinxContext>, IFboPreferencesService
    {

        public FboPreferencesService(IFboPreferencesEntityService entityService) : base(entityService)
        {

        }

        public async Task<Array> GetFboProducts(int fboId)
        {
            var fboProducts = new List<string>();
            var fboPreferences = await GetPreferenceForFbo(fboId);
            if (fboPreferences.EnableJetA.GetValueOrDefault())
                fboProducts.Add("JetA");
            if (fboPreferences.EnableSaf.GetValueOrDefault())
                fboProducts.Add("SAF");

            return fboProducts.ToArray();
        }

        public async Task<FboPreferencesDTO> GetPreferenceForFbo(int fboId)
        {
            var fboPreferences = await GetSingleBySpec(new FboPreferencesByFboIdSpecification(fboId));
            if (fboPreferences == null)
            {
                fboPreferences = await AddAsync(new FboPreferencesDTO()
                {
                    Fboid = fboId
                });
            }
            return fboPreferences;
        }
    }
}
