// shipping-operations.component.ts
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService } from '../../services/analytics.service';
import { FilterService, Filter } from '../../../../core/services/filter.service';
import { ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-shipping-operations',
  templateUrl: './shipping-operations.component.html',
  styleUrls: ['./shipping-operations.component.css']
})
export class ShippingOperationsComponent implements OnInit, OnDestroy {
  salesByMethodData: ChartConfiguration['data'] = { datasets: [] };
  avgDeliveryDays: number = 0;
  lateShipments: any = {};

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
    this.analytics.getSalesByShipMethod(filters).subscribe(data => {
      this.salesByMethodData = {
        labels: data.map(d => d.shipMethod),
        datasets: [{ data: data.map(d => d.totalSales), label: 'Sales' }]
      };
    });

    this.analytics.getAvgDeliveryDays(filters).subscribe(data => {
      this.avgDeliveryDays = data.avgDeliveryDays;
    });

    this.analytics.getLateShipments(filters).subscribe(data => {
      this.lateShipments = data;
    });
  }

  ngOnDestroy(): void {
    this.filterSub.unsubscribe();
  }
}