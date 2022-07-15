using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class MissedQuoteLogEntityService : Repository<MissedQuoteLog, FboLinxContext>
    {
        private readonly FboLinxContext _context;
        private readonly IFboService _fboService;
        public MissedQuoteLogEntityService(FboLinxContext context, IFboService fboService) : base(context)
        {
            _context = context;
            _fboService = fboService;
        }

        public async Task<List<MissedQuoteLogDto>> GetRecentMissedQuotes(int fboId, bool isGridView = false)
        {
            var daysBefore = DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0));
            var recentMissedQuotes = await _context.MissedQuoteLog.Where(m => m.FboId == fboId && m.CreatedDate >= daysBefore).OrderByDescending(o => o.CreatedDate).ToListAsync();

            var recentMissedQuotedList = new List<MissedQuoteLogDto>();
            var localTimeZone = "";
            localTimeZone = await _fboService.GetAirportTimeZoneByFboId(fboId);

            foreach (MissedQuoteLog missedQuoteLog in recentMissedQuotes)
            {
                var localDateTime = DateTime.UtcNow;

                if (isGridView)
                {
                    localDateTime = await _fboService.GetAirportLocalDateTimeByUtcFboId(missedQuoteLog.CreatedDate.GetValueOrDefault(), fboId);
                }

                var missedQuoteLogDto = new MissedQuoteLogDto();
                missedQuoteLogDto.CreatedDateString = localDateTime.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;
                missedQuoteLogDto.CustomerId = missedQuoteLog.CustomerId.GetValueOrDefault();
                missedQuoteLogDto.Emailed = missedQuoteLog.Emailed.GetValueOrDefault();
                missedQuoteLogDto.FboId = missedQuoteLog.FboId;
                missedQuoteLogDto.Oid = missedQuoteLog.Oid;
                recentMissedQuotedList.Add(missedQuoteLogDto);
            }

            return recentMissedQuotedList;
        }
    }
}
