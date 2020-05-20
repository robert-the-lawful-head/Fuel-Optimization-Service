using FBOLinx.Web.Configurations;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Responses;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Web.Auth
{
    public class JwtManager
    {
        private IHttpContextAccessor _HttpContextAccessor;
        private readonly FboLinxContext _context;
        private readonly AppSettings _appSettings;

        public JwtManager(IHttpContextAccessor httpContextAccessor, FboLinxContext context, IOptions<AppSettings> appSettings)
        {
            _HttpContextAccessor = httpContextAccessor;
            _context = context;
            _appSettings = appSettings.Value;
        }

        public string GenerateToken(int id, string name, string username, int? fboid, int expireMinutes = 10080)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserID", id.ToString()),
                    new Claim("Name", name),
                    new Claim("Username", username),
                    new Claim("Email", username.Contains("@") ? username : username + "@fuelerlinx.com"),
                    new Claim("FBO", fboid.HasValue ? fboid.ToString() : ""),
                    new Claim("Provider", "FUELERLINX")
                }),
                SigningCredentials = signingCredentials,
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            return signedAndEncodedToken;
        }

        public string GetCurrentAuthToken()
        {
            try
            {
                return _HttpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        public int? GetUserID(string token)
        {
            var claims = GetPrincipal(token);
            if (claims == null)
                return null;

            var userId = claims.FindFirst(x => x.Type == "UserID");
            if (userId == null)
                return null;

            return int.Parse(userId.Value);
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                if (token == null)
                    return null;

                token = token.Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Encoding.UTF8.GetBytes(_appSettings.Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                    ValidateLifetime = false
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User> GetClaimedUser()
        {
            try
            {
                int userId = GetClaimedUserId(_HttpContextAccessor);
                if (userId == 0)
                    return null;
                var user = await _context.User.FindAsync(userId);
                return user;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public static int GetClaimedUserId(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst("UserID").Value);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public static int GetClaimedCompanyId(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst("Company").Value);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public static string GetClaimedName(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return httpContextAccessor.HttpContext.User.FindFirst("Name").Value;
            }
            catch (System.Exception)
            {
                return "";
            }
        }
    }
}
