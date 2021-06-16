using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.Web.ViewModels;
using static FBOLinx.DB.Models.RampFees;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;

namespace FBOLinx.Web.Services
{
    public class RampFeesService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _DegaContext;
        private readonly AircraftService _aircraftService;
        private IMailService _MailService;

        public RampFeesService(FboLinxContext context, DegaContext degaContext, AircraftService aircraftService, IMailService mailService)
        {
            _context = context;
            _DegaContext = degaContext;
            _aircraftService = aircraftService;
            _MailService = mailService;
        }

        public async Task<RampFees> GetRampFeeForAircraft(int fboId, string tailNumber)
        {
            var fbo = await _context.Fbos.Where(x => x.Oid == fboId).FirstOrDefaultAsync();

            if (fbo == null)
                return null;

            var customerAircraft = await _context.CustomerAircrafts.Where(s => s.TailNumber == tailNumber && s.GroupId == fbo.GroupId).Include(x => x.Aircraft).FirstOrDefaultAsync();
            if (customerAircraft == null)
                return null;

            var rampFees = await GetRampFeesForFbo(fboId, fbo.GroupId.GetValueOrDefault());
            if (rampFees == null)
                return null;

            var factorySpecs = await _DegaContext.AircraftSpecifications.FirstOrDefaultAsync(x => x.AircraftId == customerAircraft.AircraftId);

            var result = GetRampFeesThatApplyToCustomerAircraft(customerAircraft, rampFees, factorySpecs);

            if (result == null)
                return null;

            return result.OrderBy(x => x.GetCategoryPriority()).FirstOrDefault();

        }

        public async Task<List<RampFees>> GetRampFeesForFbo(int fboId, int groupId)
        {
            var rampFees = await _context.RampFees.Where(x => x.Fboid == fboId && (!x.ExpirationDate.HasValue || x.ExpirationDate >= DateTime.Today)).ToListAsync();
            if (rampFees == null)
                return null;

            List<int> aircraftIds = rampFees.Where(x => x.CategoryType == RampFees.RampFeeCategories.AircraftType).Select(x => x.CategoryMinValue.GetValueOrDefault()).ToList();

            var aircraftTypes = await _aircraftService.GetAllAircraftsAsQueryable().Where(x => aircraftIds.Any(a => a == x.AircraftId)).ToListAsync();

            rampFees.ForEach(x => {
                if (x.CategoryType != RampFees.RampFeeCategories.AircraftType)
                    return;
                var aircraft = aircraftTypes.FirstOrDefault(a => a.AircraftId == x.CategoryMinValue);
                if (aircraft == null)
                    return;
                x.CategoryDescription = aircraft.Make + " " + aircraft.Model;
            });

            return rampFees;
        }

        public async Task<IEnumerable<RampFeesGridViewModel>> GetRampFeesByFbo(int fboId)
        {
            //Grab all of the aircraft sizes and return a record for each size, even if the FBO hasn't customized them
            IEnumerable<FBOLinx.Core.Utilities.Enum.EnumDescriptionValue> sizes =
                FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(AirCrafts.AircraftSizes));
            var rampFees = await _context.RampFees.Where(x => x.Fboid == fboId).ToListAsync();
            var allAircraft = await _aircraftService.GetAllAircrafts();

            List<RampFeesGridViewModel> result = (
                from s in sizes
                join r in rampFees on new
                {
                    size = (int?)((short?)((AirCrafts.AircraftSizes)s.Value)),
                    fboId = (int?)fboId
                }
                    equals new
                    {
                        size = r.CategoryMinValue,
                        fboId = r.Fboid
                    }
                    into leftJoinRampFees
                from r in leftJoinRampFees.DefaultIfEmpty()
                select new RampFeesGridViewModel()
                {
                    Oid = r?.Oid ?? 0,
                    Price = r?.Price,
                    Waived = r?.Waived,
                    Fboid = r?.Fboid,
                    CategoryType = r?.CategoryType,
                    CategoryMinValue = r?.CategoryMinValue,
                    CategoryMaxValue = r?.CategoryMaxValue,
                    ExpirationDate = r?.ExpirationDate,
                    Size = (AirCrafts.AircraftSizes)s.Value,
                    AppliesTo = (from a in _aircraftService.GetAllAircraftsAsQueryable()
                                 where a.Size.HasValue && a.Size == (AirCrafts.AircraftSizes)s.Value
                                 select a).OrderBy((x => x.Make)).ThenBy((x => x.Model)).ToList(),
                    LastUpdated = r?.LastUpdated

                }).ToList();

