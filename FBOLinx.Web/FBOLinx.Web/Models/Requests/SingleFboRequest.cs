using System.ComponentModel.DataAnnotations;
using FBOLinx.Core.Enums;

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
        public AccountTypes AccountType { get; set; } = AccountTypes.RevFbo;
    }
}
