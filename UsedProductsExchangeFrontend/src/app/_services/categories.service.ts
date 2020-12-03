import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Category } from '../_models/category';
import { AuthenticationService } from './authentication.service';
import {Filter} from '../_models/filter';
import {FilteredList} from '../_models/filtered-list';
import {Product} from '../_models/product';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    Authorization: 'my-auth-token'
  })
};

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

  getItem(id: number): Observable<Category> {
<<<<<<< Updated upstream
    // add authorization header with jwt token
    httpOptions.headers =
      httpOptions.headers.set('Authorization', 'Bearer ' + this.authenticationService.getToken());

=======
>>>>>>> Stashed changes
    // get categories from api
    return this.http.get<Category>(environment.apiUrl + '/categories/' + id, httpOptions);
  }

  updateCategory(categoryUpdated: Category): Observable<Category> {
    return this.http.put<Category>(environment.apiUrl + '/categories/' + categoryUpdated.id, categoryUpdated);
  }

  Remove(id: number): Observable<Category> {
    // add authorization header with jwt token
    httpOptions.headers =
      httpOptions.headers.set('Authorization', 'Bearer ' + this.authenticationService.getToken());

    // get categories from api
    return this.http.delete<Category>(environment.apiUrl + '/categories/' + id, httpOptions);
  }
  //POST
  Post(category: Category): Observable<Category>{
    return this.http.post<Category>(environment.apiUrl, category);
  }
}
