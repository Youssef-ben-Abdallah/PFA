import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { OltpRoutingModule } from './oltp-routing.module';
import { SharedModule } from '../../shared/shared.module';

import { ProductListComponent } from './product/product-list/product-list.component';
import { ProductFormComponent } from './product/product-form/product-form.component';
import { CategoryListComponent } from './category/category-list/category-list.component';
import { CategoryFormComponent } from './category/category-form/category-form.component';
import { OrderListComponent } from './order/order-list/order-list.component';

@NgModule({
  declarations: [
    ProductListComponent,
    ProductFormComponent,
    CategoryListComponent,
    CategoryFormComponent,
    OrderListComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    OltpRoutingModule,
    SharedModule
  ]
})
export class OltpModule {}