import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService, Product } from '../product.service';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {
  productForm: FormGroup;
  isEdit = false;
  productId?: number;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public  router: Router,
    private productService: ProductService
  ) {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(0)]],
      stock: [0, [Validators.required, Validators.min(0)]],
      subCategoryId: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEdit = true;
        this.productId = +params['id'];
        this.productService.get(this.productId).subscribe(product => {
          this.productForm.patchValue(product);
        });
      }
    });
  }

  onSubmit(): void {
    if (this.productForm.invalid) return;
    const product: Product = this.productForm.value;
    if (this.isEdit && this.productId) {
      this.productService.update(this.productId, product).subscribe(() => {
        this.router.navigate(['/oltp/products']);
      });
    } else {
      this.productService.create(product).subscribe(() => {
        this.router.navigate(['/oltp/products']);
      });
    }
  }
}