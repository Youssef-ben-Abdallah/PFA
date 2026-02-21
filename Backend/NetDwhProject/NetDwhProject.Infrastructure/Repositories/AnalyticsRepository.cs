using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NetDwhProject.Core.Entities.Dwh;
using NetDwhProject.Core.Interfaces;
using NetDwhProject.Infrastructure.Data;

namespace NetDwhProject.Infrastructure.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly DwDbContext _context;

    public AnalyticsRepository(DwDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetTotalSales(DateTime? from, DateTime? to)
    {
        var query = _context.SalesDashboard.AsQueryable();
        if (from.HasValue) query = query.Where(x => x.OrderDate >= from);
        if (to.HasValue) query = query.Where(x => x.OrderDate <= to);
        return await query.SumAsync(x => x.LineTotal);
    }

    public async Task<int> GetTotalOrders(DateTime? from, DateTime? to)
    {
        var query = _context.SalesDashboard.AsQueryable();
        if (from.HasValue) query = query.Where(x => x.OrderDate >= from);
        if (to.HasValue) query = query.Where(x => x.OrderDate <= to);
        return await query.Select(x => x.SalesOrderID).Distinct().CountAsync();
    }

    public async Task<long> GetTotalUnits(DateTime? from, DateTime? to)
    {
        var query = _context.SalesDashboard.AsQueryable();
        if (from.HasValue) query = query.Where(x => x.OrderDate >= from);
        if (to.HasValue) query = query.Where(x => x.OrderDate <= to);
        return await query.SumAsync(x => x.OrderQty);
    }

    public async Task<decimal> GetGrossProfit(DateTime? from, DateTime? to)
    {
        var query = _context.SalesDashboard.AsQueryable();
        if (from.HasValue) query = query.Where(x => x.OrderDate >= from);
        if (to.HasValue) query = query.Where(x => x.OrderDate <= to);
        return await query.SumAsync(x => (x.UnitPrice - x.StandardCost) * x.OrderQty);
    }

    public async Task<IEnumerable<dynamic>> GetSalesByPeriod(string groupBy, DateTime? from, DateTime? to)
    {
        var query = _context.SalesDashboard.AsQueryable();
        if (from.HasValue) query = query.Where(x => x.OrderDate >= from);
        if (to.HasValue) query = query.Where(x => x.OrderDate <= to);

        if (groupBy.ToLower() == "month")
        {
            return await query
                .GroupBy(x => new { x.OrderDate.Year, x.OrderDate.Month })
                .Select(g => new { Period = $"{g.Key.Year}-{g.Key.Month:D2}", Total = g.Sum(x => x.LineTotal) })
                .OrderBy(g => g.Period)
                .ToListAsync();
        }
        // add other groupings (day, quarter, year) as needed
        return Enumerable.Empty<dynamic>();
    }

    public async Task<IEnumerable<dynamic>> GetTopProducts(int top, DateTime? from, DateTime? to)
    {
        var query = _context.SalesDashboard.AsQueryable();
        if (from.HasValue) query = query.Where(x => x.OrderDate >= from);
        if (to.HasValue) query = query.Where(x => x.OrderDate <= to);

        return await query
            .GroupBy(x => new { x.ProductID, x.ProductName })
            .Select(g => new { ProductName = g.Key.ProductName, TotalSales = g.Sum(x => x.LineTotal) })
            .OrderByDescending(g => g.TotalSales)
            .Take(top)
            .ToListAsync();
    }

    public async Task<IEnumerable<dynamic>> GetSalesByTerritory(DateTime? from, DateTime? to)
    {
        var query = _context.SalesDashboard.AsQueryable();
        if (from.HasValue) query = query.Where(x => x.OrderDate >= from);
        if (to.HasValue) query = query.Where(x => x.OrderDate <= to);

        return await query
            .GroupBy(x => new { x.TerritoryID, x.TerritoryName })
            .Select(g => new { Territory = g.Key.TerritoryName, Total = g.Sum(x => x.LineTotal) })
            .ToListAsync();
    }

    public async Task<(decimal online, decimal offline)> GetOnlineOfflineRatio(DateTime? from, DateTime? to)
    {
        var query = _context.SalesDashboard.AsQueryable();
        if (from.HasValue) query = query.Where(x => x.OrderDate >= from);
        if (to.HasValue) query = query.Where(x => x.OrderDate <= to);

        var online = await query.Where(x => x.OnlineOrderFlag).SumAsync(x => x.LineTotal);
        var offline = await query.Where(x => !x.OnlineOrderFlag).SumAsync(x => x.LineTotal);
        return (online, offline);
    }

    public async Task<IEnumerable<dynamic>> GetMapData(DateTime? from, DateTime? to)
    {
        var query = _context.SalesDashboard.AsQueryable();
        if (from.HasValue) query = query.Where(x => x.OrderDate >= from);
        if (to.HasValue) query = query.Where(x => x.OrderDate <= to);

        return await query
            .GroupBy(x => new { x.CountryRegionCode, x.TerritoryName })
            .Select(g => new { Country = g.Key.CountryRegionCode, Territory = g.Key.TerritoryName, Total = g.Sum(x => x.LineTotal) })
            .ToListAsync();
    }
}
