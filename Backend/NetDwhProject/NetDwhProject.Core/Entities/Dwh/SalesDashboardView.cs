using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDwhProject.Core.Entities.Dwh;

public class SalesDashboardView
{
    public int SalesOrderDetailID { get; set; }
    public string SalesOrderID { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime ShipDate { get; set; }
    public bool OnlineOrderFlag { get; set; }
    public int CustomerID { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string SubCategoryName { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal StandardCost { get; set; }
    public int? SalesPersonID { get; set; }
    public string? SalesPersonName { get; set; }
    public decimal? SalesQuota { get; set; }
    public decimal? CommissionPct { get; set; }
    public int TerritoryID { get; set; }
    public string TerritoryName { get; set; } = string.Empty;
    public string CountryRegionCode { get; set; } = string.Empty;
    public string TerritoryGroup { get; set; } = string.Empty;
    public int ShipMethodID { get; set; }
    public string ShipMethodName { get; set; } = string.Empty;
    public int OrderQty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal UnitPriceDiscount { get; set; }
    public decimal LineTotal { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TaxAmt { get; set; }
    public decimal Freight { get; set; }
    public decimal TotalDue { get; set; }
}
