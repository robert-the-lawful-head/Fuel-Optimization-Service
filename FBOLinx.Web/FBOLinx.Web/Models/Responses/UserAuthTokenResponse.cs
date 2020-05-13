using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class UserAuthTokenResponse
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

        public UserAuthTokenResponse(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
        }

        public UserAuthTokenResponse(string authToken, DateTime authTokenExpiration, string refreshToken, DateTime? refreshTokenExpiration, string username, int userId, bool success = true, string message = null)
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
