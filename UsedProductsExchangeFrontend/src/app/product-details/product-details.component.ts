import { Component, OnInit } from '@angular/core';
import {ProductsService} from "../_services/products.service";
import {Product} from "../_models/product";
import {ProductListingComponent} from "../components/product-listing/product-listing.component";
import {Observable} from 'rxjs';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  selectedProduct: Product;

  constructor(private productsService: ProductsService)
  {
    this.selectedProduct = this.productsService.GetCurrentProduct();
  }

  ngOnInit(): void
  {
  }

  deleteProduct():Observable<Product>
  {
      return this.productsService.Remove(this.selectedProduct.productId);
  }
}
