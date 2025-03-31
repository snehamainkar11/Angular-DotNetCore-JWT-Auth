using AuthECAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthECAPI.Extensions
{
    public static  class EFCoreExtensions
    {
        public static IServiceCollection InjectDBContext(this IServiceCollection services ,IConfiguration config)
        {
            services.AddDbContext<AppDBContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DevDB")));
           
            return services;
        }
    }
}
