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
    }
}
