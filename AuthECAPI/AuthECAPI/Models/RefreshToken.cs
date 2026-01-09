using System.ComponentModel.DataAnnotations;

namespace AuthECAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public string UserId { get; set; }
        public bool IsRevoked { get; set; }
    }

}
