using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class UserUpdatePasswordRequest
    {
        public Models.User User { get; set; }
        public string NewPassword { get; set; }
    }
}
