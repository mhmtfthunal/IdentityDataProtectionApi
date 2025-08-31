using IdentityDataProtectionApi.Data;
using IdentityDataProtectionApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- EF Core ---
var cs = builder.Configuration.GetConnectionString("SqlServer")
         ?? "Server=(localdb)\\MSSQLLocalDB;Database=IdentityDataProtectionDb;Trusted_Connection=True;TrustServerCertificate=True";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(cs));

// API & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Data Protection (ileride token vb. için)
builder.Services.AddDataProtection();

var app = builder.Build();

// DB migrate + basit seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Users.Any())
    {
        var hasher = new PasswordHasher<User>();
        var u1 = new User { Email = "user@example.com" };
        u1.PasswordHash = hasher.HashPassword(u1, "P@ssw0rd!");
        var u2 = new User { Email = "admin@example.com" };
        u2.PasswordHash = hasher.HashPassword(u2, "Admin123!");
        db.AddRange(u1, u2);
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
