using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Favorites
{
    public interface IFboCompaniesFavoritesService
    {
        Task<FboFavoriteCompany> AddCompanyFavorite(FboFavoriteCompany fboFavoriteCompany);
        Task<bool> DeleteCompanyFavorite(int oid);
        Task<List<FboFavoriteCompany>> GetCompaniesFavoritesByFboId(int fboId);
    }

    public class FboCompaniesFavoritesService : IFboCompaniesFavoritesService
    {
        private IRepository<FboFavoriteCompany, FboLinxContext> _FboFavoriteCompanyRepo;

        public FboCompaniesFavoritesService(IRepository<FboFavoriteCompany, FboLinxContext> FboFavoriteCompanyRepo)
        {
            _FboFavoriteCompanyRepo = FboFavoriteCompanyRepo;
        }

        public async Task<FboFavoriteCompany> AddCompanyFavorite(FboFavoriteCompany fboFavoriteCompany)
        {
            return await _FboFavoriteCompanyRepo.AddAsync(fboFavoriteCompany);
        }
        public async Task<bool> DeleteCompanyFavorite(int oid)
        {
            await _FboFavoriteCompanyRepo.DeleteAsync(oid);
            return true;
        }

        public async Task<List<FboFavoriteCompany>> GetCompaniesFavoritesByFboId(int fboId)
        {
            return _FboFavoriteCompanyRepo.Where(x => x.FboId == fboId).ToList();
        }
    }
}
