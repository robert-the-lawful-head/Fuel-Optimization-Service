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
using FBOLinx.DB.Models;

namespace FBOLinx.Web.Services
{
    public class FboService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _degaContext;
        private readonly FilestorageContext _fileStorageContext;
        private readonly IServiceProvider _services;
        public FboService(FboLinxContext context, DegaContext degaContext, IServiceProvider services, FilestorageContext fileStorageContext)
        {
            _context = context;
            _degaContext = degaContext;
            _services = services;
            _fileStorageContext = fileStorageContext;
        }

        public async Task DoLegacyGroupTransition(int groupId)
        {
            var customerMargins = await (from cm in _context.CustomerMargins
                                         join pt in _context.PricingTemplate on new { cm.TemplateId, marginType = (short)1 } equals new { TemplateId = pt.Oid, marginType = (short)pt.MarginType }
                                         where cm.Amount < 0
                                         select cm).ToListAsync();
            foreach (var cm in customerMargins)
            {
                cm.Amount = -cm.Amount;
            }
            _context.CustomerMargins.UpdateRange(customerMargins);
            await _context.SaveChangesAsync();

            var deleteAircraftPricesTask = Task.Run(async () =>
            {
                using (var scope = _services.CreateScope())
                {
                    var groupTransitionService = scope.ServiceProvider.GetRequiredService<GroupTransitionService>();
                    await groupTransitionService.PerformLegacyGroupTransition(groupId);
                }
            });
        }

        public async Task<FBOLinx.DB.Models.Fbos> GetFbo(int fboId)
        {
            var result = await _context.Fbos.Where(x => x.Oid == fboId).Include(x => x.Group)
                .Include(x => x.fboAirport).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Coordinate> GetFBOLocation(int fboid)
        {
            var fboAirport = await _context.Fboairports.Where(fa => fa.Fboid == fboid).FirstOrDefaultAsync();
            if (fboAirport == null)
                return new Coordinate(0, 0);
            var airport = await _degaContext.AcukwikAirports.Where(a => a.Icao == fboAirport.Icao).FirstOrDefaultAsync();

            var latDirection = airport.Latitude.Substring(0, 1);
            var lngDirection = airport.Longitude.Substring(0, 1);

            double latitude = double.Parse(airport.Latitude.Substring(1, 2)) + double.Parse(airport.Latitude.Substring(4, 2)) / 60 + double.Parse(airport.Latitude[7..]) / 3600;
            double longitude = airport.Longitude.Length == 8 ?
                double.Parse(airport.Longitude.Substring(1, 2)) + double.Parse(airport.Longitude.Substring(4, 2)) / 60 + double.Parse(airport.Longitude[6..]) / 3600 :
                double.Parse(airport.Longitude.Substring(1, 3)) + double.Parse(airport.Longitude.Substring(5, 2)) / 60 + double.Parse(airport.Longitude[7..]) / 3600;

            if (latDirection != "N") latitude = -latitude;
            if (lngDirection != "E") longitude = -longitude;

            return new Coordinate(latitude, longitude);
        }

        public async Task<string> UploadLogo(FboLogoRequest fboLogoRequest)
        {
            var imageAsArray = Convert.FromBase64String(fboLogoRequest.FileData);

            var existingRecord = await _fileStorageContext.FboLinxImageFileData.Where(f => f.FboId == fboLogoRequest.FboId).ToListAsync();
            if (existingRecord.Count > 0)
            {
                existingRecord[0].FileData = imageAsArray;
                existingRecord[0].FileName = fboLogoRequest.FileName;
                existingRecord[0].ContentType = fboLogoRequest.ContentType;
                _fileStorageContext.FboLinxImageFileData.Update(existingRecord[0]);
            }
            else
            {
                FBOLinx.DB.Models.FboLinxImageFileData fboLinxImageFileData = new DB.Models.FboLinxImageFileData();
                fboLinxImageFileData.FileData = imageAsArray;
                fboLinxImageFileData.FileName = fboLogoRequest.FileName;
                fboLinxImageFileData.ContentType = fboLogoRequest.ContentType;
                fboLinxImageFileData.FboId = fboLogoRequest.FboId;
                _fileStorageContext.FboLinxImageFileData.Add(fboLinxImageFileData);
            }

            await _fileStorageContext.SaveChangesAsync();

            var imageBase64 = Convert.ToBase64String(imageAsArray, 0, imageAsArray.Length);
            return fboLogoRequest.ContentType + ";base64," + imageBase64;
        }

        public async Task<string> GetLogo(int fboId)
        {
            var existingRecord = await _fileStorageContext.FboLinxImageFileData.Where(f => f.FboId == fboId).ToListAsync();
            if (existingRecord.Count > 0)
            {
                var imageBase64 = Convert.ToBase64String(existingRecord[0].FileData, 0, existingRecord[0].FileData.Length);
                return existingRecord[0].ContentType + ";base64," + imageBase64;
            }

            return "";
        }

        public async Task DeleteLogo(int fboId)
        {
            var existingRecord = await _fileStorageContext.FboLinxImageFileData.Where(f => f.FboId == fboId).SingleOrDefaultAsync();
            if (existingRecord.Oid > 0)
            {
                _fileStorageContext.FboLinxImageFileData.Remove(existingRecord);
                await _fileStorageContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetFBOIcao(int fboId)
        {
            var fboAirport = await _context.Fboairports.Where(fa => fa.Fboid == fboId).FirstOrDefaultAsync();

            return fboAirport.Icao;
        }

        public async Task<List<string>> GetToEmailsForEngagementEmails(int fboId)
        {
            List<string> toEmails = new List<string>();

            var fboInfo = await _context.Fbos.FindAsync(fboId);
            //var responseFbo = await _apiClient.GetAsync("fbos/" + fbo.Oid, conductorUser.Token);
            //var fboInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Fbos>(responseFbo);

            if (fboInfo.FuelDeskEmail != "")
                toEmails.Add(fboInfo.FuelDeskEmail);

            var fboContacts = await _context.Fbocontacts
                                .Include("Contact")
                                .Where(x => x.Fboid == fboId && !string.IsNullOrEmpty(x.Contact.Email))
                                .Select(f => new Contacts
                                {
                                    FirstName = f.Contact.FirstName,
                                    LastName = f.Contact.LastName,
                                    Title = f.Contact.Title,
                                    Oid = f.Oid,
                                    Email = f.Contact.Email,
                                    Primary = f.Contact.Primary,
                                    CopyAlerts = f.Contact.CopyAlerts,
                                    CopyOrders = f.Contact.CopyOrders
                                })
                                .ToListAsync();
            //var responseFboContacts = await _apiClient.GetAsync("fbocontacts/fbo/" + fbo.Oid, conductorUser.Token);
            //var fboContacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FboContactsViewModel>>(responseFboContacts);

            foreach (Contacts fboContact in fboContacts)
            {
                if (fboContact.CopyAlerts.GetValueOrDefault())
                    toEmails.Add(fboContact.Email);
            }

            return toEmails;
        }

        public async Task<List<Fbos>> GetFbosByIcaos(string icaos)
        {
            var fbos = await (from f in _context.Fbos
                              join fa in _context.Fboairports on f.Oid equals fa.Fboid
                              where icaos.Contains(fa.Icao) && f.GroupId > 1
                              select f).ToListAsync();
            return fbos;
        }
    }
}
