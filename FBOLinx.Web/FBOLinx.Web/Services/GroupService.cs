using FBOLinx.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using Geolocation;
using FBOLinx.Web.Models.Requests;
using System.IO;
using System.Transactions;
using EFCore.BulkExtensions;

namespace FBOLinx.Web.Services
{
    public class GroupService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private readonly FilestorageContext _fileStorageContext;
        private readonly IServiceProvider _services;
        public GroupService(FboLinxContext context, DegaContext degaContext, IServiceProvider services, FilestorageContext fileStorageContext)
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var changeableGroups = request.Groups.Where(group => group.Oid != request.BaseGroupId).Select(group => group.Oid).ToList();

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

                    var baseCustomerInfoByGroups = _context.CustomerInfoByGroup
                        .Where(a => a.GroupId == request.BaseGroupId)
                        .ToList()
                        .Concat(customerInfoByGroups)
                        .ToList();
                    var distinctedByCustomerIDCustomerInfoByGroups = baseCustomerInfoByGroups
                        .GroupBy(cg => new { cg.CustomerId, cg.GroupId })
                        .Select(g => g.First())
                        .ToList();
                    var duplicatedByCustomerIDCustomerInfoByGroups = baseCustomerInfoByGroups
                        .Except(distinctedByCustomerIDCustomerInfoByGroups)
                        .ToList();
                    await _context.BulkDeleteAsync(duplicatedByCustomerIDCustomerInfoByGroups);

                    var baseContactInfoByGroups = _context.ContactInfoByGroup
                        .Where(a => a.GroupId == request.BaseGroupId)
                        .ToList()
                        .Concat(contactInfoByGroups)
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
                    groupedByCustomerNameCustomerInfoByGroups.ForEach(async groupedResult =>
                    {
                        if (groupedResult.Count() > 0)
                        {
                            var customerIds = groupedResult
                                .Skip(1)
                                .Select(cg => cg.CustomerId)
                                .ToList();
                            var replacingCustomerContacts = _context.CustomerContacts
                                .Where(c => customerIds.Contains(c.CustomerId))
                                .ToList();
                            replacingCustomerContacts.ForEach(c => c.CustomerId = groupedResult.First().CustomerId);
                            await _context.BulkUpdateAsync(replacingCustomerContacts);
                            await _context.BulkDeleteAsync(groupedResult.Skip(1).ToList());

                            var duplicatedCustomerAircrafts = _context.CustomerAircrafts.Where(ca => customerIds.Contains(ca.CustomerId)).ToList();
                            duplicatedCustomerAircrafts.ForEach(ca => ca.CustomerId = groupedResult.First().CustomerId);
                            await _context.BulkDeleteAsync(duplicatedCustomerAircrafts);
                        }
                    });

                    var baseCustomerAircrafts = _context.CustomerAircrafts
                       .Where(a => a.GroupId == request.BaseGroupId)
                       .ToList();
                    var distinctedCustomerAircrafts = baseCustomerAircrafts
                        .GroupBy(ca => new { ca.GroupId, ca.CustomerId, ca.TailNumber, ca.AircraftId, ca.Size })
                        .Select(g => g.First())
                        .ToList();
                    var duplicatedCustomerAircrafts = baseCustomerAircrafts
                        .Except(distinctedCustomerAircrafts)
                        .ToList();

                    duplicatedCustomerAircrafts.ForEach(async ca =>
                    {
                        var aircraftPrices = _context.AircraftPrices.Where(ap => ap.CustomerAircraftId == ca.Oid).ToList();
                        var baseCustomerAircraft = distinctedCustomerAircrafts.Where(dca => dca.GroupId == ca.GroupId
                            && dca.CustomerId == ca.CustomerId
                            && dca.TailNumber == ca.TailNumber
                            && dca.AircraftId == ca.AircraftId
                            && dca.Size == ca.Size
                        ).First();
                        aircraftPrices.ForEach(ap => ap.CustomerAircraftId = baseCustomerAircraft.Oid);
                        await _context.BulkUpdateAsync(aircraftPrices);
                    });

                    await _context.BulkDeleteAsync(duplicatedCustomerAircrafts);

                    var removingGroups = _context.Group.Where(a => changeableGroups.Contains(a.Oid));
                    await _context.BulkDeleteAsync(removingGroups.ToList());

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
