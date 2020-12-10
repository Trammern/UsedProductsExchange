import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {ProductsService} from '../../_services/products.service';
import {Product} from '../../_models/product.model';
import {catchError} from 'rxjs/operators';
import {AuthenticationService} from '../../_services/authentication.service';

@Component({
  selector: 'app-product-add',
  templateUrl: './product-add.component.html',
  styleUrls: ['./product-add.component.css']
})
export class ProductAddComponent implements OnInit {
  addProductForm: FormGroup;
  submitted = false;
  loading = false;
  errormessage = '';

  constructor(private formBuilder: FormBuilder, private router: Router, private productsService: ProductsService, private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    //  Initialize the form group
    this.addProductForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: [''],
    });
  }

  get name() { return this.addProductForm.get('name'); }
  get description() { return this.addProductForm.get('description'); }
  get categoryId() { return this.addProductForm.get('category'); }
  get expiration() { return this.addProductForm.get('expiration'); }
  get price() { return this.addProductForm.get('price'); }

  onSubmit(): void {
    this.submitted = true;

    // stop here if form is invalid
    if (this.addProductForm.invalid) {
      return;
    }

    this.loading = true;

    const nameValue = this.name.value;
    const descriptionValue = this.description.value;
    const categoryValue = this.categoryId.value;
    const expirationValue = this.expiration.value;
    const priceValue = this.price.value;

    const product: Product = {
      name: nameValue,
      bids: [],
      categoryId: categoryValue,
      currentPrice: priceValue,
      description: descriptionValue,
      expiration: expirationValue,
      pictureUrl: '',
      userId: this.authenticationService.getUser().id,
    };

    this.productsService.add(product)
      .pipe(
        catchError(err => {
          this.loading = false;
          this.errormessage = err.error;
          return err;
        })
      )
      .subscribe(city => {
        this.name.reset();
        this.loading = false;
        this.errormessage = '';
      });
  }

}
