import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService } from '../../services/analytics.service';
import { FilterService, Filter } from '../../../../core/services/filter.service';
import { ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-salesperson-performance',
  templateUrl: './salesperson-performance.component.html',
  styleUrls: ['./salesperson-performance.component.css']
})
export class SalespersonPerformanceComponent implements OnInit, OnDestroy {
  // Raw data
  salesData: any[] = [];
  profitData: any[] = [];
  quotaData: any[] = [];
  territoryMap: any[] = [];

  // Chart data
  salesChartData: ChartConfiguration['data'] = { datasets: [] };
  profitChartData: ChartConfiguration['data'] = { datasets: [] };

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
    // Sales by salesperson
    this.analytics.getSalespersonSales(filters).subscribe(data => {
      this.salesData = data;
      this.salesChartData = {
        labels: data.map((s: any) => s.salesPersonName),
        datasets: [{ data: data.map((s: any) => s.totalSales), label: 'Total Sales' }]
      };
    });

    // Profit by salesperson
    this.analytics.getSalespersonProfit(filters).subscribe(data => {
      this.profitData = data;
      this.profitChartData = {
        labels: data.map((p: any) => p.salesPersonName),
        datasets: [{ data: data.map((p: any) => p.grossProfit), label: 'Gross Profit' }]
      };
    });

    // Quota data
    this.analytics.getSalespersonQuota(filters).subscribe(data => {
      this.quotaData = data;
    });

    // Territory map
    this.analytics.getSalespersonTerritoryMap(filters).subscribe(data => {
      this.territoryMap = data;
    });
  }

  ngOnDestroy(): void {
    this.filterSub.unsubscribe();
  }
}