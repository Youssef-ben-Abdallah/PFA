using NetDwhProject.Core.Entities.Oltp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDwhProject.Core.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Role> Roles { get; }
    IRepository<UserRole> UserRoles { get; }
    IRepository<Category> Categories { get; }
    IRepository<SubCategory> SubCategories { get; }
    IRepository<Product> Products { get; }
    IRepository<Customer> Customers { get; }
    IRepository<Order> Orders { get; }
    IRepository<OrderDetail> OrderDetails { get; }
    Task<int> CompleteAsync();
}