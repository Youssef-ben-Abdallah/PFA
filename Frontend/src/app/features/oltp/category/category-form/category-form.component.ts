import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService, Category } from '../category.service';

@Component({
  selector: 'app-category-form',
  templateUrl: './category-form.component.html',
  styleUrls: ['./category-form.component.css']
})
export class CategoryFormComponent implements OnInit {
  categoryForm: FormGroup;
  isEdit = false;
  categoryId?: number;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public router: Router,
    private categoryService: CategoryService
  ) {
    this.categoryForm = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEdit = true;
        this.categoryId = +params['id'];
        this.categoryService.get(this.categoryId).subscribe(category => {
          this.categoryForm.patchValue(category);
        });
      }
    });
  }

  onSubmit(): void {
    if (this.categoryForm.invalid) return;
    const category: Category = this.categoryForm.value;
    if (this.isEdit && this.categoryId) {
      this.categoryService.update(this.categoryId, category).subscribe(() => {
        this.router.navigate(['/oltp/categories']);
      });
    } else {
      this.categoryService.create(category).subscribe(() => {
        this.router.navigate(['/oltp/categories']);
      });
    }
  }
}