using AuthECAPI.Models;

namespace AuthECAPI.Services
{
    public interface ITokenService
    {
        public Task<Token> GenerateAccessToken(AppUser user);
        public  Task<RefreshToken> GenerateRefreshToken(string userId);

    }
}
