using FBOLinx.DB.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class AirportWatchService
    {
        private FboLinxContext _context;

        public AirportWatchService(FboLinxContext context)
        {
            _context = context;
        }


    }
}
