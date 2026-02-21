import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService, KpiData } from '../../services/analytics.service';
import { FilterService, Filter } from '../../../../core/services/filter.service';
import { ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-executive-overview',
  templateUrl: './executive-overview.component.html',
  styleUrls: ['./executive-overview.component.css']
})
export class ExecutiveOverviewComponent implements OnInit, OnDestroy {
  kpis: KpiData = {} as KpiData;
  salesByMonthData: ChartConfiguration['data'] = { datasets: [] };
  salesByTerritoryData: ChartConfiguration['data'] = { datasets: [] };
  onlineOfflineData: ChartConfiguration['data'] = { datasets: [] };
  topProducts: any[] = [];
  mapData: any[] = [];

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
    // initial load with empty filters
    this.loadData({});
  }

  loadData(filters: Filter): void {
    this.analytics.getKpis(filters).subscribe(data => this.kpis = data);
    this.analytics.getSalesByPeriod('month', filters).subscribe(data => {
      this.salesByMonthData = {
        labels: data.map(d => d.period),
        datasets: [{ data: data.map(d => d.total), label: 'Sales', fill: false }]
      };
    });
    this.analytics.getSalesByTerritory(filters).subscribe(data => {
      this.salesByTerritoryData = {
        labels: data.map(d => d.territory),
        datasets: [{ data: data.map(d => d.total), label: 'Sales' }]
      };
    });
    this.analytics.getOnlineOfflineRatio(filters).subscribe(data => {
      this.onlineOfflineData = {
        labels: ['Online', 'Offline'],
        datasets: [{ data: [data.online, data.offline] }]
      };
    });
    this.analytics.getTopProducts(10, filters).subscribe(data => this.topProducts = data);
    this.analytics.getMapData(filters).subscribe(data => this.mapData = data);
  }

  ngOnDestroy(): void {
    this.filterSub.unsubscribe();
  }
}