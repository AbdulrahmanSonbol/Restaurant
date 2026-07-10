using AutoMapper;
using Contracts.DTOs.UserDTO;
using Domain.Entities.IdentitMyodule;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IDistributedCache _distributedCache;

        public AuthenticationService(
             UserManager<User> userManager,
             IConfiguration configuration,
             SignInManager<User> signInManager,
             IMapper mapper,
             IEmailService emailService,
             IDistributedCache distributedCache  

            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _distributedCache = distributedCache;
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

            return _mapper.Map<UserDTO>(User) with { Token = Token };
        }

        #endregion

        #region Register

        public async Task<Result<string>> RegisterAsync(RegisterDTO registerDTO)
        {
            var userExists = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (userExists is not null)
                return Error.InvalidCrendentials("Email is already registered.");

            var user = _mapper.Map<User>(registerDTO);

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Error.InvalidCrendentials(errors);
            }

            var otp = await GenerateAndSaveOtpAsync(user.Email!);

            var emailResult = await SendOtpEmailAsync(
                user.Email!,
                "Verification Code - Qa3da Restaurant",
                "Welcome to Qa3da",
                "Thank you for registering. Use the 4-digit verification code below to activate your account:",
                otp
            );

            if (!emailResult.IsSuccess)
            {
                var error = emailResult.Errors.First();
                return Error.InvalidCrendentials(error.Code, error.Description);
            }

            return Result<string>.Ok("Registration successful. Please check your email for the 4-digit verification code.");
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

            var otp = await GenerateAndSaveOtpAsync(email);

            var emailResult = await SendOtpEmailAsync(
                  user.Email!,
                  "Reset Password Code - Qa3da Restaurant",
                  "Reset Password Request",
                  "Use the 4-digit token below to reset your password:",
                  otp
              );    

            if (!emailResult.IsSuccess)
            {
                var error = emailResult.Errors.First();
                return Error.InvalidCrendentials(error.Code, error.Description);
            }

            return Result<string>.Ok("If the email exists, a reset code has been sent.");
        }

        public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            if (resetPasswordDTO.NewPassword != resetPasswordDTO.ConfirmNewPassword)
                return Error.InvalidCrendentials("Passwords do not match.");

            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user is null)
                return Error.InvalidCrendentials("Email not found.");

            var cacheKey = $"otp:{resetPasswordDTO.Email}";

            var cachedOtp = await _distributedCache.GetStringAsync(cacheKey);

            if (cachedOtp is null || cachedOtp != resetPasswordDTO.Token)
                return Error.InvalidCrendentials("Invalid or expired reset token.");

            var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();

            if (passwordValidator != null)
            {
                var validationResult = await passwordValidator.ValidateAsync(_userManager, user, resetPasswordDTO.NewPassword);
                if (!validationResult.Succeeded)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.Description));
                    return Error.InvalidCrendentials(errors);
                }
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, resetPasswordDTO.NewPassword);

            await _userManager.UpdateSecurityStampAsync(user);

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return Error.InvalidCrendentials("Error updating password.");

            await _distributedCache.RemoveAsync(cacheKey);

            return Result<bool>.Ok(true);
        }

        #endregion

        #region ConfirmEmail

        public async Task<Result<bool>> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.InvalidCrendentials("Email not found.");

            var cacheKey = $"otp:{email}";

            var cachedOtp = await _distributedCache.GetStringAsync(cacheKey);

            if (cachedOtp is null || cachedOtp != token)
            {
                return Error.InvalidCrendentials("Invalid or expired verification code.");
            }

            user.EmailConfirmed = true;

            await _userManager.UpdateSecurityStampAsync(user);

            await _userManager.UpdateAsync(user);

            await _distributedCache.RemoveAsync(cacheKey);

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
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty)
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

        private async Task<string> GenerateAndSaveOtpAsync(string email)
        {
            var random = new Random();
            var otpCode = random.Next(1000, 9999).ToString();

            var cacheKey = $"otp:{email}";

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) 
            };

            await _distributedCache.SetStringAsync(cacheKey, otpCode, cacheOptions);

            return otpCode;
        }

        private async Task<Result<bool>> SendOtpEmailAsync(string email, string subject, string title, string messageText, string otpCode)
        {
            var emailBody = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #eee; border-radius: 10px; text-align: center;'>
            <h2 style='color: #ff5722;'>{title}</h2>
            <p>{messageText}</p>
            <div style='background-color: #f4f4f4; padding: 15px 30px; margin: 20px auto; width: fit-content; font-size: 32px; font-weight: bold; letter-spacing: 8px; border-radius: 5px; color: #ff5722;'>
                {otpCode}
            </div>
            <p style='font-size: 12px; color: #aaa;'>This code is valid for 15 minutes.</p>
        </div>";

            try
            {
                await _emailService.SendEmailAsync(email, subject, emailBody);
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Error.InvalidCrendentials($"Failed to send email: {ex.Message}");
            }
        }

        #endregion

    }
}
