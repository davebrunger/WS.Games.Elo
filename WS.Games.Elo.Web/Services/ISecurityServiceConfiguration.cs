using Microsoft.IdentityModel.Tokens;

namespace WS.Games.Elo.Web.Services
{
    public interface ISecurityServiceConfiguration
    {
        SecurityKey SecretKey { get; }
        string Issuer { get; }
        string Audience { get; }
        double TokenExpiresInDays { get; }
    }
}