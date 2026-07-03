using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.UserDTO
{
    public record ConfirmEmailDTO(
        string Email,
        string Token
    );
}
