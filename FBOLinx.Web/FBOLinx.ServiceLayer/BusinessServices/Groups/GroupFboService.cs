using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.Requests.FBO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Groups
{
    public interface IGroupFboService
    {
        Task<Group> CreateNewGroup(string groupName);
        Task<Fbos> CreateNewFbo(SingleFboRequest request);
        Task DeleteFbo(int fboId);
        Task DeleteFbos(List<int> fboIds);
    }
    public class GroupFboService : IGroupFboService
    {
        private FboLinxContext _context;
        private IServiceScopeFactory _serviceScopeFactory;
        private FuelerLinxContext _fuelerLinxContext;
        private FuelerLinxApiService _fuelerLinxApiService;

        public GroupFboService(FboLinxContext context, IServiceScopeFactory serviceScopeFactory, FuelerLinxContext fuelerLinxContext, FuelerLinxApiService fuelerLinxApiService)
        {
            _fuelerLinxContext = fuelerLinxContext;
            _serviceScopeFactory = serviceScopeFactory;
            _context = context;
            _fuelerLinxApiService = fuelerLinxApiService;
        }

        #region Public Methods

        public async Task<Group> CreateNewGroup(string groupName)
        {
            Group group = new Group
            {
                GroupName = groupName,
                Active = true
            };
            try
            {
                _context.Group.Add(group);
                await _context.SaveChangesAsync();

                if (group.Oid != 0)
                {
                    try
                    {
                        await _fuelerLinxContext.Database.ExecuteSqlRawAsync("exec up_Insert_FBOlinxGroupIntofuelerList @GroupName='" + group.GroupName + "', @GroupID=" + group.Oid + "");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("SP error: " + ex.Message);
                    }

                    try
                    {
                        var task = Task.Run(async () =>
                        {
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var groupCustomersService = scope.ServiceProvider.GetRequiredService<IGroupCustomersService>();
                                await groupCustomersService.StartAircraftTransfer(group.Oid);
                            }

                        });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Customer Aircraft add error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Global error: " + ex.Message);
            }

            return group;
        }

        public async Task<Fbos> CreateNewFbo(SingleFboRequest request)
        {
            if (request.GroupId.GetValueOrDefault() == 0)
            {
                var group = await CreateNewGroup(request.Group);
                if (group == null || group.Oid == 0)
                    throw new Exception("There was an issue creating a new group.");
                request.GroupId = group.Oid;
            }

            Fbos fbo = new Fbos
            {
                Fbo = request.Fbo.Trim(),
                GroupId = request.GroupId,
                AcukwikFBOHandlerId = request.AcukwikFboHandlerId,
                Active = true,
                DateActivated = DateTime.Now,
                AccountType = request.AccountType
            };

            _context.Fbos.Add(fbo);
            await _context.SaveChangesAsync();

            Fboairports fboairport = new Fboairports
            {
                Icao = request.Icao,
                Iata = request.Iata,
                Fboid = fbo.Oid
            };
            _context.Fboairports.Add(fboairport);
            await _context.SaveChangesAsync();

            return fbo;
        }

        public async Task DeleteFbo(int fboId)
        {
            var customCustomerTypes = _context.CustomCustomerTypes.Where(c => c.Fboid.Equals(fboId));
            _context.CustomCustomerTypes.RemoveRange(customCustomerTypes);

            var customerCompanyTypes = _context.CustomerCompanyTypes.Where(c => c.Fboid.Equals(fboId));
            _context.CustomerCompanyTypes.RemoveRange(customerCompanyTypes);

            var customerDefaultTemplates = _context.CustomerDefaultTemplates.Where(c => c.Fboid.Equals(fboId));
            _context.CustomerDefaultTemplates.RemoveRange(customerDefaultTemplates);

            var customersViewedByFbos = _context.CustomersViewedByFbo.Where(c => c.Fboid.Equals(fboId));
            _context.CustomersViewedByFbo.RemoveRange(customersViewedByFbos);

            var distributionLogs = _context.DistributionLog.Where(c => c.Fboid.Equals(fboId));
            _context.DistributionLog.RemoveRange(distributionLogs);

            var distributionQueues = _context.DistributionQueue.Where(c => c.Fboid.Equals(fboId));
            _context.DistributionQueue.RemoveRange(distributionQueues);

            var emailContents = _context.EmailContent.Where(c => c.FboId.Equals(fboId));
            _context.EmailContent.RemoveRange(emailContents);

            var fboaircraftSizes = _context.FboaircraftSizes.Where(c => c.Fboid.Equals(fboId));
            _context.FboaircraftSizes.RemoveRange(fboaircraftSizes);

            var fboairports = _context.Fboairports.Where(c => c.Fboid.Equals(fboId));
            _context.Fboairports.RemoveRange(fboairports);

            var fbocontacts = _context.Fbocontacts.Where(c => c.Fboid.Equals(fboId));
            _context.Fbocontacts.RemoveRange(fbocontacts);

            var fbopreferences = _context.Fbopreferences.Where(c => c.Fboid.Equals(fboId));
            _context.Fbopreferences.RemoveRange(fbopreferences);

            var fboprices = _context.Fboprices.Where(c => c.Fboid.Equals(fboId));
            _context.Fboprices.RemoveRange(fboprices);

            var networkNotes = _context.NetworkNotes.Where(c => c.Fboid.Equals(fboId));
            _context.NetworkNotes.RemoveRange(networkNotes);

            var pricingTemplates = _context.PricingTemplate.Where(c => c.Fboid.Equals(fboId));
            _context.PricingTemplate.RemoveRange(pricingTemplates);

            var rampFees = _context.RampFees.Where(c => c.Fboid.Equals(fboId));
            _context.RampFees.RemoveRange(rampFees);

            var rampFeeSettings = _context.RampFeeSettings.Where(c => c.Fboid.Equals(fboId));
            _context.RampFeeSettings.RemoveRange(rampFeeSettings);

            var tempAddOnMargins = _context.TempAddOnMargin.Where(c => c.FboId.Equals(fboId));
            _context.TempAddOnMargin.RemoveRange(tempAddOnMargins);

            var users = _context.User.Where(c => c.FboId.Equals(fboId));
            _context.User.RemoveRange(users);

            var volumeScaleDiscounts = _context.VolumeScaleDiscount.Where(c => c.Fboid.Equals(fboId));
            _context.VolumeScaleDiscount.RemoveRange(volumeScaleDiscounts);

            var contractFuelRelationships = _context.ContractFuelRelationships.Where(c => c.Fboid.Equals(fboId));
            _context.ContractFuelRelationships.RemoveRange(contractFuelRelationships);

            var defaultDiscounts = _context.DefaultDiscount.Where(c => c.Fboid.Equals(fboId));
            _context.DefaultDiscount.RemoveRange(defaultDiscounts);

            var distributionEmailsBodies = _context.DistributionEmailsBody.Where(c => c.Fboid.Equals(fboId));
            _context.DistributionEmailsBody.RemoveRange(distributionEmailsBodies);

            var fbocustomerPricings = _context.FbocustomerPricing.Where(c => c.Fboid.Equals(fboId));
            _context.FbocustomerPricing.RemoveRange(fbocustomerPricings);

            var fbologos = _context.Fbologos.Where(c => c.Fboid.Equals(fboId));
            _context.Fbologos.RemoveRange(fbologos);

            var fbosalesTaxes = _context.FbosalesTax.Where(c => c.Fboid.Equals(fboId));
            _context.FbosalesTax.RemoveRange(fbosalesTaxes);

            var fuelReqs = _context.FuelReq.Where(c => c.Fboid.Equals(fboId));
            _context.FuelReq.RemoveRange(fuelReqs);

            var priceHistories = _context.PriceHistory.Where(c => c.Fboid.Equals(fboId));
            _context.PriceHistory.RemoveRange(priceHistories);

            var reminderEmailsToFbos = _context.ReminderEmailsToFbos.Where(c => c.Fboid.Equals(fboId));
            _context.ReminderEmailsToFbos.RemoveRange(reminderEmailsToFbos);

            var requestPricingTrackers = _context.RequestPricingTracker.Where(c => c.Fboid.Equals(fboId));
            _context.RequestPricingTracker.RemoveRange(requestPricingTrackers);

            var fbo = _context.Fbos.Find(fboId);
            if (fbo != null)
            {
                _context.Fbos.Remove(fbo);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteGroup(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("exec up_Delete_Group @OID = " + id);

            List<int> groupfboIds = await _context.Fbos.Where(f => f.GroupId.Equals(id)).Select(f => f.Oid).ToListAsync();
            await DeleteFbos(groupfboIds);

            var group = _context.Group.Find(id);
            _context.Group.Remove(group);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteFbos(List<int> fboIds)
        {
            foreach (int fboId in fboIds)
            {
                await _context.Database.ExecuteSqlRawAsync("exec up_Delete_Fbo @OID = " + fboId);
            }
        }

        #endregion
    }
}
