import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService } from '../../services/analytics.service';
import { FilterService, Filter } from '../../../../core/services/filter.service';
import { ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-customer-analysis',
  templateUrl: './customer-analysis.component.html',
  styleUrls: ['./customer-analysis.component.css']
})
export class CustomerAnalysisComponent implements OnInit, OnDestroy {
  // Raw data
  topCustomers: any[] = [];
  orderDistribution: any[] = [];
  territoryMatrix: any[] = [];

  // Chart data
  topCustomersChartData: ChartConfiguration['data'] = { datasets: [] };
  orderDistributionChartData: ChartConfiguration['data'] = { datasets: [] };
  newCustomersChartData: ChartConfiguration['data'] = { datasets: [] };

  private filterSub: Subscription;

  constructor(
    public analytics: AnalyticsService,
    public filterService: FilterService
  ) {
    this.filterSub = this.filterService.filters$.subscribe(filters => {
      this.loadData(filters);
    });
  }

  ngOnInit(): void {
    this.loadData({});
  }

  loadData(filters: Filter): void {
    // Top customers
    this.analytics.getTopCustomers(20, filters).subscribe(data => {
      this.topCustomers = data;
      this.topCustomersChartData = {
        labels: data.map((c: any) => c.customerName),
        datasets: [{ data: data.map((c: any) => c.totalSales), label: 'Total Sales' }]
      };
    });

    // Order value distribution
    this.analytics.getOrderValueDistribution(filters).subscribe(data => {
      this.orderDistribution = data;
      this.orderDistributionChartData = {
        labels: data.map((d: any) => d.range),
        datasets: [{ data: data.map((d: any) => d.count), label: 'Order Count' }]
      };
    });

    // New customers per month
    this.analytics.getNewCustomersPerMonth(filters).subscribe(data => {
      this.newCustomersChartData = {
        labels: data.map((d: any) => d.period),
        datasets: [{ data: data.map((d: any) => d.newCustomers), label: 'New Customers' }]
      };
    });

    // Customer territory matrix
    this.analytics.getCustomerTerritoryMatrix(filters).subscribe(data => {
      this.territoryMatrix = data;
    });
  }

  ngOnDestroy(): void {
    this.filterSub.unsubscribe();
  }
}