using Microsoft.IdentityModel.Tokens;
using WS.Games.Elo.Web.Services;

namespace WS.Games.Elo.Web
{
    internal class SecurityServiceConfiguration : ISecurityServiceConfiguration
    {
        public SecurityKey SecretKey { get; }
        public string Issuer { get; }
        public string Audience { get; }
        public double TokenExpiresInDays { get; }

        public SecurityServiceConfiguration(SecurityKey secretKey, string issuer, string audience, double tokenExpiresInDays)
        {
            this.SecretKey = secretKey;
            this.Issuer = issuer;
            this.Audience = audience;
            this.TokenExpiresInDays = tokenExpiresInDays;

        }
    }
}