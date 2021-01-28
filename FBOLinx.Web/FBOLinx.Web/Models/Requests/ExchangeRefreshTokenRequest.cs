using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class ExchangeRefreshTokenRequest
    {
        [Required]
        public string AuthToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
