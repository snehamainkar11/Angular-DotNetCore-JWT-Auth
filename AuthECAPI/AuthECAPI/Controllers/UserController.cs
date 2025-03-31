using AuthECAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthECAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("/api/signup")]
        public async Task<IActionResult> SignUp([FromBody] UserRegistrationModel userRegistrationModel)
        {
            AppUser user = new AppUser
            {
                UserName = userRegistrationModel.Email,
                Email = userRegistrationModel.Email,
                FullName = userRegistrationModel.FullName,
            };
            var result = await _userManager.CreateAsync(user, userRegistrationModel.Password);
            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
 
