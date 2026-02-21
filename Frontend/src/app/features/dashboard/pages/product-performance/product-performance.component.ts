import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AnalyticsService } from '../../services/analytics.service';
import { FilterService, Filter } from '../../../../core/services/filter.service';
import { ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-product-performance',
  templateUrl: './product-performance.component.html',
  styleUrls: ['./product-performance.component.css']
})
export class ProductPerformanceComponent implements OnInit, OnDestroy {
  // Raw data
  categoryData: any[] = [];
  topProfitProducts: any[] = [];
  matrixData: any[] = [];

  // Chart data
  topProfitChartData: ChartConfiguration['data'] = { datasets: [] };
  scatterChartData: ChartConfiguration['data'] = { datasets: [] };

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
    // Category / SubCategory sales
    this.analytics.getProductCategorySales(filters).subscribe(data => {
      this.categoryData = data;
    });

    // Top products by profit
    this.analytics.getTopProductsByProfit(15, filters).subscribe(data => {
      this.topProfitProducts = data;
      this.topProfitChartData = {
        labels: data.map((p: any) => p.productName),
        datasets: [{ data: data.map((p: any) => p.grossProfit), label: 'Gross Profit' }]
      };
    });

    // Scatter data (units vs margin)
    this.analytics.getProductScatterData(filters).subscribe(data => {
      this.scatterChartData = {
        datasets: [{
          data: data.map((d: any) => ({ x: d.units, y: d.margin })),
          label: 'Products',
          pointRadius: 5,
          backgroundColor: 'rgba(75, 192, 192, 0.5)'
        }]
      };
    });

    // Product matrix
    this.analytics.getProductMatrix(filters).subscribe(data => {
      this.matrixData = data;
    });
  }

  ngOnDestroy(): void {
    this.filterSub.unsubscribe();
  }
}