import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Filter } from '../../../core/services/filter.service';

export interface KpiData {
  totalSales: number;
  totalOrders: number;
  totalUnits: number;
  grossProfit: number;
  aov: number;
  margin: number;
}

@Injectable({ providedIn: 'root' })
export class AnalyticsService {
  private api = 'https://localhost:5125/api/analytics';

  constructor(private http: HttpClient) {}

  getKpis(filters: Filter): Observable<KpiData> {
    const params = this.buildParams(filters);
    return this.http.get<KpiData>(`${this.api}/kpis`, { params });
  }

  getSalesByPeriod(groupBy: string, filters: Filter): Observable<any[]> {
    let params = this.buildParams(filters).set('groupBy', groupBy);
    return this.http.get<any[]>(`${this.api}/sales-by-period`, { params });
  }

  getTopProducts(top: number, filters: Filter): Observable<any[]> {
    let params = this.buildParams(filters).set('top', top);
    return this.http.get<any[]>(`${this.api}/top-products`, { params });
  }

  getSalesByTerritory(filters: Filter): Observable<any[]> {
    const params = this.buildParams(filters);
    return this.http.get<any[]>(`${this.api}/sales-by-territory`, { params });
  }

  getOnlineOfflineRatio(filters: Filter): Observable<{ online: number; offline: number }> {
    const params = this.buildParams(filters);
    return this.http.get<{ online: number; offline: number }>(`${this.api}/online-offline-ratio`, { params });
  }

  getMapData(filters: Filter): Observable<any[]> {
    const params = this.buildParams(filters);
    return this.http.get<any[]>(`${this.api}/map-data`, { params });
  }

  private buildParams(filters: Filter): HttpParams {
    let params = new HttpParams();
    if (filters.from) params = params.set('from', filters.from);
    if (filters.to) params = params.set('to', filters.to);
    if (filters.territoryId) params = params.set('territoryId', filters.territoryId);
    return params;
  }

  // Add to AnalyticsService

  getSalesTrendsDaily(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/sales-trends/daily`, { params: this.buildParams(filters) });
  }

  getSalesTrendsMonthly(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/sales-trends/monthly`, { params: this.buildParams(filters) });
  }

  getCumulativeSales(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/sales-trends/cumulative`, { params: this.buildParams(filters) });
  }

  getYoYComparison(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/sales-trends/yoy`, { params: this.buildParams(filters) });
  }

  getProductCategorySales(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/product-performance/category-sales`, { params: this.buildParams(filters) });
  }

  getTopProductsByProfit(top: number, filters: Filter): Observable<any[]> {
    let params = this.buildParams(filters).set('top', top);
    return this.http.get<any[]>(`${this.api}/product-performance/top-profit`, { params });
  }

  getProductScatterData(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/product-performance/scatter`, { params: this.buildParams(filters) });
  }

  getProductMatrix(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/product-performance/matrix`, { params: this.buildParams(filters) });
  }

  getTopCustomers(top: number, filters: Filter): Observable<any[]> {
    let params = this.buildParams(filters).set('top', top);
    return this.http.get<any[]>(`${this.api}/customer-analysis/top`, { params });
  }

  getOrderValueDistribution(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/customer-analysis/order-distribution`, { params: this.buildParams(filters) });
  }

  getNewCustomersPerMonth(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/customer-analysis/new-customers`, { params: this.buildParams(filters) });
  }

  getCustomerTerritoryMatrix(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/customer-analysis/territory-matrix`, { params: this.buildParams(filters) });
  }

  getSalespersonSales(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/salesperson/sales`, { params: this.buildParams(filters) });
  }

  getSalespersonProfit(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/salesperson/profit`, { params: this.buildParams(filters) });
  }

  getSalespersonQuota(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/salesperson/quota`, { params: this.buildParams(filters) });
  }

  getSalespersonTerritoryMap(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/salesperson/territory-map`, { params: this.buildParams(filters) });
  }

  getSalesByShipMethod(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/shipping/sales-by-method`, { params: this.buildParams(filters) });
  }

  getAvgDeliveryDays(filters: Filter): Observable<{ avgDeliveryDays: number }> {
    return this.http.get<{ avgDeliveryDays: number }>(`${this.api}/shipping/avg-delivery`, { params: this.buildParams(filters) });
  }

  getLateShipments(filters: Filter): Observable<{ lateCount: number; totalLines: number; latePercent: number }> {
    return this.http.get<any>(`${this.api}/shipping/late-shipments`, { params: this.buildParams(filters) });
  }

  getCurrencyRates(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/currency/rates`, { params: this.buildParams(filters) });
  }

  getSalesByCurrency(filters: Filter): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/currency/sales`, { params: this.buildParams(filters) });
  }
}