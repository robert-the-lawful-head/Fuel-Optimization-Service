using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.Web.Models.Requests
{
    public class FuelReqsCompanyStatisticsForGroupRequest
    {
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        [Required]
        public List<int> FboIds { get; set; }
    }
}
