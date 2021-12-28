using System.Threading.Tasks;
using Reports.Auth.Core.Services.Communication;

namespace Reports.Auth.Core.Services
{
    public interface IAuthenticationService
    {
         Task<TokenResponse> CreateAccessTokenAsync(string email, string password);
         Task<TokenResponse> RefreshTokenAsync(string refreshToken, string userEmail);
         void RevokeRefreshToken(string refreshToken);
    }
}