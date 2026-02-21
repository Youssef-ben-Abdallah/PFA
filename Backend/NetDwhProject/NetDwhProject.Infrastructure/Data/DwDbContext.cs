using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetDwhProject.Core.Entities.Dwh;
using System.Reflection.Emit;

namespace NetDwhProject.Infrastructure.Data;

public class DwDbContext : DbContext
{
    public DwDbContext(DbContextOptions<DwDbContext> options) : base(options) { }

    public DbSet<SalesDashboardView> SalesDashboard => Set<SalesDashboardView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SalesDashboardView>()
            .ToView("vwSalesDashboard")
            .HasKey(v => v.SalesOrderDetailID);
    }
}
