using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class AuthTokenResponse
    {
        public string Token => string.Format("Bearer {0}", AuthToken);
        public string AuthToken { get; set; }
        public DateTime AuthTokenExpiration { get; set; }
        public string RefreshToken { get; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public bool Success { get; }
        public string Message { get; }

        public AuthTokenResponse(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
        }

        public AuthTokenResponse(string authToken, DateTime authTokenExpiration, string refreshToken, DateTime? refreshTokenExpiration, string username, int userId, bool success = true, string message = null)
        {
            Success = success;
            Message = message;
            AuthToken = authToken;
            AuthTokenExpiration = authTokenExpiration;
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
            Username = username;
            UserId = userId;
        }
    }
}
