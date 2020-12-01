import { Component, OnInit } from '@angular/core';
import {ProductsService} from '../_services/products.service';
import {Product} from '../_models/product';

@Component({
  selector: 'app-profile-view',
  templateUrl: './profile-view.component.html',
  styleUrls: ['./profile-view.component.css']
})
export class ProfileViewComponent implements OnInit {

 submitProduct: Product;

  constructor(private productsService: ProductsService) { }

  ngOnInit(): void {
  }

  Submit(): void
  {
   this.submitProduct.name = document.getElementById('prodnamefld').innerHTML;
   this.submitProduct.currentPrice = 200.00;
   this.submitProduct.expiration = document.getElementById('expdate').innerHTML;
   this.productsService.createProduct(this.submitProduct);
  }


}
