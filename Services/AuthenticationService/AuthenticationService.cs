using Contracts.DTOs.UserDTO;
using Domain.Entities.IdentitMyodule;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(
             UserManager<User> userManager,
             IConfiguration configuration,
             SignInManager<User> signInManager

            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        #region Login

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User is null)
                return Error.InvalidCrendentials("Invalid email");

            if (!await _userManager.IsEmailConfirmedAsync(User))
                return Error.InvalidCrendentials("Your email is not verified yet. Please check your inbox.");
            
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);
            if (!IsPasswordValid)
                return Error.InvalidCrendentials("Invalid password");

            var Token = await CreateTokenAsync(User);

            return new UserDTO
                (
                User.Id,
                User.FirstName,
                User.LastName,
                User.Email!,
                User.PhoneNumber ?? string.Empty,
                User.Role.ToString(),
                Token, User.RefreshToken ?? string.Empty
                );

        }


        #endregion

        #region Register

        public async Task<Result<string>> RegisterAsync(RegisterDTO registerDTO)
        {
            var userExists = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (userExists != null)
                return Error.InvalidCrendentials("Email is already registered.");


            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                Role = UserRole.User 
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Error.InvalidCrendentials(errors);
            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var validToken = Microsoft.AspNetCore.WebUtilities.WebEncoders.
                Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(confirmationToken));

            return Result<string>.Ok(validToken);
        }

        #endregion

        #region Logout

        public async Task<Result<bool>> LogoutAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Error.InvalidCrendentials("User not found");

            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;


            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Error.InvalidCrendentials("An error occurred during logout.");
            

            await _signInManager.SignOutAsync();

            return Result<bool>.Ok(true);

        }

        #endregion

        #region Password Reset

        public async Task<Result<string>> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.InvalidCrendentials("Email not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var validToken = Microsoft.AspNetCore.WebUtilities.WebEncoders
                .Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(token));

            return Result<string>.Ok(validToken);
        }

        public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            if (resetPasswordDTO.NewPassword != resetPasswordDTO.ConfirmNewPassword)
                return Error.InvalidCrendentials("Passwords do not match.");

            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user is null)
                return Error.InvalidCrendentials("Email not found.");

            var token = resetPasswordDTO.Token;
            try
            {
                var decodedTokenBytes = Microsoft.AspNetCore.WebUtilities.WebEncoders
                    .Base64UrlDecode(token);

                token = System.Text.Encoding.UTF8.GetString(decodedTokenBytes);
            }
            catch
            {
                return Error.InvalidCrendentials("Invalid token format.");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDTO.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Error.InvalidCrendentials(errors);
            }

            return Result<bool>.Ok(true);
        }

        #endregion

        #region ConfirmEmail

        public async Task<Result<bool>> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.InvalidCrendentials("Email not found.");

            try
            {
                var decodedTokenBytes = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(token);
                token = System.Text.Encoding.UTF8.GetString(decodedTokenBytes);
            }
            catch
            {
                return Error.InvalidCrendentials("Invalid confirmation token format.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return Error.InvalidCrendentials("Invalid or expired confirmation token.");
            }

            return Result<bool>.Ok(true);
        }

        #endregion

        #region Helper

        private async Task<string> CreateTokenAsync(User user)
        {
            // Token [Issuer, Audience, Claims, Expires, SigningCredentials ]

            #region Claims

            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var Role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, Role));
            }

            #endregion

            #region Credentials

            var SecretKey = _configuration["JWTOptions:SecretKey"];

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));

            var Cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            #endregion

            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: Claims,
                signingCredentials: Cred
            );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        #endregion

    }
}
