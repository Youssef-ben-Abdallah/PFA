// sales-trends.component.ts
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService } from '../../services/analytics.service';
import { FilterService, Filter } from '../../../../core/services/filter.service';
import { ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-sales-trends',
  templateUrl: './sales-trends.component.html',
  styleUrls: ['./sales-trends.component.css']
})
export class SalesTrendsComponent implements OnInit, OnDestroy {
  dailyData: ChartConfiguration['data'] = { datasets: [] };
  monthlyData: ChartConfiguration['data'] = { datasets: [] };
  cumulativeData: ChartConfiguration['data'] = { datasets: [] };
  yoyData: any[] = [];

  private filterSub: Subscription;

  constructor(
    private analytics: AnalyticsService,
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
    // Daily sales
    this.analytics.getSalesTrendsDaily(filters).subscribe(data => {
      this.dailyData = {
        labels: data.map(d => d.date),
        datasets: [{ data: data.map(d => d.sales), label: 'Daily Sales', fill: false }]
      };
    });

    // Monthly sales (for combo chart with orders)
    this.analytics.getSalesTrendsMonthly(filters).subscribe(data => {
      this.monthlyData = {
        labels: data.map(d => d.period),
        datasets: [
          { data: data.map(d => d.sales), label: 'Sales', type: 'bar', yAxisID: 'y' },
          // We'll need orders per month; maybe another endpoint or combine
        ]
      };
    });

    // Cumulative sales
    this.analytics.getCumulativeSales(filters).subscribe(data => {
      this.cumulativeData = {
        labels: data.map(d => d.date),
        datasets: [{ data: data.map(d => d.cumulative), label: 'Cumulative Sales', fill: true }]
      };
    });

    // YoY comparison
    this.analytics.getYoYComparison(filters).subscribe(data => {
      this.yoyData = data;
    });
  }

  ngOnDestroy(): void {
    this.filterSub.unsubscribe();
  }
}