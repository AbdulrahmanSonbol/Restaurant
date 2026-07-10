using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.UserDTO
{
    public record UserDTO
    {
        public UserDTO() { }

        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;

        public string Role { get; init; } = string.Empty;
    }


}
