using FBOLinx.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;

namespace FBOLinx.Web.Services
{
    public class RefreshTokenService
    {
        private readonly FboLinxContext _context;

        public RefreshTokenService(FboLinxContext context)
        {
            _context = context;
        }
    }
}
