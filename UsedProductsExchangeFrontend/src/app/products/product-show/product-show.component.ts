import { Component, OnInit } from '@angular/core';
import {Observable} from 'rxjs';
import {ActivatedRoute} from '@angular/router';
import {Product} from '../../_models/product.model';
import {ProductsService} from '../../_services/products.service';
import {environment} from '../../../environments/environment';
import {formatCurrency} from '@angular/common';
import {BidService} from '../../_services/bid.service';
import {Bid} from '../../_models/bid';
import {AuthenticationService} from '../../_services/authentication.service';
import {catchError, map} from 'rxjs/operators';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

class FromGroup {
}

@Component({
  selector: 'app-product-show',
  templateUrl: './product-show.component.html',
  styleUrls: ['./product-show.component.css']
})
export class ProductShowComponent implements OnInit {
  err: any;
  chosenProduct$: Observable<Product>;
  potentialBid: Bid;
  addBidForm: FormGroup;


  constructor(private productsService: ProductsService,
              private activeRoute: ActivatedRoute,
              private bidService: BidService,
              private authenticationSerivce: AuthenticationService,
              private formBuilder: FormBuilder) { }

  ngOnInit(): void {

    this.addBidForm = this.formBuilder.group({
      price: ['']
    });

    this.potentialBid = new Bid();

    this.activeRoute.params.subscribe(routeParams => {
      this.chosenProduct$ = this.productsService.getItem(routeParams.id);
      this.potentialBid.productId = routeParams.id;
      this.potentialBid.userId = this.authenticationSerivce.getUser().userId;
    });
  }

  public createImgPath = (serverPath: string) => {
    return environment.url + '/' + serverPath;
  }


  formatCurrency(price: number, locale: string, currency: string): string {
    return formatCurrency(price, locale, currency);
  }

  placeBid(){

    this.potentialBid.price = this.addBidForm.get('price').value;

    this.bidService.add(this.potentialBid)
      .subscribe();
  }
}
