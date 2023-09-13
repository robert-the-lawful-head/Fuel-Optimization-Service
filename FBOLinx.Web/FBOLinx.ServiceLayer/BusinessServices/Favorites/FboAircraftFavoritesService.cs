using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Favorites
{
    public interface IFboAircraftFavoritesService
    {
        Task<FboFavoriteAircraft> AddAircraftFavorite(FboFavoriteAircraft fboFavoriteAircraft);
        Task<bool> DeleteAircraftFavorite(int oid);
        Task<List<FboFavoriteAircraft>> GetAircraftfavoritesByFboId(int fboId);
        Task SaveBulkCustomerFavoriteAircraft(List<int> customerAircraftIds, int fboId);
        Task BulkDeleteCustomerFavoriteAircraftByCustomerAicraftId(List<int> customerAircraftIds);
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

        public async Task SaveBulkCustomerFavoriteAircraft(List<int> customerAircraftIds, int fboId)
        {
            var fboFavoriteAircrafts = new List<FboFavoriteAircraft>();
            foreach(var caId in customerAircraftIds)
            {
                fboFavoriteAircrafts.Add(new FboFavoriteAircraft()
                {
                    FboId = fboId,
                    CustomerAircraftsId = caId
                });
            }
            await _fboFavoriteAircraftsRepo.AddRangeAsync(fboFavoriteAircrafts);
        }
        public async Task BulkDeleteCustomerFavoriteAircraftByCustomerAicraftId(List<int> customerAircraftIds)
        {
            var customerAircrafts = await _fboFavoriteAircraftsRepo.Where(x => customerAircraftIds.Contains(x.CustomerAircraftsId)).ToListAsync();

            foreach (var entity in customerAircrafts)
            {
                await _fboFavoriteAircraftsRepo.DeleteAsync(entity);
            }
        }
    }
}
