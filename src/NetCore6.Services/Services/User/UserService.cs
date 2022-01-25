using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCore6.Bl.DTOs.User;
using NetCore6.Core.Abstract;
using NetCore6.Core.Settings;

namespace NetCore6.Services.Services.User
{
    public interface IUserService
    {
        Task<AuthenticateResponseDTO> ToRegister(UserModelDTO userModelDTO);
        Task<AuthenticateResponseDTO> Login(UserModelDTO userModelDTO);
        Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO, string emailClaim);
    }

    public class UserService: IUserService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IValidator<UserModelDTO> _validator;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserService(
            IValidator<UserModelDTO> validator,
            IOptions<JwtSettings> jwtSettings,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _validator = validator;
            _signInManager = signInManager;
        }

        public async Task<AuthenticateResponseDTO> ToRegister(UserModelDTO userModelDTO)
        {
            var validationResult = _validator.Validate(userModelDTO);

            if(validationResult.IsValid is false)
                return null;

            var user = new IdentityUser
            {
                UserName = userModelDTO.Email,
                Email = userModelDTO.Email
            };

            var userRegistry = await _userManager.CreateAsync(user, userModelDTO.Password);

            if(userRegistry.Succeeded)
                return await GenerateToken(userModelDTO);
            else
                return null;
        }

        public async Task<AuthenticateResponseDTO> Login(UserModelDTO userModelDTO)
        {
            var userLoggedIn = await _signInManager.PasswordSignInAsync
            (
                userModelDTO.Email, 
                userModelDTO.Password, 
                isPersistent: false, lockoutOnFailure: false
            );

            if(userLoggedIn.Succeeded)
                return await GenerateToken(userModelDTO);
            else
                return null;
        }

        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO, string emailClaim)
        {
            var user = await _userManager.FindByEmailAsync(emailClaim);

            if(user == null)
                return new NotFoundResult();
            
            if(string.Compare(changePasswordDTO.NewPassword, changePasswordDTO.ConfirmNewPassword) != 0)
                return new BadRequestResult();

            var changePass = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);

            if(changePass.Succeeded)
                return new OkResult();
            else
                return new BadRequestResult();
        }

        public async Task<AuthenticateResponseDTO> GenerateToken(UserModelDTO userModelDTO)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userModelDTO.Email)
            };

            var user = await _userManager.FindByEmailAsync(userModelDTO.Email);
            var claimDB = await _userManager.GetClaimsAsync(user);

            claims.AddRange(claimDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes);

            var securityToken = new JwtSecurityToken
            (
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new AuthenticateResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };
        }


    }
}