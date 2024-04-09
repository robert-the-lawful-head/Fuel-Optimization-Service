using FBOLinx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.DTO
{
    public class GroupViewModel : Group
    {
        public int NeedAttentionCustomers { get; set; }
        public DateTime? LastLogin { get; set; }
        public int FboCount { get; set; }
        public int ActiveFboCount { get; set; }
        public int ExpiredFboPricingCount { get; set; }
        public int ExpiredFboAccountCount { get; set; }
        public int Quotes30Days { get; set; }
        public int Orders30Days { get; set; }
        public bool HasPremiumFbos { get; set; }
        public bool HasActiveFbos { get; set; }
    }
}
