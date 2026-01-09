using AuthECAPI.DTO;
using AuthECAPI.FunctionTriggers;
using AuthECAPI.Models;
using AuthECAPI.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthECAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGoogleAuthService _googleAuthService;

        public AuthController(ITokenService tokenService, UserManager<AppUser> userManager, IGoogleAuthService googleAuthService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _userManager = userManager;
            _googleAuthService = googleAuthService;
        }

        [AllowAnonymous]
        [HttpPost("/api/signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.EmailId);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
                return Unauthorized(new { message = "Username or password is incorrect" });

            var accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken(user.Id);
            foreach (var token in user.RefreshTokens ?? new List<RefreshToken>())
            {
                token.IsRevoked = true;
            }

            user.RefreshTokens ??= new List<RefreshToken>();
            user.RefreshTokens.Add(refreshToken);

            await _userManager.UpdateAsync(user);
            return Ok(new LoginResponseModel
            {
                UserId = user.Id,
                Token = accessToken.AccessToken,
                RefreshToken = refreshToken.Token,
                TokenExpiresIn = (int)(accessToken.Expires - DateTime.UtcNow).TotalSeconds,
                Succeeded = true
            });
        }


        [AllowAnonymous]
        [HttpPost("/api/signup")]
        public async Task<IActionResult> SignUp([FromBody] UserRegistrationModel userRegistrationModel, [FromServices] IFunctionInvoker functionInvoker)
        {
            var existingUser = await _userManager.FindByEmailAsync(userRegistrationModel.EmailId);
            if (existingUser != null)
            {
                return BadRequest(new
                {
                    message = "Email is already registered"
                });
            }
            AppUser user = new AppUser
            {
                UserName = userRegistrationModel.EmailId,
                Email = userRegistrationModel.EmailId,
                FirstName = userRegistrationModel.FirstName,
                LastName = userRegistrationModel.LastName,
                PhoneNumber = userRegistrationModel.MobileNo,
            };
            var result = await _userManager.CreateAsync(user, userRegistrationModel.Password);
            await _userManager.AddToRoleAsync(user, "User");

            if (result.Succeeded)
            {
                await functionInvoker.TriggerWelcomeEmail(user.Email, user.FirstName);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }

        [AllowAnonymous]
        [HttpPost("/api/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Refresh token is required");

            // Find user who owns this refresh token
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u =>
                    u.RefreshTokens.Any(rt =>
                        rt.Token == request.RefreshToken &&
                        !rt.IsRevoked &&
                        rt.Expiry > DateTime.UtcNow));

            if (user == null)
                return Unauthorized("Invalid or expired refresh token");

            var oldToken = user.RefreshTokens
                .First(rt => rt.Token == request.RefreshToken);

            // Revoke old refresh token
            oldToken.IsRevoked = true;

            // Generate new tokens
            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = await _tokenService.GenerateRefreshToken(user.Id);

            user.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = newAccessToken.AccessToken,
                RefreshToken = newRefreshToken.Token,
                TokenExpiresIn = (int)(newAccessToken.Expires - DateTime.UtcNow).TotalSeconds
            });
        }
        [HttpPost("/api/signin-google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            var user = await _googleAuthService
                .AuthenticateGoogleUserAsync(dto.IdToken);

            var accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = await  _tokenService.GenerateRefreshToken(user.Id);

            user.RefreshTokens ??= new List<RefreshToken>();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token = accessToken.AccessToken,
                refreshToken = refreshToken.Token
            });
        }
    }
}

