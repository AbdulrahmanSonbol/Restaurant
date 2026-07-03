using Contracts.DTOs.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        public async Task<ActionResult<string>> Register(RegisterDTO registerDTO)
        {
            var Result = await _authenticationService.RegisterAsync(registerDTO);
            return HandleResult(Result);
        }

        #endregion

        #region logout

        [Authorize] 
        [HttpPost("Logout")]
        public async Task<ActionResult<bool>> Logout()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return BadRequest("Invalid user identity.");
            }

            var Result = await _authenticationService.LogoutAsync(userId);
          
            return HandleResult(Result);
        }

        #endregion

        #region Password Reset

        [HttpPost("Forget-Password")]
        public async Task<ActionResult<string>> ForgetPassword([FromBody] ForgetPasswordDTO dto)
        {
            var result = await _authenticationService.GeneratePasswordResetTokenAsync(dto.Email);
            return HandleResult(result);
        }

        [HttpPost("Reset-Password")]
        public async Task<ActionResult<bool>> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var result = await _authenticationService.ResetPasswordAsync(resetPasswordDTO);

            return HandleResult(result);
        }

        #endregion

        #region Confirm Email

        [HttpPost("Confirm-Email")]
        public async Task<ActionResult<bool>> ConfirmEmail([FromBody] ConfirmEmailDTO dto)
        {
            var result = await _authenticationService.ConfirmEmailAsync(dto.Email, dto.Token);
            return HandleResult(result);
        }

        #endregion

    }
}
