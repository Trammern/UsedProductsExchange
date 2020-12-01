import { Component, OnInit } from '@angular/core';
import {ProductsService} from "../_services/products.service";
import {Product} from "../_models/product";
import {ProductListingComponent} from "../components/product-listing/product-listing.component";

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  selectedProduct: Product;
  constructor(private productsService: ProductsService) { }

  ngOnInit(): void
  {
  }

  deleteProduct(number: number)
  {
this.productsService.Remove(this.selectedProduct.productId);
  }
}
