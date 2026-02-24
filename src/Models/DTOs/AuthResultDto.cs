using System;

namespace web_backend.Models.DTOs
{
    public class AuthResultDto
    {
        public string Token { get; }
        public DateTimeOffset ExpiresAt { get; }
        public UserDto User { get; }

        public AuthResultDto(string token, DateTimeOffset expiresAt, UserDto user)
        {
            Token = token;
            ExpiresAt = expiresAt;
            User = user;
        }
    }
}
