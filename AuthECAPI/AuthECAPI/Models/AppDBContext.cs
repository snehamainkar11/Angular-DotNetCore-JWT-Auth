using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthECAPI.Models
{
    public class AppDBContext :IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) :base(options)
        {
            
        }
        public DbSet<AppUser> AppUsers { get; set; }

    }
}
