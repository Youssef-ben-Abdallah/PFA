import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExecutiveOverviewComponent } from './pages/executive-overview/executive-overview.component';
import { SalesTrendsComponent } from './pages/sales-trends/sales-trends.component';
import { ProductPerformanceComponent } from './pages/product-performance/product-performance.component';
import { CustomerAnalysisComponent } from './pages/customer-analysis/customer-analysis.component';
import { SalespersonPerformanceComponent } from './pages/salesperson-performance/salesperson-performance.component';
import { ShippingOperationsComponent } from './pages/shipping-operations/shipping-operations.component';
import { CurrencyComponent } from './pages/currency/currency.component';

const routes: Routes = [
  { path: '', redirectTo: 'executive-overview', pathMatch: 'full' },
  { path: 'executive-overview', component: ExecutiveOverviewComponent },
  { path: 'sales-trends', component: SalesTrendsComponent },
  { path: 'product-performance', component: ProductPerformanceComponent },
  { path: 'customer-analysis', component: CustomerAnalysisComponent },
  { path: 'salesperson-performance', component: SalespersonPerformanceComponent },
  { path: 'shipping-operations', component: ShippingOperationsComponent },
  { path: 'currency', component: CurrencyComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule {}