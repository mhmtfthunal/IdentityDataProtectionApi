using Microsoft.EntityFrameworkCore;
using IdentityDataProtectionApi.Models;

namespace IdentityDataProtectionApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}
