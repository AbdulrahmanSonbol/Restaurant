using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contracts.DTOs.UserDTO
{
    public record LoginDTO(
        string Email,
        string Password);
}
