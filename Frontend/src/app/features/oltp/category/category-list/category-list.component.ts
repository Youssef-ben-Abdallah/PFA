import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CategoryService, Category } from '../category.service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  categories: Category[] = [];

  constructor(private categoryService: CategoryService, private router: Router) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe(data => this.categories = data);
  }

  edit(id: number): void {
    this.router.navigate(['/oltp/categories/edit', id]);
  }

  delete(id: number): void {
    if (confirm('Are you sure?')) {
      this.categoryService.delete(id).subscribe(() => this.loadCategories());
    }
  }

  add(): void {
    this.router.navigate(['/oltp/categories/add']);
  }
}