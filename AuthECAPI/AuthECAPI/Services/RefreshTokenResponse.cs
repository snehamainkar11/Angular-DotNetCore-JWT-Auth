namespace AuthECAPI.Services
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; internal set; }
        public string RefreshToken { get; internal set; }
        public int ExpiresIn { get; internal set; }
    }
}