            // Pull additional "custom" ramp fees(weight, tail, wingspan, etc.)
            List<RampFeesGridViewModel> customRampFees = (from r in rampFees
                                                          join a in allAircraft on r.CategoryMinValue equals (a.AircraftId) into leftJoinAircrafts
                                                          from a in leftJoinAircrafts.DefaultIfEmpty()
                                                          where r.Fboid == fboId && r.CategoryType.HasValue &&
                                                                r.CategoryType.Value != RampFeeCategories.AircraftSize
                                                          select new RampFeesGridViewModel()
                                                          {
                                                              Oid = r.Oid,
                                                              Price = r.Price,
                                                              Waived = r.Waived,
                                                              Fboid = r.Fboid,
                                                              CategoryType = r.CategoryType,
                                                              CategoryMinValue = r.CategoryMinValue,
                                                              CategoryMaxValue = r.CategoryMaxValue,
                                                              ExpirationDate = r.ExpirationDate,
                                                              AircraftMake = a == null ? "" : a.Make,
                                                              AircraftModel = a == null ? "" : a.Model,
                                                              CategoryStringValue = r.CategoryStringValue,
                                                              LastUpdated = r.LastUpdated
                                                          }).ToList();

            result.AddRange(customRampFees);

            return result;
        }

        public async Task NotifyFboNoRampFees(List<string> toEmails, string fbo, string customerName, string icao)
        {
            FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
            mailMessage.From = new MailAddress("donotreply@fbolinx.com");
            foreach (string email in toEmails)
            {
                if (_MailService.IsValidEmailRecipient(email))
                    mailMessage.To.Add(email);
            }

            var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridEngagementTemplateData
            {
                fboName = fbo,
                customerName = customerName,
                ICAO = icao,
                subject = "FBOLinx reminder - incomplete quotes"
            };

            mailMessage.SendGridEngagementTemplate = dynamicTemplateData;

            //Send email
            var result = await _MailService.SendAsync(mailMessage);
        }
        private List<RampFees> GetRampFeesThatApplyToCustomerAircraft(CustomerAircrafts customerAircraft, List<RampFees> validRampFees, AircraftSpecifications specifications)
        {
            if (validRampFees == null)
                return new List<RampFees>();

            var result = validRampFees.Where(servicesAndFeesByCompany => 
            (servicesAndFeesByCompany.CategoryType == RampFees.RampFeeCategories.AircraftType && customerAircraft.AircraftId == servicesAndFeesByCompany.CategoryMinValue) ||
                    (servicesAndFeesByCompany.CategoryType == RampFees.RampFeeCategories.AircraftSize && (customerAircraft.Size == (AirCrafts.AircraftSizes?)System.Convert.ToInt16(servicesAndFeesByCompany.CategoryMinValue) || customerAircraft?.Aircraft?.Size == (AirCrafts.AircraftSizes?)System.Convert.ToInt16(servicesAndFeesByCompany.CategoryMinValue))) ||
                    (servicesAndFeesByCompany.CategoryType == RampFees.RampFeeCategories.TailNumber && !string.IsNullOrEmpty(servicesAndFeesByCompany.CategoryStringValue) && servicesAndFeesByCompany.CategoryStringValue.Split(',').Contains(customerAircraft.TailNumber)) ||
                    (servicesAndFeesByCompany.CategoryType == RampFees.RampFeeCategories.WeightRange && servicesAndFeesByCompany.CategoryMinValue <= customerAircraft.Aircraft?.BasicOperatingWeight && servicesAndFeesByCompany.CategoryMaxValue >= customerAircraft.Aircraft?.BasicOperatingWeight) ||
                    (servicesAndFeesByCompany.CategoryType == RampFees.RampFeeCategories.Wingspan && servicesAndFeesByCompany.CategoryMinValue <= specifications?.FuselageDimensionsWingSpanFt && servicesAndFeesByCompany.CategoryMaxValue >= specifications?.FuselageDimensionsWingSpanFt)).ToList();

            return result;
        }
    }
}
