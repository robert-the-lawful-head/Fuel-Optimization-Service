using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class UserAuthTokenFromAccessTokenRequest
    {
        public string AccessToken { get; set; }
    }
}
