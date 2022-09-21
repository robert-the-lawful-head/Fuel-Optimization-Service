using FBOLinx.Core.Enums;
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
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public string Role { get; set; }
        public string Icao { get; set; }
        public string Fbo { get; set; }

        public ExchangeRefreshTokenResponse(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
        }

        public ExchangeRefreshTokenResponse(string authToken, DateTime authTokenExpiration, string refreshToken, DateTime? refreshTokenExpiration, string groupName, int groupId, UserRoles role, string icao, string fbo, bool success = true, string message = null)
        {
            AuthToken = authToken;
            AuthTokenExpiration = authTokenExpiration;
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
            Success = success;
            Message = message;
            GroupName = groupName;
            GroupId = groupId;
            Role = FBOLinx.Core.Utilities.Enum.GetDescription(role);
            Icao = icao;
            Fbo = fbo;
        }
    }
}
