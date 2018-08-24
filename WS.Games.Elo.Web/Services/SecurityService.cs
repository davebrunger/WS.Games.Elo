using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WS.Games.Elo.Lib.Services;

namespace WS.Games.Elo.Web.Services
{
    public class SecurityService
    {
        private readonly ISecurityServiceConfiguration configuration;    
        private readonly UserService userService;

        public SecurityService(UserService userService, ISecurityServiceConfiguration configuration)
        {
            this.userService = userService;
            this.configuration = configuration;
        }

        public string GetNewToken(string username, string password)
        {
            var user = userService.GetUser(username);
            if (user?.Password != password)
            {
                return null;
            }
            var claims = new Claim[] {
                new Claim(ClaimTypes.Name, username),
            };
            var signingCredentials = new SigningCredentials(configuration.SecretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration.Issuer,
                audience: configuration.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(configuration.TokenExpiresInDays),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}