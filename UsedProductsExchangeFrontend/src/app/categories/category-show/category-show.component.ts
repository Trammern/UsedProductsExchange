import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {Category} from '../../_models/category';
import {Observable} from 'rxjs';
import {FilteredList} from '../../_models/filtered-list';
import {Filter} from '../../_models/filter';
import {catchError, tap} from 'rxjs/operators';
import {CategoriesService} from '../../_services/categories.service';
import {ActivatedRoute} from '@angular/router';
import {AuthenticationService} from '../../_services/authentication.service';

@Component({
  selector: 'app-category-show',
  templateUrl: './category-show.component.html',
  styleUrls: ['./category-show.component.css']
})
export class CategoryShowComponent implements OnInit {
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

  constructor(private categoriesService: CategoriesService,
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
      filter.searchField = 'name';
    }
    this.listData$ = this.categoriesService.getItems(filter).pipe(
      tap(filteredList => {
        this.count = filteredList.totalCount;
        this.categories = filteredList.list;
      }),
      catchError(this.err)
    );
  }

  private getCategoryProducts(id: number): void {
    this.chosenCategory$ = this.categoriesService.getItem(id);
  }

}
