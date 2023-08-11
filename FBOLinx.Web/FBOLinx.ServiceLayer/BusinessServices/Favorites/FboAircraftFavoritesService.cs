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
        Task<FboFavoriteAircraft> AddAircraftFavorite(int fboId, int aircraftId);
        Task<bool> DeleteAircraftFavorite(int oid);
        Task<List<FboFavoriteCompany>> GetCompaniesFavoritesByFboId(int fboId);
    }

    public class FboAircraftFavoritesService : IFboAircraftFavoritesService
    {
        private IRepository<FboFavoriteAircraft, FboLinxContext> _FboFavoriteAircraftsRepo;

        public FboAircraftFavoritesService(IRepository<FboFavoriteAircraft, FboLinxContext> FboFavoriteAircraftsRepo)
        {
            _FboFavoriteAircraftsRepo = FboFavoriteAircraftsRepo;
        }

        public Task<FboFavoriteAircraft> AddAircraftFavorite(int fboId, int aircraftId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAircraftFavorite(int oid)
        {
            throw new NotImplementedException();
        }

        public Task<List<FboFavoriteCompany>> GetCompaniesFavoritesByFboId(int fboId)
        {
            throw new NotImplementedException();
        }
    }
}
