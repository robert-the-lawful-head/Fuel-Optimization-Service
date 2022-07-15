using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog
{
    public class MissedQuoteLogService : BaseDTOService<MissedQuoteLogDto, DB.Models.MissedQuoteLog, FboLinxContext>
    {
        public MissedQuoteLogService(MissedQuoteLogEntityService entityService) : base(entityService)
        {
        }
    }
}
