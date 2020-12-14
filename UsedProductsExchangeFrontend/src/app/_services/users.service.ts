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

  usersApiUrl = environment.apiUrl + '/user'
  private currentUser: User;

  getUser(id: number): Observable<User> {
    return this.http.get<User>(this.usersApiUrl + '/' + id).pipe(first());
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(environment.apiUrl);
  }


  put(updatedUser: User) {
    return this.http.put<User>(this.usersApiUrl + '/' + updatedUser.userId, updatedUser).pipe();
  }

  getCurrentUser(): User{
    return this.currentUser;
  }

  setCurrentUser(user: User){
    this.currentUser = user;
  }
}





