import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Category } from '../_models/category';
import { AuthenticationService } from './authentication.service';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    'Authorization': 'my-auth-token'
  })
};

@Injectable()
export class CategoriesService {
  constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }

  getItems(): Observable<Category[]> {
    // add authorization header with jwt token
    httpOptions.headers =
      httpOptions.headers.set('Authorization', 'Bearer ' + this.authenticationService.getToken());

    // get categories from api
    return this.http.get<Category[]>(environment.apiUrl + '/categories', httpOptions);
  }

  getItem(id: number): Observable<Category> {
    // add authorization header with jwt token
    httpOptions.headers =
      httpOptions.headers.set('Authorization', 'Bearer ' + this.authenticationService.getToken());

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
}
