using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Favorites
{
    public interface IFboAircraftFavoritesService
    {
        Task<FboFavoriteAircraft> AddAircraftFavorite(FboFavoriteAircraft fboFavoriteAircraft, int groupId);
        Task<bool> DeleteAircraftFavorite(int oid, int groupId);
        Task SaveBulkCustomerFavoriteAircraft(List<int> customerAircraftIds, int fboId, int groupId);
        Task BulkDeleteCustomerFavoriteAircraftByCustomerAicraftId(List<int> customerAircraftIds, int fboId, int groupId);
        Task BulkDeleteCustomerFavoriteAircrafts(List<FboFavoriteAircraft> CustomerAicrafts, int fboId, int groupId);

    }

    public class FboAircraftFavoritesService : IFboAircraftFavoritesService
    {
        private IRepository<FboFavoriteAircraft, FboLinxContext> _fboFavoriteAircraftsRepo;
        private ICustomerAircraftService _CustomerAircraftService;

        public FboAircraftFavoritesService(IRepository<FboFavoriteAircraft, FboLinxContext> FboFavoriteAircraftsRepo,
            ICustomerAircraftService customerAircraftService)
        {
            _fboFavoriteAircraftsRepo = FboFavoriteAircraftsRepo;
            _CustomerAircraftService = customerAircraftService;
        }

        public async Task<FboFavoriteAircraft> AddAircraftFavorite(FboFavoriteAircraft fboFavoriteAircraft, int groupId)
        {
            var existingEntity = (await _fboFavoriteAircraftsRepo.GetAsync(x => x.CustomerAircraftsId == fboFavoriteAircraft.CustomerAircraftsId && x.FboId == fboFavoriteAircraft.FboId)).FirstOrDefault();

            if (existingEntity != null) return existingEntity;

            var result = await _fboFavoriteAircraftsRepo.AddAsync(fboFavoriteAircraft);
            
            _CustomerAircraftService.ClearCache(groupId,fboFavoriteAircraft.FboId);

            return result;
        }

        public async Task<bool> DeleteAircraftFavorite(int oid, int groupId)
        {
            var result = await _fboFavoriteAircraftsRepo.DeleteAsync(oid);
            _CustomerAircraftService.ClearCache(groupId, result.FboId);
            return true;
        }

        public async Task SaveBulkCustomerFavoriteAircraft(List<int> customerAircraftIds, int fboId, int groupId)
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

            _CustomerAircraftService.ClearCache(groupId, fboId);
        }
        public async Task BulkDeleteCustomerFavoriteAircraftByCustomerAicraftId(List<int> customerAircraftIds,int fboId, int groupId)
        {
            var customerAircrafts = await _fboFavoriteAircraftsRepo.Where(x => customerAircraftIds.Contains(x.CustomerAircraftsId)).ToListAsync();

            await _fboFavoriteAircraftsRepo.DeleteRangeAsync(customerAircrafts);
            _CustomerAircraftService.ClearCache(groupId, fboId);
        }

        public async Task BulkDeleteCustomerFavoriteAircrafts(List<FboFavoriteAircraft> CustomerAicrafts, int fboId, int groupId)
        {
            await _fboFavoriteAircraftsRepo.DeleteRangeAsync(CustomerAicrafts);
            _CustomerAircraftService.ClearCache(groupId, fboId);
        }
    }
}
