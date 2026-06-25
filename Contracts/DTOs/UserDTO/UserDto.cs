using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.UserDTO
{
    public record UserDTO(Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Role,
        string Token,
        string RefreshToken);
}
