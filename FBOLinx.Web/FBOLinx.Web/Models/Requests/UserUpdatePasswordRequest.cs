using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.Models.Requests
{
    public class UserUpdatePasswordRequest
    {
        public User User { get; set; }
        public string NewPassword { get; set; }
    }
}
