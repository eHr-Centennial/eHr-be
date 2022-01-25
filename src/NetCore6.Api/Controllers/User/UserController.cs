using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore6.Bl.DTOs.User;
using NetCore6.Services.Services.User;

namespace NetCore6.Api.Controllers.User
{
    [ApiController]
    [Route("api/User/")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("ToRegister", Name = "userRegistry")]
        public async Task<ActionResult<AuthenticateResponseDTO>> ToRegister([FromBody] UserModelDTO userModelDTO)
        {
            var userRegistry = await _userService.ToRegister(userModelDTO);

            if(userRegistry is null)
            {
                return BadRequest("userLoggedIn");
            }

            return userRegistry;
        }

        [HttpPost("Login", Name = "login")]
        public async Task<ActionResult<AuthenticateResponseDTO>> Login([FromBody] UserModelDTO userModelDTO)
        {
            var userLoggedIn = await _userService.Login(userModelDTO);

            if(userLoggedIn is null)
            {
                return BadRequest("Login Incorrect");
            }

            return userLoggedIn;

        }

        [HttpPut("ChangePassword", Name = "changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault(); 

            var passwordService = await _userService.ChangePassword(changePasswordDTO, emailClaim.Value);

            return passwordService;
        }
    }
}