using System;

namespace web_backend.Models.DTOs
{
    public class UserDto
    {
        public string Id { get; }
        public string? Email { get; }
        public string Name { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset UpdatedAt { get; }

        public UserDto(string id, string? email, string name, DateTimeOffset createdAt, DateTimeOffset updatedAt)
        {
            Id = id;
            Email = email;
            Name = name;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
