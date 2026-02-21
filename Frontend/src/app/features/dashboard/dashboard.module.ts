import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { NgChartsModule } from 'ng2-charts';

import { ExecutiveOverviewComponent } from './pages/executive-overview/executive-overview.component';
import { SalesTrendsComponent } from './pages/sales-trends/sales-trends.component';
import { ProductPerformanceComponent } from './pages/product-performance/product-performance.component';
import { CustomerAnalysisComponent } from './pages/customer-analysis/customer-analysis.component';
import { SalespersonPerformanceComponent } from './pages/salesperson-performance/salesperson-performance.component';
import { ShippingOperationsComponent } from './pages/shipping-operations/shipping-operations.component';
import { CurrencyComponent } from './pages/currency/currency.component';

@NgModule({
  declarations: [
    ExecutiveOverviewComponent,
    SalesTrendsComponent,
    ProductPerformanceComponent,
    CustomerAnalysisComponent,
    SalespersonPerformanceComponent,
    ShippingOperationsComponent,
    CurrencyComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    SharedModule,
    NgChartsModule
  ]
})
export class DashboardModule {}