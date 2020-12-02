import { Component, OnInit } from '@angular/core';
import {FormControl} from '@angular/forms';
import {Product} from '../../_models/product';
import {ProductsService} from '../../_services/products.service';
import {catchError} from 'rxjs/operators';

@Component({
  selector: 'app-update-product',
  templateUrl: './update-product.component.html',
  styleUrls: ['./update-product.component.css']
})
export class UpdateProductComponent implements OnInit {
  name = new FormControl('');
  description = new FormControl('');
  price = new FormControl('');
  expirationDate = new FormControl('');

  updateProduct: any;
  private errorMessage: any;

  constructor(private productsService: ProductsService) {
  }

  ngOnInit(): void {
  }

  Submit() {
    let uProductName = this.name.value;
    let uProductExpirationDate = this.expirationDate.value;
    let uProductPrice = this.price.value;
    let uProductDescription = this.description.value;
    let uProductId = 2
    let uProductImagePath = "something"
    let uProductUserId = 1;
    let uProdutcCategoryId = 1;

    let product: Product =
      {
        productId: uProductId,
        categoryId: uProdutcCategoryId,
        name: uProductName,
        userId: uProductUserId,
        expiration: uProductExpirationDate,
        currentPrice: uProductPrice,
        description: uProductDescription,
        pictureURL: uProductImagePath
      }

    this.productsService.updateProduct(product)
      .pipe(catchError(err =>{
          this.errorMessage = err.error;
          return err;
        })
      ).subscribe();



  }
}
