using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog
{
    public class MissedQuoteLogService : BaseDTOService<MissedQuoteLogDto, DB.Models.MissedQuoteLog, FboLinxContext>
    {
        private IFboService _FboService;

        public MissedQuoteLogService(MissedQuoteLogEntityService entityService, IFboService fboService) : base(entityService)
        {
            _FboService = fboService;
        }

        public async Task<List<MissedQuoteLogDto>> GetRecentMissedQuotes(int fboId, bool isGridView = false)
        {
            var daysBefore = DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0));
            var recentMissedQuotes = await _EntityService.GetListBySpec(new MissedQuoteLogSpecification(fboId, daysBefore));

            var recentMissedQuotedList = new List<MissedQuoteLogDto>();
            var localTimeZone = "";
            localTimeZone = await _FboService.GetAirportTimeZoneByFboId(fboId);

            foreach (DB.Models.MissedQuoteLog missedQuoteLog in recentMissedQuotes)
            {
                var localDateTime = DateTime.UtcNow;

                if (isGridView)
                {
                    localDateTime = await _FboService.GetAirportLocalDateTimeByUtcFboId(missedQuoteLog.CreatedDate.GetValueOrDefault(), fboId);
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
