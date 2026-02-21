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

        var adminRoleId = context.Roles.First(r => r.Name == "Admin").Id;
        var userRoleId = context.Roles.First(r => r.Name == "User").Id;

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

            context.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRoleId });
            context.SaveChanges();
        }

        if (!context.Users.Any(u => u.Username == "demo.user"))
        {
            var demoUser = new User
            {
                Username = "demo.user",
                Email = "demo.user@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!", workFactor: 12)
            };
            context.Users.Add(demoUser);
            context.SaveChanges();

            context.UserRoles.Add(new UserRole { UserId = demoUser.Id, RoleId = userRoleId });
            context.SaveChanges();
        }

        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Electronics", Description = "Devices and accessories" },
                new() { Name = "Home & Kitchen", Description = "Household and kitchen essentials" },
                new() { Name = "Sports", Description = "Fitness and outdoor products" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            var subCategories = new List<SubCategory>
            {
                new() { Name = "Laptops", CategoryId = categories[0].Id },
                new() { Name = "Accessories", CategoryId = categories[0].Id },
                new() { Name = "Cookware", CategoryId = categories[1].Id },
                new() { Name = "Furniture", CategoryId = categories[1].Id },
                new() { Name = "Fitness", CategoryId = categories[2].Id }
            };

            context.SubCategories.AddRange(subCategories);
            context.SaveChanges();

            var products = new List<Product>
            {
                new() { Name = "Ultrabook Pro 14", Description = "Lightweight productivity laptop", Price = 1299.99m, Stock = 18, SubCategoryId = subCategories[0].Id },
                new() { Name = "Gaming Laptop X", Description = "High-performance gaming laptop", Price = 1699.50m, Stock = 10, SubCategoryId = subCategories[0].Id },
                new() { Name = "Wireless Mouse", Description = "Ergonomic Bluetooth mouse", Price = 34.99m, Stock = 120, SubCategoryId = subCategories[1].Id },
                new() { Name = "Stainless Pan Set", Description = "Non-stick cookware set", Price = 189.00m, Stock = 40, SubCategoryId = subCategories[2].Id },
                new() { Name = "Standing Desk", Description = "Adjustable height desk", Price = 449.90m, Stock = 15, SubCategoryId = subCategories[3].Id },
                new() { Name = "Yoga Mat", Description = "Anti-slip fitness mat", Price = 24.75m, Stock = 80, SubCategoryId = subCategories[4].Id }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        if (!context.Customers.Any())
        {
            var customers = new List<Customer>
            {
                new() { FirstName = "Ava", LastName = "Johnson", Email = "ava.johnson@example.com", Phone = "+1-202-555-0101" },
                new() { FirstName = "Noah", LastName = "Williams", Email = "noah.williams@example.com", Phone = "+1-202-555-0102" },
                new() { FirstName = "Liam", LastName = "Brown", Email = "liam.brown@example.com", Phone = "+1-202-555-0103" }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        if (!context.Orders.Any())
        {
            var customers = context.Customers.Take(3).ToList();
            var products = context.Products.Take(6).ToList();
            if (customers.Count < 3 || products.Count < 6)
            {
                return;
            }

            var orders = new List<Order>
            {
                new() { CustomerId = customers[0].Id, OrderDate = DateTime.UtcNow.AddDays(-7), Status = "Completed" },
                new() { CustomerId = customers[1].Id, OrderDate = DateTime.UtcNow.AddDays(-4), Status = "Completed" },
                new() { CustomerId = customers[2].Id, OrderDate = DateTime.UtcNow.AddDays(-2), Status = "Pending" }
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();

            var orderDetails = new List<OrderDetail>
            {
                new() { OrderId = orders[0].Id, ProductId = products[0].Id, Quantity = 1, UnitPrice = products[0].Price },
                new() { OrderId = orders[0].Id, ProductId = products[2].Id, Quantity = 2, UnitPrice = products[2].Price },
                new() { OrderId = orders[1].Id, ProductId = products[3].Id, Quantity = 1, UnitPrice = products[3].Price },
                new() { OrderId = orders[1].Id, ProductId = products[5].Id, Quantity = 3, UnitPrice = products[5].Price },
                new() { OrderId = orders[2].Id, ProductId = products[1].Id, Quantity = 1, UnitPrice = products[1].Price },
                new() { OrderId = orders[2].Id, ProductId = products[4].Id, Quantity = 1, UnitPrice = products[4].Price }
            };

            context.OrderDetails.AddRange(orderDetails);
            context.SaveChanges();

            foreach (var order in orders)
            {
                var total = context.OrderDetails
                    .Where(od => od.OrderId == order.Id)
                    .Sum(od => od.Quantity * od.UnitPrice);
                order.TotalAmount = total;
            }

            context.SaveChanges();
        }
    }
}
