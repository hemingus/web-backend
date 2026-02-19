using System.ComponentModel.DataAnnotations;

public class User
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
   
    public string? Email { get; private set; }
    public string Name { get; private set; }
    public string PasswordHash { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset? LastLoginAt { get; private set; }

    public User(string? email, string name, string passwordHash)
    {
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateName(string name)
    {
        Name = name;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RegisterLogin()
    {
        LastLoginAt = DateTimeOffset.UtcNow;
    }

    private User() { } // Required for EF Core
}
