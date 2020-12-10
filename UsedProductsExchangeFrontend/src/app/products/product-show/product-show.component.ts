import { Component, OnInit } from '@angular/core';
import {Observable} from 'rxjs';
import {ActivatedRoute} from '@angular/router';
import {Product} from '../../_models/product.model';
import {ProductsService} from '../../_services/products.service';

@Component({
  selector: 'app-product-show',
  templateUrl: './product-show.component.html',
  styleUrls: ['./product-show.component.css']
})
export class ProductShowComponent implements OnInit {
  err: any;
  chosenProduct$: Observable<Product>;

  constructor(private productsService: ProductsService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe(routeParams => {
      this.chosenProduct$ = this.productsService.getItem(routeParams.id);
    });
  }
}
