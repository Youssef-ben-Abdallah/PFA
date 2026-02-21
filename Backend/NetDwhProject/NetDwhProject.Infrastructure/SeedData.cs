using System.Linq;
using BCrypt.Net;
using NetDwhProject.Core.Entities.Oltp;
using NetDwhProject.Infrastructure.Data;

namespace NetDwhProject.Infrastructure;

public static class SeedData
{
    public static void Initialize(OltpDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "User" }
            );
            context.SaveChanges();
        }

        if (!context.Users.Any(u => u.Username == "admin"))
        {
            var admin = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!", workFactor: 12)
            };
            context.Users.Add(admin);
            context.SaveChanges();

            var adminRole = context.Roles.First(r => r.Name == "Admin");
            context.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRole.Id });
            context.SaveChanges();
        }
    }
}