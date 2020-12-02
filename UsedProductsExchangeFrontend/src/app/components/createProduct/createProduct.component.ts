import { Component, OnInit } from '@angular/core';
import {ProductsService} from '../../_services/products.service';
import {Product} from '../../_models/product';
import {FormControl} from '@angular/forms';
import {catchError} from 'rxjs/operators';

@Component({
  selector: 'app-profile-view',
  templateUrl: './createProduct.component.html',
  styleUrls: ['./createProduct.component.css']
})
export class createProductComponent implements OnInit {

 submitProduct: any;
  name= new FormControl('');
  description= new FormControl('');
  price= new FormControl('');
  expirationDate= new FormControl('');

  errorMessage: string = "";

  constructor(private productsService: ProductsService) { }

  ngOnInit(): void {
  }

  Submit(): void
  {
    let productName = this.name.value
    let productDescription = this.description.value
    let productPrice = this.price.value
    let productExpirationDate = this.expirationDate.value

    let product: Product = {
      name: productName,
      description: productDescription,
      currentPrice: productPrice,
      expiration: productExpirationDate,
      pictureURL: "something",
      categoryId: 1,
      userId: 1
    }

    this.productsService.createProduct(product)
      .pipe(catchError(err =>{
        this.errorMessage = err.error;
        return err;
      })
      ).subscribe();
  }


}
