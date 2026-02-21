import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Product {
  id?: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  subCategoryId: number;
}

@Injectable({ providedIn: 'root' })
export class ProductService {
  private api = 'https://localhost:5125/api/products';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.api);
  }

  get(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.api}/${id}`);
  }

  create(product: Product): Observable<Product> {
    return this.http.post<Product>(this.api, product);
  }

  update(id: number, product: Product): Observable<void> {
    return this.http.put<void>(`${this.api}/${id}`, product);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }
}