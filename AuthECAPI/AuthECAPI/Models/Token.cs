namespace AuthECAPI.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
        public string UserId { get; set; }
    }

}
