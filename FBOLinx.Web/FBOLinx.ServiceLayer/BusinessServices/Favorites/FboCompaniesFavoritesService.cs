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
        Task<FboFavoriteCompanies> AddCompanyFavorite(int fboId, int CustomerInfoByGroupId);
        Task<bool> DeleteCompanyFavorite(int oid);
        Task<List<FboFavoriteCompanies>> GetCompaniesFavoritesByFboId(int fboId);
    }

    public class FboCompaniesFavoritesService : IFboAircraftFavoritesService
    {
        private IRepository<FboFavoriteCompanies, FboLinxContext> _fboFavoriteCompaniesRepo;

        public FboCompaniesFavoritesService(IRepository<FboFavoriteCompanies, FboLinxContext> fboFavoriteCompaniesRepo)
        {
            _fboFavoriteCompaniesRepo = fboFavoriteCompaniesRepo;
        }

        public Task<FboFavoriteAircrafts> AddAircraftFavorite(int fboId, int aircraftId)
        {
            throw new NotImplementedException();
        }

        public Task<FboFavoriteCompanies> AddCompanyFavorite(int fboId, int CustomerInfoByGroupId)
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

        public Task<List<FboFavoriteAircrafts>> GetAircraftFavoriteByFboId(int fboId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FboFavoriteCompanies>> GetCompaniesFavoritesByFboId(int fboId)
        {
            throw new NotImplementedException();
        }
    }
}
