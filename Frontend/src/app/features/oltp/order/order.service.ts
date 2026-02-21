import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Order {
  id: number;
  orderDate: string;
  customerId: number;
  customerName?: string; // we might include from API
  totalAmount: number;
  status: string;
}

@Injectable({ providedIn: 'root' })
export class OrderService {
  private api = 'https://localhost:5125/api/orders';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Order[]> {
    return this.http.get<Order[]>(this.api);
  }

  get(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.api}/${id}`);
  }
  // No create/update/delete for simplicity
}