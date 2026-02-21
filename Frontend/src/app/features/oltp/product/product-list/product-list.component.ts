import { Component, OnInit } from '@angular/core';
import { ProductService, Product } from '../product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];

  constructor(private productService: ProductService, private router: Router) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAll().subscribe(data => this.products = data);
  }

  edit(id: number): void {
    this.router.navigate(['/oltp/products/edit', id]);
  }

  delete(id: number): void {
    if (confirm('Are you sure?')) {
      this.productService.delete(id).subscribe(() => this.loadProducts());
    }
  }

  add(): void {
    this.router.navigate(['/oltp/products/add']);
  }
}