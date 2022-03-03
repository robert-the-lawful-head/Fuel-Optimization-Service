using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Services.Interfaces;
using FBOLinx.Web.ViewModels;
using FBOLinx.Core.Enums;
using FBOLinx.Web.Models.Requests;

namespace FBOLinx.Web.Services
{
    public class PricingTemplateService : IPricingTemplateService
    {
        private readonly FboLinxContext _context;
        private readonly FilestorageContext _fileStorageContext;
        private int _FboId;
        private int _GroupId;
        public PricingTemplateService(FboLinxContext context, FilestorageContext fileStorageContext)
        {
            _context = context;
            _fileStorageContext = fileStorageContext;
        }
        public async Task FixCustomCustomerTypes(int groupId, int fboId)
        {
            if (groupId == 0 || fboId == 0)
            {
                return;
            }

            PricingTemplate defaultPricingTemplate = await _context.PricingTemplate
                                                                        .Where(x => x.Fboid == fboId && x.Default == true)
                                                                        .FirstOrDefaultAsync();

            if (defaultPricingTemplate == null)
            {
                return;
            }

            List<int> pricingTemplates = await _context.PricingTemplate
                                                            .Where(p => p.Fboid.Equals(fboId))
                                                            .Select(p => p.Oid)
                                                            .ToListAsync();
            var filteredList = await (
                from cg in _context.CustomerInfoByGroup
                join cct in _context.CustomCustomerTypes
                on new { customerId = cg.CustomerId, fboId }
                   equals
                   new
                   {
                       customerId = cct.CustomerId,
                       fboId = cct.Fboid
                   } into leftJoinCCT
                from cct in leftJoinCCT.DefaultIfEmpty()
                where cg.GroupId.Equals(groupId)
                select new
                {
                    cg.CustomerId,
                    cct
                })
                .Distinct()
                .ToListAsync();

            filteredList.ForEach(c =>
            {
                if (c.cct == null)
                {
                    CustomCustomerTypes newcct = new CustomCustomerTypes
                    {
                        CustomerId = c.CustomerId,
                        CustomerType = defaultPricingTemplate.Oid,
                        Fboid = fboId
                    };
                    _context.CustomCustomerTypes.Add(newcct);
                }
                else
                {
                    bool customerTypeExits = pricingTemplates.Any(p => p.Equals(c.cct.CustomerType));
                    if (!customerTypeExits)
                    {
                        c.cct.CustomerType = defaultPricingTemplate.Oid;
                        _context.CustomCustomerTypes.Update(c.cct);
                    }
                }
            });

            await _context.SaveChangesAsync();
        }

        public async Task FixDefaultPricingTemplate(int fboId)
        {
            var existingPricingTemplates =
                   await _context.PricingTemplate.Where(x => x.Fboid == fboId).ToListAsync();
            if (existingPricingTemplates != null && existingPricingTemplates.Count != 0)
                return;

            //Add a default pricing template - project #1c5383
            var newTemplate = new PricingTemplate()
            {
                Default = true,
                Fboid = fboId,
                Name = "Posted Retail",
                MarginType = MarginTypes.RetailMinus,
                Notes = ""
            };

            await _context.PricingTemplate.AddAsync(newTemplate);
            await _context.SaveChangesAsync();

            await AddDefaultCustomerMargins(newTemplate.Oid, 1, 500);
            await AddDefaultCustomerMargins(newTemplate.Oid, 501, 750);
            await AddDefaultCustomerMargins(newTemplate.Oid, 751, 1000);
            await AddDefaultCustomerMargins(newTemplate.Oid, 1001, 99999);
        }

