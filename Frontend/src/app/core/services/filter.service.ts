import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface Filter {
  from?: string;
  to?: string;
  territoryId?: number;
  categoryId?: number;
  // add more as needed
}

@Injectable({ providedIn: 'root' })
export class FilterService {
  private filtersSubject = new BehaviorSubject<Filter>({});
  filters$: Observable<Filter> = this.filtersSubject.asObservable();

  updateFilters(filters: Filter): void {
    this.filtersSubject.next(filters);
  }
}