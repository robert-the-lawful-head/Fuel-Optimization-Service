using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.Core.Enums;

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

        public string GenerateToken(int id, int? fboid, UserRoles role, int? groupId, int expireMinutes = 10080)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Role, ((short) role).ToString()),
                    new Claim(ClaimTypes.GroupSid, groupId.ToString()),
                    new Claim(ClaimTypes.Sid,  fboid.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var signedEncodedToken = tokenHandler.WriteToken(token);
            return signedEncodedToken;

            //var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            //var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

            //var securityTokenDescriptor = new SecurityTokenDescriptor()
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim("UserID", id.ToString()),
            //        new Claim("Name", name),
            //        new Claim("Username", username),
            //        new Claim("Email", username.Contains("@") ? username : username + "@fbolinx.com"),
            //        new Claim("FBO", fboid.HasValue ? fboid.ToString() : ""),
            //        new Claim("Provider", "FBOLINX")
            //    }),
            //    SigningCredentials = signingCredentials,
            //    Expires = DateTime.UtcNow.AddMinutes(expireMinutes)
            //};

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            //var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            //return signedAndEncodedToken;
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
                return System.Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                    .Value);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public static UserRoles GetClaimedRole(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return (UserRoles)Convert.ToInt16(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value);
            }
            catch (System.Exception)
            {
                return UserRoles.NotSet;
            }
        }

        public static int GetClaimedFboId(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return System.Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public static int GetClaimedGroupId(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return System.Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.GroupSid).Value);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }
    }
}