        public async Task<List<PricingTemplate>> GetAllPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0, bool isAnalytics = false)
        {
            _FboId = fboId;
            _GroupId = groupId;

            List<PricingTemplate> result = new List<PricingTemplate>();
            var standardTemplates = await GetStandardPricingTemplatesForCustomerAsync(customer, fboId, groupId, pricingTemplateId);
            var aircraftPricesResult = await GetTailSpecificPricingTemplatesForCustomerAsync(customer, fboId, groupId, pricingTemplateId);

            result.AddRange(standardTemplates);

            //Set the applicable tail numbers for the aircraft-specific templates
            foreach (PricingTemplate aircraftPricingTemplate in aircraftPricesResult)
            {
                if (standardTemplates.Any(x => x.Oid == aircraftPricingTemplate.Oid))
                    continue;
                List<string> tailNumberList = (from ca in _context.CustomerAircrafts
                                               join ap in _context.AircraftPrices on ca.Oid equals ap.CustomerAircraftId
                                               where ap.PriceTemplateId == aircraftPricingTemplate.Oid
                                                     && ca.CustomerId == customer.CustomerId
                                                     && ca.GroupId == _GroupId
                                                     && !string.IsNullOrEmpty(ca.TailNumber)
                                               select ca.TailNumber.Trim()).ToList();
                if (tailNumberList == null || tailNumberList.Count == 0)
                    continue;
                aircraftPricingTemplate.Name += " - " + string.Join(",", tailNumberList);
                aircraftPricingTemplate.TailNumbers = tailNumberList;
                result.Add(aircraftPricingTemplate);
            }

            //Set the applicable tail numbers for the standard/default templates
            if (!isAnalytics || (isAnalytics && aircraftPricesResult.Count > 0))
            {
                var customerAircrafts = await _context.CustomerAircrafts.Where(x => x.CustomerId == customer.CustomerId && x.GroupId == groupId).ToListAsync();

                standardTemplates.ForEach(x => x.TailNumbers = customerAircrafts.Where(c => !string.IsNullOrEmpty(c.TailNumber) && !aircraftPricesResult.Any(a => a.TailNumbers != null && a.TailNumbers.Contains(c.TailNumber))).Select(c => c.TailNumber.Trim()).ToList());
            }
            else
                standardTemplates.ForEach(x => x.TailNumbers = new List<string>() { "All Tails" });

            return result;
        }

