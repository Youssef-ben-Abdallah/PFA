using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDwhProject.Core.Entities.Oltp;
using NetDwhProject.Core.Interfaces;
using NetDwhProject.Infrastructure.Data;
using System.Data;

namespace NetDwhProject.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly OltpDbContext _context;
    private IRepository<User>? _users;
    private IRepository<Role>? _roles;
    private IRepository<UserRole>? _userRoles;
    private IRepository<Category>? _categories;
    private IRepository<SubCategory>? _subCategories;
    private IRepository<Product>? _products;
    private IRepository<Customer>? _customers;
    private IRepository<Order>? _orders;
    private IRepository<OrderDetail>? _orderDetails;

    public UnitOfWork(OltpDbContext context)
    {
        _context = context;
    }

    public IRepository<User> Users => _users ??= new Repository<User>(_context);
    public IRepository<Role> Roles => _roles ??= new Repository<Role>(_context);
    public IRepository<UserRole> UserRoles => _userRoles ??= new Repository<UserRole>(_context);
    public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);
    public IRepository<SubCategory> SubCategories => _subCategories ??= new Repository<SubCategory>(_context);
    public IRepository<Product> Products => _products ??= new Repository<Product>(_context);
    public IRepository<Customer> Customers => _customers ??= new Repository<Customer>(_context);
    public IRepository<Order> Orders => _orders ??= new Repository<Order>(_context);
    public IRepository<OrderDetail> OrderDetails => _orderDetails ??= new Repository<OrderDetail>(_context);

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
