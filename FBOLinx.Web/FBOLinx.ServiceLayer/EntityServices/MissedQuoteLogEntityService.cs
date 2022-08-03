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
    public interface IMissedQuoteLogEntityService : IRepository<MissedQuoteLog, FboLinxContext>
    {
    }
    public class MissedQuoteLogEntityService : Repository<MissedQuoteLog, FboLinxContext>, IMissedQuoteLogEntityService
    {
        public MissedQuoteLogEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
