import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Category {
  id?: number;
  name: string;
  description: string;
}

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private api = 'https://localhost:5125/api/categories';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.api);
  }

  get(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.api}/${id}`);
  }

  create(category: Category): Observable<Category> {
    return this.http.post<Category>(this.api, category);
  }

  update(id: number, category: Category): Observable<void> {
    return this.http.put<void>(`${this.api}/${id}`, category);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }
}