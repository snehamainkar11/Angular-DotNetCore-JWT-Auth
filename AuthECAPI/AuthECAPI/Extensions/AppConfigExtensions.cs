namespace AuthECAPI.Extensions
{
    public static class AppConfigExtensions
    {
        public static WebApplication ConfigCORS(this WebApplication app, IConfiguration config)
        {
            app.UseCors(options =>
                options.WithOrigins("http://localhost:4200").
                AllowAnyMethod().
                AllowAnyHeader());
            return app;
        }
    }
}
