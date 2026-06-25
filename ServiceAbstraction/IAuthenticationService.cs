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
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
    }
}
