using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contracts.DTOs.UserDTO
{
    public record RegisterDTO(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Password,
        string ConfirmPassword);
}
