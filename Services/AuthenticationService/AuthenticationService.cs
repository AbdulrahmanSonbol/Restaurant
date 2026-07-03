using Contracts.DTOs.UserDTO;
using Domain.Entities.IdentityModule;
using Domain.Entities.Identityodule;
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

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
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

            var token = await CreateTokenAsync(user);

            return new UserDTO(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                user.Role.ToString(),
                token,
                user.RefreshToken ?? string.Empty
            );
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
            };

            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var Role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, Role));
            }

            #endregion

            #region Credentials

            var SecretKey = _configuration["JWTOptions:SecretKey"];

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

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
