using AuthECAPI.Models;

namespace AuthECAPI.Services
{
    public interface IGoogleAuthService
    {
        Task<AppUser> AuthenticateGoogleUserAsync(string idToken);
    }
}