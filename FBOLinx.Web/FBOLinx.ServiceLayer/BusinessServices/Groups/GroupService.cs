using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Requests.Groups;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Groups
{
    public interface IGroupService : IBaseDTOService<GroupDTO, DB.Models.Group>
    {
        Task<string> UploadLogo(GroupLogoRequest groupLogoRequest);
        Task<string> GetLogo(int groupId);
        Task DeleteLogo(int groupId);
        Task MergeGroups(MergeGroupRequest request);
    }

    public class GroupService : BaseDTOService<GroupDTO, DB.Models.Group, FboLinxContext>, IGroupService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private readonly FilestorageContext _fileStorageContext;
        private readonly IServiceProvider _services;
        public GroupService(IGroupEntityService entityService, FboLinxContext context, DegaContext degaContext, IServiceProvider services, FilestorageContext fileStorageContext) : base(entityService)
        {
            _context = context;
            _degaContext = degaContext;
            _services = services;
            _fileStorageContext = fileStorageContext;
        }
        public async Task<string> UploadLogo(GroupLogoRequest groupLogoRequest)
        {
            var imageAsArray = Convert.FromBase64String(groupLogoRequest.FileData);

            var existingRecord = await _fileStorageContext.FboLinxImageFileData.Where(f => f.GroupId == groupLogoRequest.GroupId).ToListAsync();
            if (existingRecord.Count > 0)
            {
                existingRecord[0].FileData = imageAsArray;
                existingRecord[0].FileName = groupLogoRequest.FileName;
                existingRecord[0].ContentType = groupLogoRequest.ContentType;
                _fileStorageContext.FboLinxImageFileData.Update(existingRecord[0]);
            }
            else
            {
                FBOLinx.DB.Models.FboLinxImageFileData fboLinxImageFileData = new DB.Models.FboLinxImageFileData();
                fboLinxImageFileData.FileData = imageAsArray;
                fboLinxImageFileData.FileName = groupLogoRequest.FileName;
                fboLinxImageFileData.ContentType = groupLogoRequest.ContentType;
                fboLinxImageFileData.GroupId = groupLogoRequest.GroupId;
                _fileStorageContext.FboLinxImageFileData.Add(fboLinxImageFileData);
            }

            await _fileStorageContext.SaveChangesAsync();

            var imageBase64 = Convert.ToBase64String(imageAsArray, 0, imageAsArray.Length);
            return groupLogoRequest.ContentType + ";base64," + imageBase64;
        }

        public async Task<string> GetLogo(int groupId)
        {
            var existingRecord = await _fileStorageContext.FboLinxImageFileData.Where(f => f.GroupId == groupId).ToListAsync();
            if (existingRecord.Count > 0)
            {
                var imageBase64 = Convert.ToBase64String(existingRecord[0].FileData, 0, existingRecord[0].FileData.Length);
                return existingRecord[0].ContentType + ";base64," + imageBase64;
            }

            return "";
        }

        public async Task DeleteLogo(int groupId)
        {
            var existingRecord = await _fileStorageContext.FboLinxImageFileData.Where(f => f.GroupId == groupId).SingleOrDefaultAsync();
            if (existingRecord.Oid > 0)
            {
                _fileStorageContext.FboLinxImageFileData.Remove(existingRecord);
                await _fileStorageContext.SaveChangesAsync();
            }
        }

        public async Task MergeGroups(MergeGroupRequest request)
        {
            var distinctedByCustomerIDCustomerInfoByGroups = new List<DB.Models.CustomerInfoByGroup>();
            var changeableGroups = request.Groups.Where(group => group.Oid != request.BaseGroupId).Select(group => group.Oid).ToList();

            try
            {
                var users = await _context.User.Where(a => changeableGroups.Contains((a.GroupId ?? 0))).ToListAsync();
                var fbos = await _context.Fbos.Where(a => changeableGroups.Contains((a.GroupId ?? 0))).ToListAsync();
                var adminEmails = await _context.AdminEmails.Where(a => changeableGroups.Contains(a.GroupId)).ToListAsync();
                var companiesByGroups = await _context.CompaniesByGroup.Where(a => changeableGroups.Contains(a.GroupId)).ToListAsync();
                var contactInfoByGroups = await _context.ContactInfoByGroup.Where(a => changeableGroups.Contains(a.GroupId)).ToListAsync();
                var customerAircrafts = await _context.CustomerAircrafts.Where(a => changeableGroups.Contains((a.GroupId ?? 0))).ToListAsync();
                var customerAircraftViewedByGroups = await _context.CustomerAircraftViewedByGroup.Where(a => changeableGroups.Contains((a.GroupId ?? 0))).ToListAsync();
                var customerCompanyTypes = await _context.CustomerCompanyTypes.Where(a => changeableGroups.Contains(a.GroupId)).ToListAsync();
                var customerInfoByGroups = await _context.CustomerInfoByGroup.Where(a => changeableGroups.Contains(a.GroupId)).ToListAsync();
                var customerNotes = await _context.CustomerNotes.Where(a => changeableGroups.Contains(a.GroupId)).ToListAsync();
                var customerSchedulingSoftwareByGroups = await _context.CustomerSchedulingSoftwareByGroup.Where(a => changeableGroups.Contains(a.GroupId)).ToListAsync();
                var distributionLogs = await _context.DistributionLog.Where(a => changeableGroups.Contains((a.GroupId ?? 0))).ToListAsync();
                var distributionQueues = await _context.DistributionQueue.Where(a => changeableGroups.Contains(a.GroupId)).ToListAsync();

                users.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(users);

                fbos.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(fbos);

                adminEmails.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(adminEmails);

                companiesByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(companiesByGroups);

                contactInfoByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(contactInfoByGroups);

                customerAircrafts.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(customerAircrafts);

                customerAircraftViewedByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(customerAircraftViewedByGroups);

                customerCompanyTypes.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(customerCompanyTypes);

                customerInfoByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(customerInfoByGroups);

                customerNotes.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(customerNotes);

                customerSchedulingSoftwareByGroups.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(customerSchedulingSoftwareByGroups);

                distributionLogs.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(distributionLogs);

                distributionQueues.ForEach(a => a.GroupId = request.BaseGroupId);
                await _context.BulkUpdateAsync(distributionQueues);

                List<DB.Models.CustomerInfoByGroup> baseCustomerInfoByGroupsData = await _context.CustomerInfoByGroup
                    .Where(a => a.GroupId == request.BaseGroupId)
                    .ToListAsync();
                var baseCustomerInfoByGroups = baseCustomerInfoByGroupsData.Concat(baseCustomerInfoByGroupsData)
                    .ToList();
                distinctedByCustomerIDCustomerInfoByGroups = baseCustomerInfoByGroups
                    .GroupBy(cg => new { cg.CustomerId, cg.GroupId })
                    .Select(g => g.First())
                    .ToList();
                var duplicatedByCustomerIDCustomerInfoByGroups = baseCustomerInfoByGroups
                    .Except(distinctedByCustomerIDCustomerInfoByGroups)
                    .ToList();
                await _context.BulkDeleteAsync(duplicatedByCustomerIDCustomerInfoByGroups);

                List<DB.Models.ContactInfoByGroup> baseContactInfoByGroupsData = await _context.ContactInfoByGroup
                    .Where(a => a.GroupId == request.BaseGroupId)
                    .ToListAsync();
                var baseContactInfoByGroups = baseContactInfoByGroupsData.Concat(contactInfoByGroups)
                    .ToList();
                var distinctedContactInfoByGroups = baseContactInfoByGroups
                    .GroupBy(cg => new { cg.GroupId, cg.Email })
                    .Select(g => g.First())
                    .ToList();
                var duplicatedContactInfoByGroups = baseContactInfoByGroups
                    .Except(distinctedContactInfoByGroups)
                    .ToList();
                await _context.BulkDeleteAsync(duplicatedContactInfoByGroups);


                var groupedByCustomerNameCustomerInfoByGroups = distinctedByCustomerIDCustomerInfoByGroups
                           .GroupBy(cg => new { Company = cg.Company.ToLower(), cg.GroupId })
                           .ToList();
                foreach (var groupedResult in groupedByCustomerNameCustomerInfoByGroups)
                {
                    if (groupedResult.Count() > 0)
                    {
                        var customerIds = groupedResult
                            .Skip(1)
                            .Select(cg => cg.CustomerId)
                            .ToList();
                        List<DB.Models.CustomerContacts> replacingCustomerContacts = await _context.CustomerContacts
                            .Where(c => customerIds.Contains(c.CustomerId))
                            .ToListAsync();
                        replacingCustomerContacts.ForEach(c => c.CustomerId = groupedResult.First().CustomerId);
                        await _context.BulkUpdateAsync(replacingCustomerContacts);
                        await _context.BulkDeleteAsync(groupedResult.Skip(1).ToList());

                        var duplicateCustomerAircrafts = await _context.CustomerAircrafts.Where(ca => customerIds.Contains(ca.CustomerId)).ToListAsync();
                        duplicateCustomerAircrafts.ForEach(ca => ca.CustomerId = groupedResult.First().CustomerId);
                        await _context.BulkUpdateAsync(duplicateCustomerAircrafts);
                    }
                }

                List<DB.Models.CustomerAircrafts> baseCustomerAircrafts = await _context.CustomerAircrafts
                       .Where(a => a.GroupId == request.BaseGroupId)
                       .ToListAsync();
                var distinctedCustomerAircrafts = baseCustomerAircrafts
                    .GroupBy(ca => new { ca.GroupId, ca.CustomerId, ca.TailNumber, ca.AircraftId }) //, ca.Size
                    .Select(g => g.First())
                    .ToList();
                var duplicatedCustomerAircrafts = baseCustomerAircrafts
                    .Except(distinctedCustomerAircrafts)
                    .ToList();

                foreach (var ca in duplicatedCustomerAircrafts)
                {
                    List<DB.Models.AircraftPrices> aircraftPrices = await _context.AircraftPrices.Where(ap => ap.CustomerAircraftId == ca.Oid).ToListAsync();
                    var baseCustomerAircraft = distinctedCustomerAircrafts.Where(dca => dca.GroupId == ca.GroupId
                        && dca.CustomerId == ca.CustomerId
                        && dca.TailNumber == ca.TailNumber
                        && dca.AircraftId == ca.AircraftId
                    //&& dca.Size == ca.Size
                    ).First();
                    aircraftPrices.ForEach(ap => ap.CustomerAircraftId = baseCustomerAircraft.Oid);
                    await _context.BulkUpdateAsync(aircraftPrices);
                }

                await _context.BulkDeleteAsync(duplicatedCustomerAircrafts);

                var removingGroups = await _context.Group.Where(a => changeableGroups.Contains(a.Oid)).ToListAsync();
                await _context.BulkDeleteAsync(removingGroups);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
