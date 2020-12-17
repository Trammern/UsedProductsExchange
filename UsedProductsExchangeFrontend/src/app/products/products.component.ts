import { Component, OnInit } from '@angular/core';
import {Category} from '../_models/category';
import {CategoriesService} from '../_services/categories.service';
import {ProductsService} from '../_services/products.service';
import {catchError, tap} from 'rxjs/operators';
import {Observable} from 'rxjs';
import {Filter} from '../_models/filter';
import {FormBuilder, FormGroup} from '@angular/forms';
import {FilteredList} from '../_models/filtered-list';
import {ActivatedRoute} from '@angular/router';
import {Product} from '../_models/product.model';
import {environment} from '../../environments/environment';
import {AuthenticationService} from '../_services/authentication.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  filterForm: FormGroup;
  listData$: Observable<FilteredList<Product>>;
  products: Product[];
  chosenProduct$: Observable<Product>;
  filter: Filter = {
    itemsPrPage: 5,
    currentPage: 1
  };
  count: number;
  err: any;

  constructor(private productsService: ProductsService,
              public authenticationsService: AuthenticationService,
              private fb: FormBuilder,
              private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.filterForm = this.fb.group({
      itemsPrPage: [''],
      currentPage: [''],
      searchText: ['']
    });
    this.filterForm.patchValue(this.filter);
    this.getProducts();
    this.filterForm.valueChanges.subscribe(() => {
      this.getProducts();
    });
  }

  getProducts(currentPage: number = 0): void {
    if (currentPage > 0) {
      this.filterForm.patchValue({ currentPage });
    }
    const filter = this.filterForm.value as Filter;
    if (filter.currentPage <= 0) {
      filter.currentPage = 1;
    }
    if (filter.searchText) {
      filter.searchField = 'name';
    }
    this.listData$ = this.productsService.getItems(filter).pipe(
      tap(filteredList => {
        this.count = filteredList.totalCount;
        this.products = filteredList.list;
      }),
      catchError(this.err)
    );
  }

}
