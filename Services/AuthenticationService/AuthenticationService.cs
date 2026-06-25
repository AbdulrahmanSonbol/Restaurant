using Contracts.DTOs.UserDTO;
using Domain.Entities.Identityodule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Shared.Result;
using System;
using System.Collections.Generic;
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
        public Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            throw new NotImplementedException();
        }
    }
}
