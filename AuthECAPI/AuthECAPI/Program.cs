using AuthECAPI.Data;
using AuthECAPI.DBContext;
using AuthECAPI.Extensions;
using AuthECAPI.FunctionTriggers;
using AuthECAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        In = ParameterLocation.Header,
        Description = "Please enter a token",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {  
      {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },[]
      }
    });
});


//services from identity core
builder.Services.AddDbContext<AppDBContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB1"))).
                 AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularProd", policy =>
    {
        policy.WithOrigins("https://blogapp-cze7fag5e5g6bka0.centralindia-01.azurewebsites.net")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddAuthorization();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddHttpClient<IFunctionInvoker, FunctionInvoker>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI();
    app.UseCors(options =>
                    options.WithOrigins("http://localhost:4200", "http://localhost:5173").
                    AllowAnyMethod().
                    AllowAnyHeader());
}
else
{
    app.UseExceptionHandler("/error");
    app.UseCors("AllowAngularProd");

}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DBSeeder.SeedRolesAsync(services);
    await DBSeeder.AddAdmin(services);
}


app.UseDefaultFiles();   // serve index.html
app.UseStaticFiles();    // serve Angular assets

app.UseRouting();        // enable routing
app.Use(async (context, next) =>
{
    if (context.Request.Method == HttpMethods.Options)
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
    }
    else
    {
        await next();
    }
});

app.UseAuthentication(); // validate JWT
app.UseAuthorization();  // enforce policies

app.MapControllers();    // m

app.Run();
