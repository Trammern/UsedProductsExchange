import { Component, OnInit } from '@angular/core';
import {Category} from '../_models/category';
import {FormBuilder, FormGroup} from '@angular/forms';
import {Observable} from 'rxjs';
import {CategoriesService} from '../_services/categories.service';
import {catchError, tap} from 'rxjs/operators';
import {Filter} from '../_models/filter';
import {FilteredList} from '../_models/filtered-list';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
  filterForm: FormGroup;
  listData$: Observable<FilteredList<Category>>;
  categories: Category[];
  filter: Filter = {
    itemsPrPage: 5,
    currentPage: 1
  };
  count: number;
  err: any;
  constructor(private categoriesService: CategoriesService, private fb: FormBuilder) { }

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
  }

  delete(category: Category): void {
    this.categoriesService.Remove(category.id)
      .pipe(
        tap(() => this.getCategories()),
        catchError(err => {
          return err;
        })
      ).subscribe();
  }
  get itemsPrPage(): number { return (this.filterForm.value as Filter).itemsPrPage; }
  get currentPage(): number { return (this.filterForm.value as Filter).currentPage; }
  get maxPages(): number { return Math.ceil(this.count / this.itemsPrPage); }

}
