using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class OAuthRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PartnerId { get; set; }
    }
}
