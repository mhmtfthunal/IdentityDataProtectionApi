using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IdentityDataProtectionApi.Data;
using IdentityDataProtectionApi.DTOs;
using IdentityDataProtectionApi.Models;

namespace IdentityDataProtectionApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthController(AppDbContext db) => _db = db;

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest req)
    {
        var exists = await _db.Users.AnyAsync(u => u.Email == req.Email);
        if (exists) return Conflict("Email already exists.");

        var user = new User { Email = req.Email };
        user.PasswordHash = _hasher.HashPassword(user, req.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id },
            new UserResponse { Id = user.Id, Email = user.Email });
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest req)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == req.Email);
        if (user is null) return Unauthorized("Invalid credentials.");

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Invalid credentials.");

        return new UserResponse { Id = user.Id, Email = user.Email };
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetUser(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();
        return new UserResponse { Id = user.Id, Email = user.Email };
    }
}
