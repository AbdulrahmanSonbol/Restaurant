using Contracts.DTOs.UserDTO;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync (LoginDTO loginDTO);
        Task<Result<string>> RegisterAsync(RegisterDTO registerDTO);
        Task<Result<bool>> LogoutAsync(Guid userId);
        Task<Result<string>> GeneratePasswordResetTokenAsync(string email);
        Task<Result<bool>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<Result<bool>> ConfirmEmailAsync(string email, string token);
    }
}
