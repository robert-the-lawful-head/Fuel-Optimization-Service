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

namespace FBOLinx.Web.Services
{
    public class RampFeesService
    {
        private readonly FboLinxContext _context;
        private readonly DegaContext _DegaContext;
        private readonly AircraftService _aircraftService;

        public RampFeesService(FboLinxContext context, DegaContext degaContext, AircraftService aircraftService)
        {
            _context = context;
            _DegaContext = degaContext;
            _aircraftService = aircraftService;
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
