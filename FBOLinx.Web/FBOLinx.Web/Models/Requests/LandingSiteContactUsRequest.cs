﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class LandingSiteContactUsRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
    }
}
