using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.UserDTO
{
    public record ResetPasswordDTO(
        string Email,
        string Token,
        string NewPassword,
        string ConfirmNewPassword
    );

}
