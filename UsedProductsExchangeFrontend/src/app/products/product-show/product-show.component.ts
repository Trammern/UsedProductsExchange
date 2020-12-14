import { Component, OnInit } from '@angular/core';
import {Observable} from 'rxjs';
import {ActivatedRoute} from '@angular/router';
import {Product} from '../../_models/product.model';
import {ProductsService} from '../../_services/products.service';
import {environment} from '../../../environments/environment';
import {formatCurrency} from '@angular/common';

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

  public createImgPath = (serverPath: string) => {
    return environment.url + '/' + serverPath;
  }

  formatCurrency(price: number, locale: string, currency: string): string {
    return formatCurrency(price, locale, currency);
  }
}
