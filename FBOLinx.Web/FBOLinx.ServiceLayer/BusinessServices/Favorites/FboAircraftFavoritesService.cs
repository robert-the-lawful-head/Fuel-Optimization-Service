using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Favorites
{
    public interface IFboAircraftFavoritesService
    {
        Task<FboFavoriteAircrafts> AddAircraftFavorite(int fboId, int aircraftId);
        Task<bool> DeleteAircraftFavorite(int oid);
        Task<List<FboFavoriteCompanies>> GetCompaniesFavoritesByFboId(int fboId);
    }

    public class FboAircraftFavoritesService : IFboAircraftFavoritesService
    {
        private IRepository<FboFavoriteAircrafts, FboLinxContext> _fboFavoriteAircraftsRepo;

        public FboAircraftFavoritesService(IRepository<FboFavoriteAircrafts, FboLinxContext> fboFavoriteAircraftsRepo)
        {
            _fboFavoriteAircraftsRepo = fboFavoriteAircraftsRepo;
        }

        public Task<FboFavoriteAircrafts> AddAircraftFavorite(int fboId, int aircraftId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAircraftFavorite(int oid)
        {
            throw new NotImplementedException();
        }

        public Task<List<FboFavoriteCompanies>> GetCompaniesFavoritesByFboId(int fboId)
        {
            throw new NotImplementedException();
        }
    }
}
