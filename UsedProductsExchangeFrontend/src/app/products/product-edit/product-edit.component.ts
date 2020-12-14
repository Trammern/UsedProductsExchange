import { Component, OnInit } from '@angular/core';
import {ProductsService} from '../../_services/products.service';
import {AuthenticationService} from '../../_services/authentication.service';
import {FormBuilder, FormGroup} from '@angular/forms';
import {catchError} from 'rxjs/operators';
import {of} from 'rxjs';
import {Product} from '../../_models/product.model';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})
export class ProductEditComponent implements OnInit {

  currentProduct: Product;
  productUpdateForm: FormGroup;
  errString: string;
  err: any;

  constructor(private productService: ProductsService,
              private authService: AuthenticationService,
              private fb: FormBuilder
             ) { }

  ngOnInit(): void {
    this.currentProduct = new Product();
    this.currentProduct.name = 'test';
    this.currentProduct.description = 'testdesc';
    this.currentProduct.categoryId = 1;
    this.currentProduct.pictureUrl = '';

    this.productUpdateForm = this.fb.group({
      name: [this.currentProduct.name],
      category: [this.currentProduct.categoryId],
      description: [this.currentProduct.description],
      picture: [this.currentProduct.pictureUrl]
    });
  }


  onSubmit(): void{
    this.errString ='';
    this.currentProduct.name = this.productUpdateForm.get('name').value;
    this.currentProduct.categoryId = this.productUpdateForm.get('category').value;
    this.currentProduct.description = this.productUpdateForm.get('description').value;
    this.currentProduct.pictureUrl = this.productUpdateForm.get('picture').value;

    this.productService.updateProduct(this.currentProduct).pipe(
      catchError(err => {
        this.errString = err.error ?? err.message;
        return of();
      })
    )
      .subscribe(product=>{
        console.log('product', product);
      });
  }
}
