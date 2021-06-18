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
    }
}
