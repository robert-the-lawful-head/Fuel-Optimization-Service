using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
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
        private IFboAircraftFavoritesService _FboAircraftFavoritesService;
        private ICustomerInfoByGroupService _CustomerInfoByGroupService;

        public FboCompaniesFavoritesService(IRepository<FboFavoriteCompany, FboLinxContext> FboFavoriteCompanyRepo, ICustomerInfoByGroupService CustomerInfoByGroupService, IFboAircraftFavoritesService FboAircraftFavoritesService)
        {
            _FboFavoriteCompanyRepo = FboFavoriteCompanyRepo;
            _CustomerInfoByGroupService = CustomerInfoByGroupService;
            _FboAircraftFavoritesService = FboAircraftFavoritesService;
        }

        public async Task<FboFavoriteCompany> AddCompanyFavorite(FboFavoriteCompany fboFavoriteCompany)
        {
            var existingEntity = (await _FboFavoriteCompanyRepo.GetAsync(x => x.CustomerInfoByGroupId == fboFavoriteCompany.CustomerInfoByGroupId && x.FboId == fboFavoriteCompany.FboId)).FirstOrDefault();

            if (existingEntity != null) return existingEntity;

            var result = await _FboFavoriteCompanyRepo.AddAsync(fboFavoriteCompany);

            var customerInfo = await _CustomerInfoByGroupService.GetById(fboFavoriteCompany.CustomerInfoByGroupId);
            var aircrafts = customerInfo.Customer.CustomerAircrafts.Select(x => x.Oid).ToList();

            await _FboAircraftFavoritesService.SaveBulkCustomerFavoriteAircraft(aircrafts,fboFavoriteCompany.FboId);

            return result;
        }
        public async Task<bool> DeleteCompanyFavorite(int oid)
        {
            var favoriteData = await _FboFavoriteCompanyRepo.FindAsync(oid);
            var customerInfo = await _CustomerInfoByGroupService.GetById(favoriteData.CustomerInfoByGroupId);
            var aircrafts = customerInfo.Customer.CustomerAircrafts.Select(x => x.FavoriteAircraft).ToList();

            await _FboAircraftFavoritesService.BulkDeleteCustomerFavoriteAircrafts(aircrafts);
            await _FboFavoriteCompanyRepo.DeleteAsync(oid);

            return true;
        }

        public async Task<List<FboFavoriteCompany>> GetCompaniesFavoritesByFboId(int fboId)
        {
            return _FboFavoriteCompanyRepo.Where(x => x.FboId == fboId).ToList();
        }
    }
}
