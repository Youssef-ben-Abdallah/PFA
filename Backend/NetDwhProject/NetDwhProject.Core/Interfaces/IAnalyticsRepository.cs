using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDwhProject.Core.Entities.Dwh;

namespace NetDwhProject.Core.Interfaces;

public interface IAnalyticsRepository
{
    Task<decimal> GetTotalSales(DateTime? from, DateTime? to);
    Task<int> GetTotalOrders(DateTime? from, DateTime? to);
    Task<long> GetTotalUnits(DateTime? from, DateTime? to);
    Task<decimal> GetGrossProfit(DateTime? from, DateTime? to);
    Task<IEnumerable<dynamic>> GetSalesByPeriod(string groupBy, DateTime? from, DateTime? to);
    Task<IEnumerable<dynamic>> GetTopProducts(int top, DateTime? from, DateTime? to);
    Task<IEnumerable<dynamic>> GetSalesByTerritory(DateTime? from, DateTime? to);
    Task<(decimal online, decimal offline)> GetOnlineOfflineRatio(DateTime? from, DateTime? to);
    Task<IEnumerable<dynamic>> GetMapData(DateTime? from, DateTime? to);
    // Additional methods for other pages ...
}
