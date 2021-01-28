using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class UserResetPasswordValidateRequest
    {
        public string Token { get; set; }
    }
}
