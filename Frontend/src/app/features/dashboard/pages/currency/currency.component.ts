import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService } from '../../services/analytics.service';
import { FilterService, Filter } from '../../../../core/services/filter.service';
import { ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-currency',
  templateUrl: './currency.component.html',
  styleUrls: ['./currency.component.css']
})
export class CurrencyComponent implements OnInit, OnDestroy {
  ratesData: ChartConfiguration['data'] = { datasets: [] };
  salesByCurrencyData: ChartConfiguration['data'] = { datasets: [] };

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
    // If your backend returns data, populate charts
    this.analytics.getCurrencyRates(filters).subscribe(data => {
      // Example: assume data = [{ date, rate }]
      this.ratesData = {
        labels: data.map(d => d.date),
        datasets: [{ data: data.map(d => d.rate), label: 'Exchange Rate' }]
      };
    });

    this.analytics.getSalesByCurrency(filters).subscribe(data => {
      this.salesByCurrencyData = {
        labels: data.map(d => d.currency),
        datasets: [{ data: data.map(d => d.sales), label: 'Sales' }]
      };
    });
  }

  ngOnDestroy(): void {
    this.filterSub.unsubscribe();
  }
}