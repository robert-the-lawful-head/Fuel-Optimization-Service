using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Favorites
{
    public interface IFboAircraftFavoritesService
    {
        Task<FboFavoriteAircraft> AddAircraftFavorite(FboFavoriteAircraft fboFavoriteAircraft);
        Task<bool> DeleteAircraftFavorite(int oid);
        Task<List<FboFavoriteAircraft>> GetAircraftfavoritesByFboId(int fboId);
    }

    public class FboAircraftFavoritesService : IFboAircraftFavoritesService
    {
        private IRepository<FboFavoriteAircraft, FboLinxContext> _fboFavoriteAircraftsRepo;

        public FboAircraftFavoritesService(IRepository<FboFavoriteAircraft, FboLinxContext> FboFavoriteAircraftsRepo)
        {
            _fboFavoriteAircraftsRepo = FboFavoriteAircraftsRepo;
        }

        public async Task<FboFavoriteAircraft> AddAircraftFavorite(FboFavoriteAircraft fboFavoriteAircraft)
        {
             return await _fboFavoriteAircraftsRepo.AddAsync(fboFavoriteAircraft);
        }

        public async Task<bool> DeleteAircraftFavorite(int oid)
        {
            await _fboFavoriteAircraftsRepo.DeleteAsync(oid);
            return true;
        }

        public Task<List<FboFavoriteAircraft>> GetAircraftfavoritesByFboId(int fboId)
        {
            throw new NotImplementedException();
        }
    }
}
