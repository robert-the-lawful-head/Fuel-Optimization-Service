using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Responses.Auth
{
    public class AuthenticatedLinkResponse
    {
        public string AccessToken { get; set; }
        public string FboEmails { get; set; }
        public string Fbo { get; set; }
    }
}
