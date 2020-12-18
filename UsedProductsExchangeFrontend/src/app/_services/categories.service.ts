import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Category } from '../_models/category';
import { AuthenticationService } from './authentication.service';
import {Filter} from '../_models/filter';
import {FilteredList} from '../_models/filtered-list';
import {Product} from '../_models/product.model';

@Injectable()
export class CategoriesService {
  constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }

  getItems(filter: Filter): Observable<FilteredList<Category>> {
    let url = environment.apiUrl + '/categories' + '?';
    if (filter && filter.itemsPrPage > 0 && filter.currentPage > 0) {
      url = url
        + 'itemsPrPage=' + filter.itemsPrPage
        + '&currentPage=' + filter.currentPage + '&';
    }
    if (filter && filter.searchField?.length > 0 && filter.searchText?.length > 0) {
      url = url
        + 'searchField=' + filter.searchField
        + '&searchText=' + filter.searchText;
    }
    return this.http.get<FilteredList<Category>>(url);
  }

  getAllItems(): Observable<Category[]> {
    const url = environment.apiUrl + '/categories';
    return this.http.get<Category[]>(url);
  }

  getItem(id: number): Observable<Category> {
    // get categories from api
    return this.http.get<Category>(environment.apiUrl + '/categories/' + id);
    // return this.http.get<Category>(environment.apiUrl + '/categories/' + id, httpOptions);
  }

  add(category: Category): Observable<Category> {
    return this.http.post<Category>(environment.apiUrl + '/categories', category);
  }

  update(categoryUpdated: Category): Observable<Category> {
    return this.http.put<Category>(environment.apiUrl + '/categories/' + categoryUpdated.categoryId, categoryUpdated);
  }

  remove(id: number): Observable<Category> {
    // get categories from api
    return this.http.delete<Category>(environment.apiUrl + '/categories/' + id);
  }
}
