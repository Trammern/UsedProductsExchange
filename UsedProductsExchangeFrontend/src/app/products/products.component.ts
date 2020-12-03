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

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  filterForm: FormGroup;
  categories: Category[];
  listData$: Observable<FilteredList<Category>>;
  filter: Filter = {
    itemsPrPage: 5,
    currentPage: 1
  };
  count: number;
  err: any;
  chosenCategory$: Observable<Category>;

  constructor(private productsService: ProductsService, private categoriesService: CategoriesService, private fb: FormBuilder, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.filterForm = this.fb.group({
      itemsPrPage: [''],
      currentPage: [''],
      searchText: ['']
    });
    this.filterForm.patchValue(this.filter);
    this.getCategories();
    this.filterForm.valueChanges.subscribe(() => {
      this.getCategories();
    });

    this.activeRoute.params.subscribe(routeParams => {
      this.getCategoryProducts(routeParams.id);
    });
  }

  getCategories(currentPage: number = 0): void {
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
    this.listData$ = this.categoriesService.getItems(filter).pipe(
      tap(filteredList => {
        this.count = filteredList.totalCount;
        this.categories = filteredList.list;
      }),
      catchError(this.err)
    );
    console.log(this.listData$);
    console.log(this.categories);
  }

  private getCategoryProducts(id: number): void {
    this.chosenCategory$ = this.categoriesService.getItem(id);
  }

}
