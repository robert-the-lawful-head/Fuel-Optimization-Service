using System;
using System.Collections.Generic;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.DTO
{
    public class FbosGridViewModel
    {
        public int Oid { get; set; }
        public string Fbo { get; set; }
        public bool? Active { get; set; }
        public string Icao { get; set; }
        public string Iata { get; set; }
        public int GroupId { get; set; }
        public int NeedAttentionCustomers { get; set; }
        public bool PricingExpired { get; set; }
        public bool AccountExpired { get; set; }
        public DateTime? LastLogin { get; set; }
        public ICollection<User> Users { get; set; }
        public int Quotes30Days { get; set; }
        public int Orders30Days { get; set; }
        public double? CostPrice { get; set; }
        public double? RetailPrice { get; set; }
        public int? AcukwikFboHandlerId { get; set; }
    }
}
