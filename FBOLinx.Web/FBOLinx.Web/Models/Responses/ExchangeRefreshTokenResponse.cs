using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class ExchangeRefreshTokenResponse
    {
        public string AuthToken { get; set; }
        public DateTime AuthTokenExpiration { get; set; }
        public string RefreshToken { get; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public ExchangeRefreshTokenResponse(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
        }

        public ExchangeRefreshTokenResponse(string authToken, DateTime authTokenExpiration, string refreshToken, DateTime? refreshTokenExpiration, bool success = true, string message = null)
        {
            AuthToken = authToken;
            AuthTokenExpiration = authTokenExpiration;
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
            Success = success;
            Message = message;
        }
    }
}
