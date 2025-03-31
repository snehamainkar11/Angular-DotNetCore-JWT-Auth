using AuthECAPI.Extensions;
using AuthECAPI.Models;
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
});

//services from identity core
builder.Services.InjectDBContext(builder.Configuration).
                 AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ConfigCORS(builder.Configuration).
    AddIdentityAuthMiddleware();

app.MapControllers();

app.MapGroup("/api").MapIdentityApi<AppUser>();

//app.MapPost("/api/signup", async (
//    UserManager<AppUser> userManager,
//    [FromBody] UserRegistrationModel userRegistrationModel
//    ) =>
//{
//    AppUser user = new AppUser
//    {
//        UserName = userRegistrationModel.Email,
//        Email = userRegistrationModel.Email,
//        FullName = userRegistrationModel.FullName,
//    };
//    var result = await userManager.CreateAsync(user, userRegistrationModel.Password);
//    if (result.Succeeded) 
//        return Results.Ok(result);
//    else 
//        return Results.BadRequest(result);
//});

//app.MapPost("/api/signin", async (
//    UserManager<AppUser> userManager,
//    [FromBody] LoginModel loginModel
//    ) =>
//{
//    var user = await userManager.FindByEmailAsync(loginModel.Email);
//    if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
//    {
//        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!));
//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(new Claim[]
//            {
//                new Claim("UserID", user.Id.ToString()),
//            }),
//            Expires = DateTime.UtcNow.AddMinutes(10),
//            SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
//        };

//        var tokenHandler = new JwtSecurityTokenHandler();
//        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
//        var token = tokenHandler.WriteToken(securityToken);

//        return Results.Ok(new { token });
//    }
//    else
//    {
//        return Results.BadRequest(new { message = "Username or password is incorrect" });
//    }
//});

app.Run();
