import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AuthenticationService} from './authentication.service';
import {Observable} from 'rxjs';
import {Category} from '../_models/category';
import {environment} from '../../environments/environment';
import {User} from '../_models/user';
import {getEntryPointInfo} from '@angular/compiler-cli/ngcc/src/packages/entry_point';
import {Product} from '../_models/product.model';
import {catchError, first} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }

  private currentUser: User;

  // GET
  getItem(id: number): Observable<User> {
    // get product from api
    return this.http.get<User>(environment.apiUrl + '/users/' + id);
  }

  update(userUpdated: User): Observable<User> {
    return this.http.put<User>(environment.apiUrl + '/users/' + userUpdated.userId, userUpdated);
  }
}





