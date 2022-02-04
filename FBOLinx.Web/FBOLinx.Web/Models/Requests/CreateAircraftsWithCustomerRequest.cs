using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;

namespace FBOLinx.Web.Models.Requests
{
    public class CreateAircraftsWithCustomerRequest
    {
        public int GroupId { get; set; }
        public int FboId { get; set; }
        public int AircraftId { get; set; }
        public string TailNumber { get; set; }
        public AircraftSizes Size { get; set; }
        public string Customer { get; set; }
    }
}
