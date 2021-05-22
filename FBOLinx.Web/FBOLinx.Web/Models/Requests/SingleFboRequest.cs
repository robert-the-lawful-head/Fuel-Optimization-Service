using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;

namespace FBOLinx.Web.Models.Requests
{
    public class SingleFboRequest
    {
        [Required]
        public string Icao { get; set; }
        public string Iata { get; set; }
        [Required]
        public string Fbo { get; set; }
        public int? GroupId { get; set; }
        public string Group { get; set; }
        public string FuelDeskEmail { get; set; }
        [Required]
        public int AcukwikFboHandlerId { get; set; }
        public Fbos.AccountTypes AccountType { get; set; }
    }
}
