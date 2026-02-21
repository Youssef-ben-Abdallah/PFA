import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { KpiCardComponent } from './components/kpi-card/kpi-card.component';
import { FilterBarComponent } from './components/filter-bar/filter-bar.component';

@NgModule({
  declarations: [KpiCardComponent, FilterBarComponent],
  imports: [CommonModule, ReactiveFormsModule],
  exports: [KpiCardComponent, FilterBarComponent, ReactiveFormsModule]
})
export class SharedModule {}