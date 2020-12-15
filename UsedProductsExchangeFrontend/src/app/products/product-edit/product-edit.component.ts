import { Component, OnInit } from '@angular/core';
import {ProductsService} from '../../_services/products.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {catchError, switchMap, take, tap} from 'rxjs/operators';
import {Observable, of} from 'rxjs';
import {Product} from '../../_models/product.model';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})
export class ProductEditComponent implements OnInit {
  updateObservable$: Observable<Product[]>;
  editProductForm: FormGroup;
  submitted = false;
  loading = false;
  errString: string;
  err: any;
  product: Product;
  public response: {dbPath: ''};

  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private productsService: ProductsService,
              private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.updateObservable$ = this.activeRoute.paramMap
      .pipe(
        take(1),
        switchMap(params => {
          this.errString = '';
          const id = +params.get('id');
          return this.productsService.getItem(id);
        }),
        tap(product => {
          this.product = product;
          this.editProductForm.patchValue(product);
          this.editProductForm.patchValue({
            productIdAfter: product.productId
          });
        }),
        catchError(this.err)
      );

    //  Initialize the form group
    this.editProductForm = this.formBuilder.group({
      name: ['', Validators.required],
      expiration: ['', Validators.required],
      categoryId: ['', Validators.required],
      price: ['', Validators.required],
      description: [''],
    });
  }


  onSubmit(): void{
    this.submitted = true;

    // stop here if form is invalid
    if (this.editProductForm.invalid) {
      return;
    }

    this.loading = true;

    const updatedProduct = this.editProductForm.value;
    if (this.response.dbPath !== '') {
      updatedProduct.pictureUrl = this.response.dbPath;
    } else {
      updatedProduct.pictureUrl = this.product.pictureUrl;
    }
    updatedProduct.productId = updatedProduct.productIdAfter;

    this.productsService.update(updatedProduct)
      .pipe(
        catchError(err => {
          this.errString = err.error ?? err.message;
          return of();
        })
      )
      .subscribe(product => {
        console.log('product', product);
        this.router.navigateByUrl('products');
      });
  }

  uploadFinished($event: any): void {
    this.response = $event;
  }
}
