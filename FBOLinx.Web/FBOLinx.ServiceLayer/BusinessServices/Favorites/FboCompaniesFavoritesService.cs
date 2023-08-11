using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Favorites
{
    public interface IFboCompaniesFavoritesService
    {
        Task<FboFavoriteCompany> AddCompanyFavorite(int fboId, int CustomerInfoByGroupId);
        Task<bool> DeleteCompanyFavorite(int oid);
        Task<List<FboFavoriteCompany>> GetCompaniesFavoritesByFboId(int fboId);
    }

    public class FboCompaniesFavoritesService : IFboAircraftFavoritesService
    {
        private IRepository<FboFavoriteCompany, FboLinxContext> _FboFavoriteCompanyRepo;

        public FboCompaniesFavoritesService(IRepository<FboFavoriteCompany, FboLinxContext> FboFavoriteCompanyRepo)
        {
            _FboFavoriteCompanyRepo = FboFavoriteCompanyRepo;
        }

        public Task<FboFavoriteAircraft> AddAircraftFavorite(int fboId, int aircraftId)
        {
            throw new NotImplementedException();
        }

        public Task<FboFavoriteCompany> AddCompanyFavorite(int fboId, int CustomerInfoByGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAircraftFavorite(int oid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCompanyFavorite(int oid)
        {
            throw new NotImplementedException();
        }

        public Task<List<FboFavoriteAircraft>> GetAircraftFavoriteByFboId(int fboId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FboFavoriteCompany>> GetCompaniesFavoritesByFboId(int fboId)
        {
            throw new NotImplementedException();
        }
    }
}
