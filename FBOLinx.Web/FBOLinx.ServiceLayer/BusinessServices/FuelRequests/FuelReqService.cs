using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Microsoft.AspNetCore.Mvc;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelRequests
{
    public interface IFuelReqService : IBaseDTOService<FuelReqDto, FuelReq>
    {
        Task<List<FuelReq>> GetFuelOrdersForFbo(int fboId, DateTime? startDateTime = null, DateTime? endDateTime = null);
        Task<List<FuelReqsGridViewModel>> GetFuelReqsByGroupAndFbo(int groupId, int fboId, DateTime startDateTime, DateTime endDateTime);
    }

    public class FuelReqService : BaseDTOService<FuelReqDto, DB.Models.FuelReq, FboLinxContext>, IFuelReqService
    {
        private FuelReqEntityService _FuelReqEntityService;
        private readonly FuelerLinxApiService _fuelerLinxService;
        private readonly FboLinxContext _context;

        public FuelReqService(FuelReqEntityService fuelReqEntityService, FuelerLinxApiService fuelerLinxService, FboLinxContext context) : base(fuelReqEntityService)
        {
            _FuelReqEntityService = fuelReqEntityService;
            _fuelerLinxService = fuelerLinxService;
            _context = context;
        }

        public async Task<List<FuelReq>> GetFuelOrdersForFbo(int fboId, DateTime? startDateTime = null, DateTime? endDateTime = null)
        {
            var startDate = startDateTime == null ? DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0)) : startDateTime;
            var endDate = endDateTime == null ? DateTime.UtcNow.Add(new TimeSpan(3, 0, 0, 0)) : endDateTime;
            var requests =
                await _FuelReqEntityService.GetListBySpec(new FuelReqByFboAndDateSpecification(fboId, startDate.Value, endDate.Value));

            return requests;
        }

        public async Task<List<FuelReqsGridViewModel>> GetFuelReqsByGroupAndFbo(int groupId, int fboId, DateTime startDateTime, DateTime endDateTime)
        {
            Fboairports airport = _context.Fboairports.Where(x => x.Fboid == fboId).FirstOrDefault();
            string fbo = _context.Fbos.Where(f => f.Oid.Equals(fboId)).Select(f => f.Fbo).FirstOrDefault();

            var customers = await (from c in _context.Customers
                                   join ci in _context.CustomerInfoByGroup on c.Oid equals ci.CustomerId
                                   where c.FuelerlinxId > 0 && ci.GroupId == groupId
                                   select new
                                   {
                                       c.FuelerlinxId,
                                       ci.Company
                                   }).ToListAsync();

            FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _fuelerLinxService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = endDateTime, StartDateTime = startDateTime, Icao = airport.Icao, Fbo = fbo });

            List<FuelReqsGridViewModel> fuelReqsFromFuelerLinx = new List<FuelReqsGridViewModel>();


            foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result)
            {
                transaction.CustomerName = customers.Where(x => x.FuelerlinxId == transaction.CompanyId).Select(x => x.Company).FirstOrDefault();
                fuelReqsFromFuelerLinx.Add(FuelReqsGridViewModel.Cast(transaction));

            }

            List<FuelReqsGridViewModel> fuelReqVM = await
                (from fr in _context.FuelReq
                 join c in _context.CustomerInfoByGroup on new { GroupId = groupId, CustomerId = (fr.CustomerId ?? 0) } equals new { c.GroupId, c.CustomerId }
                 join ca in _context.CustomerAircrafts on fr.CustomerAircraftId equals ca.Oid
                 join f in _context.Fbos on fr.Fboid equals f.Oid
                 join frp in _context.FuelReqPricingTemplate on fr.Oid equals frp.FuelReqId
                 into leftJoinedFRP
                 from frp in leftJoinedFRP.DefaultIfEmpty()
                 where fr.Fboid == fboId && fr.Eta > startDateTime && fr.Eta < endDateTime
                 select new FuelReqsGridViewModel
                 {
                     Oid = fr.Oid,
                     ActualPpg = fr.ActualPpg,
                     ActualVolume = fr.ActualVolume,
                     Archived = fr.Archived,
                     Cancelled = fr.Cancelled,
                     CustomerId = fr.CustomerId,
                     DateCreated = fr.DateCreated,
                     DispatchNotes = fr.DispatchNotes,
                     Eta = fr.Eta,
                     Etd = fr.Etd,
                     Icao = fr.Icao,
                     Notes = fr.Notes,
                     QuotedPpg = fr.QuotedPpg,
                     QuotedVolume = fr.QuotedVolume,
                     Source = fr.Source,
                     SourceId = fr.SourceId,
                     TimeStandard = fr.TimeStandard,
                     CustomerName = c == null ? "" : c.Company,
                     TailNumber = ca == null ? "" : ca.TailNumber,
                     FboName = f == null ? "" : f.Fbo,
                     Email = fr.Email,
                     PhoneNumber = fr.PhoneNumber,
                     PricingTemplateName = frp == null ? "" : frp.PricingTemplateName,
                     FuelOn = fr.FuelOn
                 }
                )
                .OrderByDescending(f => f.Oid)
                .ToListAsync();

            fuelReqVM.AddRange(fuelReqsFromFuelerLinx);

            return fuelReqVM;
        }
    }
}
