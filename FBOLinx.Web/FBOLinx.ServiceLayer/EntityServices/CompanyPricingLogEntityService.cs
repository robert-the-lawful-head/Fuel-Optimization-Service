using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.CompanyPricingLog;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICompanyPricingLogEntityService : IRepository<CompanyPricingLog, FboLinxContext>
    {
        Task<List<CompanyPricingLogMostRecentQuoteModel>> GetMostRecentQuotesByAirport(string icao);

        Task<List<CompanyPricingLogCountByCustomer>> GetCompanyPricingLogCountByAirport(DateTime startDate,
            DateTime endDate, string icao);

        Task<List<CompanyPricingLogCountGroupAndFbo>> GetCompanyPricingLogCountByDateRange(
            DateTime startDate, DateTime endDate, int? fuelerlinxCompanyId);
    }

    public class CompanyPricingLogEntityService : Repository<CompanyPricingLog, FboLinxContext>, ICompanyPricingLogEntityService
    {
        public CompanyPricingLogEntityService(FboLinxContext context) : base(context)
        {
        }

        public async Task<List<CompanyPricingLogMostRecentQuoteModel>> GetMostRecentQuotesByAirport(string icao)
        {
            var result = await context.CompanyPricingLog.Where(x => x.ICAO == icao)
                .GroupBy(x => new { x.CompanyId, x.ICAO }).Select(x => new CompanyPricingLogMostRecentQuoteModel()
                {
                    FuelerLinxCompanyId = x.Key.CompanyId,
                    Icao = x.Key.ICAO,
                    MostRecentQuoteDateTime = x.Max(x => x.CreatedDate)
                }).ToListAsync();
            return result;
        }

        public async Task<List<CompanyPricingLogCountByCustomer>> GetCompanyPricingLogCountByAirport(DateTime startDate,
            DateTime endDate, string icao)
        {
            var result = await (from cpl in context.CompanyPricingLog
                join c in context.Customers on Math.Abs(cpl.CompanyId) equals Math.Abs(c.FuelerlinxId.HasValue ? c.FuelerlinxId.Value : 0)
                where cpl.ICAO == icao && cpl.CreatedDate >= startDate &&
                      cpl.CreatedDate <= endDate
                      group cpl by c.Oid into g
                select new CompanyPricingLogCountByCustomer()
                {
                    CustomerId = g.Key,
                    QuoteCount = g.Count(),
                    LastQuoteDate = g.Count() > 0 ? g.Max(x => x.CreatedDate) : null
                }).ToListAsync();

            return result;
        }

        public async Task<List<CompanyPricingLogCountGroupAndFbo>> GetCompanyPricingLogCountByDateRange(
            DateTime startDate, DateTime endDate, int? fuelerlinxCompanyId)
        {
            var result = await (from cpl in context.CompanyPricingLog
                                join fa in context.Fboairports on cpl.ICAO equals fa.Icao
                                join f in context.Fbos on fa.Fboid equals f.Oid
                                where cpl.CreatedDate >= startDate
                                      && cpl.CreatedDate <= endDate
                                      && (!fuelerlinxCompanyId.HasValue || cpl.CompanyId == fuelerlinxCompanyId.Value)
                                group cpl by new { FboID = f.Oid, GroupID = f.GroupId } into g
                                select new CompanyPricingLogCountGroupAndFbo()
                                {
                                    QuoteCount = g.Count(),
                                    FboId = g.Key.FboID,
                    GroupId = g.Key.GroupID
                                }).ToListAsync();
            return result;
        }
    }
}