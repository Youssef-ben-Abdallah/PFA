import { Component, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Filter } from '../../../core/services/filter.service';

@Component({
  selector: 'app-filter-bar',
  templateUrl: './filter-bar.component.html',
  styleUrls: ['./filter-bar.component.css']
})
export class FilterBarComponent {
  @Output() filterChange = new EventEmitter<Filter>();
  filterForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.filterForm = this.fb.group({
      from: [''],
      to: [''],
      territoryId: ['']
    });
  }

  applyFilters(): void {
    this.filterChange.emit(this.filterForm.value);
  }

  reset(): void {
    this.filterForm.reset();
    this.filterChange.emit({});
  }
}