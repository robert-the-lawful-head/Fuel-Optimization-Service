﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.Models.Requests
{
    public class MergeGroupRequest
    {
        public int BaseGroupId { get; set; }
        public List<Group> Groups { get; set; }
    }
}
