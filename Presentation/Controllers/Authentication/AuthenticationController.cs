using Contracts.DTOs.UserDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Controllers.Authentication
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
        }


        #region Login

        // Login
        // POST: BaseUrl/api/Authentication/login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var Result = await _authenticationService.LoginAsync(loginDTO);
            return HandleResult(Result);
        }

        #endregion


        #region Register

        // Register
        // POST: BaseUrl/api/Authentication/register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var Result = await _authenticationService.RegisterAsync(registerDTO);
            return HandleResult(Result);
        }

        #endregion


    }
}
