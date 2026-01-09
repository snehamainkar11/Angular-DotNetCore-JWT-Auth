namespace AuthECAPI.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
