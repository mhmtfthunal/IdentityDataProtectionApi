using System.ComponentModel.DataAnnotations;

namespace IdentityDataProtectionApi.DTOs;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = null!;
}

public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}

public class UserResponse
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
}
