using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace IdentityDataProtectionApi.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int Id { get; set; }

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = null!;

    // Düz şifre değil—sadece hash saklıyoruz
    [Required]
    public string PasswordHash { get; set; } = null!;
}
