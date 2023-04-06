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

        Task<List<CompanyPricingLogCountByDateRange>> GetCompanyPricingLogCountByDateRange(
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

        public async Task<List<CompanyPricingLogCountByDateRange>> GetCompanyPricingLogCountByDateRange(
            DateTime startDate, DateTime endDate, int? fuelerlinxCompanyId)
        {
            var result = await (from cpl in context.CompanyPricingLog
                join fa in context.Fboairports on cpl.ICAO equals fa.Icao
                join f in context.Fbos on fa.Fboid equals f.Oid
                where cpl.CreatedDate >= startDate 
                      && cpl.CreatedDate <= endDate
                      && (!fuelerlinxCompanyId.HasValue || cpl.CompanyId == fuelerlinxCompanyId.Value)
                group cpl by new { FboID = f.Oid, GroupID = f.GroupId } into g
                select new CompanyPricingLogCountByDateRange()
                {
                    QuoteCount = g.Count(),
                    FboId = g.Key.FboID,
                    GroupId = g.Key.GroupID.GetValueOrDefault()
                }).ToListAsync();
            return result;
        }
    }
}
