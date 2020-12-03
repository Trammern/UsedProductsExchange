import { Component, OnInit } from '@angular/core';
import {ProductsService} from '../../_services/products.service';
import {Product} from '../../_models/product';
import {catchError, tap} from 'rxjs/operators';
import {Filter} from '../../_models/filter';
import {FormGroup} from '@angular/forms';
import {Observable} from 'rxjs';
import {FilteredList} from '../../_models/filtered-list';

@Component({
  selector: 'app-delete-product',
  templateUrl: './delete-product.component.html',
  styleUrls: ['./delete-product.component.css']
})
export class DeleteProductComponent implements OnInit {
  filterForm: FormGroup;
  selectedProduct: Product;
  listData$: Observable<FilteredList<Product>>;
  products: Product[];
  count: number;
  err: any;

  constructor(private productsService: ProductsService) { }

  ngOnInit(): void {
    this.selectedProduct = this.productsService.GetCurrentProduct();
  }

  delete(): void {
    this.productsService.Remove(this.selectedProduct.productId)
      .pipe(
        tap(() => this.getProducts()),
        catchError(err => {
          return err;
        })
      ).subscribe();
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

}
