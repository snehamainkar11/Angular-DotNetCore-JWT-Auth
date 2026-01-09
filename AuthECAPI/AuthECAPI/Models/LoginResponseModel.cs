namespace AuthECAPI.Models
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public bool Succeeded { get; set; }
        public string? RefreshToken { get; set; }
        public int TokenExpiresIn { get; set; }
        public string Error { get; set; }
        public string UserId { get;  set; }
    }
}
