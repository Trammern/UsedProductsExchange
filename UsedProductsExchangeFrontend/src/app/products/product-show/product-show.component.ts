import { Component, OnInit } from '@angular/core';
import {Observable} from 'rxjs';
import {ActivatedRoute, Router} from '@angular/router';
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
  errormessage: string;


  constructor(private productsService: ProductsService,
              private activeRoute: ActivatedRoute,
              private router: Router,
              private bidService: BidService,
              private authenticationSerivce: AuthenticationService,
              private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.addBidForm = this.formBuilder.group({
      price: ['', Validators.required]
    });

    this.potentialBid = new Bid();

    this.activeRoute.params.subscribe(routeParams => {
      this.chosenProduct$ = this.productsService.getItem(routeParams.id);
      this.potentialBid.productId = routeParams.id;
      if (this.authenticationSerivce.userIsLoggedIn()) {
        this.potentialBid.userId = this.authenticationSerivce.getUser().userId;
      }
    });
  }

  public createImgPath = (serverPath: string) => {
    return environment.url + '/' + serverPath;
  }


  formatCurrency(price: number, locale: string, currency: string): string {
    return formatCurrency(price, locale, currency);
  }

  placeBid(): void {
    this.potentialBid.price = this.addBidForm.get('price').value;

    this.bidService.add(this.potentialBid)
      .pipe(
        catchError(err => {
          this.errormessage = err.error;
          return err;
        })
      )
      .subscribe(bid => {
        this.activeRoute.params.subscribe(routeParams => {
          this.chosenProduct$ = this.productsService.getItem(routeParams.id);
          this.potentialBid.productId = routeParams.id;
          if (this.authenticationSerivce.userIsLoggedIn()) {
            this.potentialBid.userId = this.authenticationSerivce.getUser().userId;
          }
        });
      });
  }

  deleteProduct(product: Product): void {
    this.productsService.remove(product.productId)
      .pipe(
        catchError(err => {
          this.errormessage = err.error;
          return err;
        })
      )
      .subscribe(bid => {
        this.router.navigateByUrl('/products');
      });
  }
}
