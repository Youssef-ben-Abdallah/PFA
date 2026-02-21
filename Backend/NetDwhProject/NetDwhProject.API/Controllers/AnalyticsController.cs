using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDwhProject.Core.Interfaces;

namespace NetDwhProject.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsRepository _analytics;

    public AnalyticsController(IAnalyticsRepository analytics)
    {
        _analytics = analytics;
    }

    [HttpGet("kpis")]
    public async Task<IActionResult> GetKpis([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var totalSales = await _analytics.GetTotalSales(from, to);
        var totalOrders = await _analytics.GetTotalOrders(from, to);
        var totalUnits = await _analytics.GetTotalUnits(from, to);
        var grossProfit = await _analytics.GetGrossProfit(from, to);
        var aov = totalOrders > 0 ? totalSales / totalOrders : 0;
        var margin = totalSales > 0 ? grossProfit / totalSales : 0;

        return Ok(new { totalSales, totalOrders, totalUnits, grossProfit, aov, margin });
    }

    [HttpGet("sales-by-period")]
    public async Task<IActionResult> GetSalesByPeriod([FromQuery] string groupBy = "month", [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
    {
        var data = await _analytics.GetSalesByPeriod(groupBy, from, to);
        return Ok(data);
    }

    [HttpGet("top-products")]
    public async Task<IActionResult> GetTopProducts([FromQuery] int top = 10, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
    {
        var data = await _analytics.GetTopProducts(top, from, to);
        return Ok(data);
    }

    [HttpGet("sales-by-territory")]
    public async Task<IActionResult> GetSalesByTerritory([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
    {
        var data = await _analytics.GetSalesByTerritory(from, to);
        return Ok(data);
    }

    [HttpGet("online-offline-ratio")]
    public async Task<IActionResult> GetOnlineOfflineRatio([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
    {
        var (online, offline) = await _analytics.GetOnlineOfflineRatio(from, to);
        return Ok(new { online, offline });
    }

    [HttpGet("map-data")]
    public async Task<IActionResult> GetMapData([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
    {
        var data = await _analytics.GetMapData(from, to);
        return Ok(data);
    }
}