        public async Task<List<PricingTemplate>> GetStandardPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0)
        {
            _FboId = fboId;
            _GroupId = groupId;

            var result = await (from cg in _context.CustomerInfoByGroup
                                join c in _context.Customers on cg.CustomerId equals c.Oid
                                join cct in _context.CustomCustomerTypes on new
                                {
                                    customerId = cg.CustomerId,
                                    fboId = _FboId
                                } equals new
                                {
                                    customerId = cct.CustomerId,
                                    fboId = cct.Fboid
                                }
                                join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid

                                where cg.GroupId == _GroupId
                                      && cg.CustomerId == customer.CustomerId
                                      && (pricingTemplateId == 0 || pt.Oid == pricingTemplateId)
                                select pt).ToListAsync();

            return result;
        }

        public async Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0)
        {
            _FboId = fboId;
            _GroupId = groupId;

            var aircraftPricesResult = await (from ap in _context.AircraftPrices
                                              join ca in _context.CustomerAircrafts on ap.CustomerAircraftId equals ca.Oid
                                              join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                              where ca.CustomerId == customer.CustomerId
                                                    && ca.GroupId == _GroupId
                                                    && pt.Fboid == _FboId
                                                    && (pricingTemplateId == 0 || pt.Oid == pricingTemplateId)
                                              group pt by new { pt.Oid, pt.Name, pt.Fboid, pt.CustomerId, pt.Notes, pt.Type, pt.MarginType }
                into groupedPrices
                                              select new PricingTemplate()
                                              {
                                                  Oid = groupedPrices.Key.Oid,
                                                  Name = groupedPrices.Key.Name,
                                                  Fboid = groupedPrices.Key.Fboid,
                                                  CustomerId = groupedPrices.Key.CustomerId,
                                                  Notes = groupedPrices.Key.Notes,
                                                  Type = groupedPrices.Key.Type,
                                                  MarginType = groupedPrices.Key.MarginType

                                              }).ToListAsync();

            return aircraftPricesResult;
        }

        public async Task<List<PricingTemplatesGridViewModel>> GetPricingTemplates(int fboId, int groupId)
        {
            //Load customer assignments by template ID
            var customerAssignments = await (from cibg in _context.CustomerInfoByGroup
                                             join cct in _context.CustomCustomerTypes on cibg.CustomerId equals cct.CustomerId
                                             join c in _context.Customers on cibg.CustomerId equals c.Oid
                                             where cct.Fboid == fboId && cibg.GroupId == groupId && (c.Suspended == null || c.Suspended == false)
                                             select new
                                             { CustomerType = cct.CustomerType }).ToListAsync();

            //Separate inner queries first for FBO Prices and Margin Tiers
            var tempFboPrices = await _context.Fboprices
                                                .Where(fp => fp.EffectiveTo > DateTime.UtcNow && fp.Fboid == fboId && fp.Expired != true).ToListAsync();

            var tempMarginTiers = await (from c in _context.CustomerMargins
                                         join tm in _context.PriceTiers on c.PriceTierId equals tm.Oid
                                         group c by c.TemplateId into cmGroupResult
                                         select new
                                         {
                                             TemplateId = cmGroupResult.Key,
                                             MaxPriceTierValue = cmGroupResult.Max(c => Math.Abs(c.Amount.HasValue ? c.Amount.Value : 0)),
                                             MinPriceTierValue = cmGroupResult.Min(c => Math.Abs(c.Amount.HasValue ? c.Amount.Value : 0))
                                         }).ToListAsync();
            var tempPricingTemplates = await (_context.PricingTemplate.Where(x => x.Fboid == fboId).ToListAsync());

            //Join the inner queries on the pricing templates
            var pricingTemplates = (from p in tempPricingTemplates
                                    join cm in tempMarginTiers on p.Oid equals cm.TemplateId
                                        into leftJoinCmTiers
                                    from cm in leftJoinCmTiers.DefaultIfEmpty()
                                    join fp in tempFboPrices on "JetA " + p.MarginTypeProduct equals fp.Product
                                        into leftJoinFp
                                    from fp in leftJoinFp.DefaultIfEmpty()
                                    where p.Fboid == fboId && (fp == null || fp.EffectiveFrom == null || fp.EffectiveFrom <= DateTime.UtcNow)
                                    select new
                                    {
                                        p.CustomerId,
                                        p.Default,
                                        p.Fboid,
                                        Margin = cm == null ? 0 : (p.MarginType == MarginTypes.CostPlus ? cm.MaxPriceTierValue : cm.MinPriceTierValue),
                                        p.MarginType,
                                        p.Name,
                                        p.Notes,
                                        p.Oid,
                                        p.Type,
                                        p.Subject,
                                        p.Email,
                                        IsPricingExpired = fp == null && (p.MarginType == null || p.MarginType == MarginTypes.FlatFee) ? true : false,
                                        IntoPlanePrice = cm == null ? (fp != null ? fp.Price : 0) :
                                            p.MarginType == MarginTypes.CostPlus ? (fp != null ? fp.Price : 0) + cm.MaxPriceTierValue :
                                            (p.MarginType == MarginTypes.RetailMinus ? (fp != null ? fp.Price : 0) - cm.MinPriceTierValue : 0),
                                        TemplateId = cm == null ? 0 : cm.TemplateId,
                                        p.EmailContentId
                                    }).ToList();


            //Group the final result
            var result = (from pt in pricingTemplates
                          group pt by new
                          {
                              pt.CustomerId,
                              pt.Default,
                              pt.Fboid,
                              pt.Margin,
                              pt.MarginType,
                              pt.Name,
                              pt.Notes,
                              pt.Oid,
                              pt.Type,
                              pt.Subject,
                              pt.Email,
                              pt.IsPricingExpired,
                              pt.IntoPlanePrice,
                              pt.TemplateId,
                              pt.EmailContentId,
                          }
                    into groupedPt
                          select new PricingTemplatesGridViewModel
                          {
                              CustomerId = groupedPt.Key.CustomerId,
                              Default = groupedPt.Key.Default,
                              Fboid = groupedPt.Key.Fboid,
                              Margin = groupedPt.Key.Margin,
                              YourMargin = groupedPt.Key.Margin,
                              MarginType = groupedPt.Key.MarginType,
                              Name = groupedPt.Key.Name,
                              Notes = groupedPt.Key.Notes,
                              Oid = groupedPt.Key.Oid,
                              Type = groupedPt.Key.Type,
                              Subject = groupedPt.Key.Subject,
                              Email = groupedPt.Key.Email,
                              IsPricingExpired = groupedPt.Key.IsPricingExpired,
                              IntoPlanePrice = groupedPt.Key.IntoPlanePrice,
                              CustomersAssigned = customerAssignments.Sum(x => x.CustomerType == groupedPt.Key.TemplateId ? 1 : 0),
                              EmailContentId = groupedPt.Key.EmailContentId
                          })
                .OrderBy(pt => pt.Name)
                .ToList();

            return result;
        }

        public async Task<PricingTemplate> GetPricingTemplate(int pricingTemplateId)
        {
            var pricingTemplate = await _context.PricingTemplate.Where(p => p.Oid == pricingTemplateId).FirstOrDefaultAsync();
            return pricingTemplate;
        }

        public async Task<string> GetFileAttachment(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fileStorageContext.FbolinxPricingTemplateAttachments.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();

            if (pricingTemplateFile != null)
            {
                var fileBase64 = Convert.ToBase64String(pricingTemplateFile.FileData, 0, pricingTemplateFile.FileData.Length);
                return "data:" + pricingTemplateFile.ContentType + ";base64," + fileBase64;
            }

            return "";
        }

        public async Task<FbolinxPricingTemplateFileAttachment> GetFileAttachmentObject(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fileStorageContext.FbolinxPricingTemplateAttachments.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();
            return pricingTemplateFile;
        }

        public async Task<string> GetFileAttachmentName(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fileStorageContext.FbolinxPricingTemplateAttachments.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();

            if (pricingTemplateFile == null || pricingTemplateFile.Oid == 0)
                return "";

            return pricingTemplateFile.FileName;
        }

        public async Task UploadFileAttachment(FbolinxPricingTemplateAttachmentsRequest request)
        {
            var fileAsArray = Convert.FromBase64String(request.FileData);

            var existingRecord = await _fileStorageContext.FbolinxPricingTemplateAttachments.Where(f => f.PricingTemplateId == request.PricingTemplateId).FirstOrDefaultAsync();

            if (existingRecord != null && existingRecord.Oid > 0)
            {
                existingRecord.FileData = fileAsArray;
                existingRecord.FileName = request.FileName;
                existingRecord.ContentType = request.ContentType;
                _fileStorageContext.FbolinxPricingTemplateAttachments.Update(existingRecord);
            }
            else
            {
                FBOLinx.DB.Models.FbolinxPricingTemplateFileAttachment fboLinxFileData = new DB.Models.FbolinxPricingTemplateFileAttachment();
                fboLinxFileData.FileData = fileAsArray;
                fboLinxFileData.FileName = request.FileName;
                fboLinxFileData.ContentType = request.ContentType;
                fboLinxFileData.PricingTemplateId = request.PricingTemplateId;
                _fileStorageContext.FbolinxPricingTemplateAttachments.Add(fboLinxFileData);
            }

            await _fileStorageContext.SaveChangesAsync();
        }

        public async Task DeleteFileAttachment(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fileStorageContext.FbolinxPricingTemplateAttachments.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();

            _fileStorageContext.FbolinxPricingTemplateAttachments.Remove(pricingTemplateFile);
            await _fileStorageContext.SaveChangesAsync();
        }

        private async Task AddDefaultCustomerMargins(int priceTemplateId, double min, double max)
        {
            var newPriceTier = new PriceTiers() { Min = min, Max = max, MaxEntered = max };
            await _context.PriceTiers.AddAsync(newPriceTier);
            await _context.SaveChangesAsync();

            var newCustomerMargin = new CustomerMargins()
            {
                Amount = 0,
                TemplateId = priceTemplateId,
                PriceTierId = newPriceTier.Oid
            };
            await _context.CustomerMargins.AddAsync(newCustomerMargin);
            await _context.SaveChangesAsync();

        }
    }
}
