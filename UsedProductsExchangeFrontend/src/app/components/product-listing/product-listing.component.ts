import { Component, OnInit } from '@angular/core';
import {Product} from '../../_models/product';
import {ProductsService} from '../../_services/products.service';
import {Observable} from 'rxjs';
import {FormBuilder, FormGroup} from '@angular/forms';
import {FilteredList} from '../../_models/filtered-list';
import {Category} from '../../_models/category';
import {Filter} from '../../_models/filter';
import {catchError, tap} from 'rxjs/operators';

@Component({
  selector: 'app-product-listing',
  templateUrl: './product-listing.component.html',
  styleUrls: ['./product-listing.component.css']
})
export class ProductListingComponent implements OnInit {
  filterForm: FormGroup;
  listData$: Observable<FilteredList<Product>>;
  products: Product[];
  filter: Filter = {
    itemsPrPage: 10,
    currentPage: 1
  };
  count: number;
  err: any;
  constructor(private productsService: ProductsService, private fb: FormBuilder) { }

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
      filter.searchField = 'Name';
    }
    this.listData$ = this.productsService.getItems(filter).pipe(
      tap(filteredList => {
        this.count = filteredList.totalCount;
        this.products = filteredList.list;
      }),
      catchError(this.err)
    );
  }

  delete(category: Category): void {
    this.productsService.Remove(category.id)
      .pipe(
        tap(() => this.getProducts()),
        catchError(err => {
          return err;
        })
      ).subscribe();
  }
  get itemsPrPage(): number { return (this.filterForm.value as Filter).itemsPrPage; }
  get currentPage(): number { return (this.filterForm.value as Filter).currentPage; }
  get maxPages(): number { return Math.ceil(this.count / this.itemsPrPage); }
}
