import {EventEmitter, Injectable, Output} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import {User} from '../_models/user';

@Injectable()
export class AuthenticationService {
  @Output() user: EventEmitter<User> = new EventEmitter();

  constructor(private http: HttpClient) {}

  // Check if we can log in user. If we can, then we store the token and username in local storage.
  login(username: string, password: string): Observable<boolean> {
    return this.http.post<any>(environment.apiUrl + '/token', { username, password })
      .pipe(map(response => {
        const token = response.token;
        const account: User = response.account;
        // login successful if there's a jwt token in the response
        if (token) {
          // store username and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentUser', JSON.stringify({ account, token }));
          this.user.emit(account);
          // return true to indicate successful login
          return true;
        } else {
          // return false to indicate failed login
          return false;
        }
      }));
  }

  register(name: string, username: string, password: string, address: string, email: string): Observable<boolean> {
    console.log(name, username, password, address, email);
    return this.http.post<any>(environment.apiUrl + '/register', { name, username, password, address, email })
      .pipe(map(response => {
        console.log(response);
        const token = response.token;
        const account: User = response.account;
        // register successful if there's a jwt token in the response
        if (token) {
          // store username and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentUser', JSON.stringify({ account, token }));
          this.user.emit(account);
          // return true to indicate successful register login
          return true;
        } else {
          // return false to indicate failed registering
          return false;
        }
      }));
  }

  // Get user's token from local storage
  getToken(): string {
    // Get the logged in user from local storage.
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    // Check if there is a logged in user.
    if (currentUser) {
      // There is a logged in user, so we return the requested token.
      return currentUser.token;
    } else {
      // There is no logged in user, we return null.
      return null;
    }
  }

  // Get user's username from local storage
  getUser(): User {
    // Get the logged in user from local storage.
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    // Check if there is a logged in user.
    if (currentUser) {
      const user: User = currentUser.account;
      // There is a logged in user, so we return the requested username.
      return user;
    }
    else {
      // There is no logged in user, we return null.
      return null;
    }
  }

  userIsLoggedIn(): boolean {
    if (this.getUser() !== null) return true;

    return false;
  }

  logout(): void {
    // remove user's token from local storage to log user out
    localStorage.removeItem('currentUser');
    this.user.emit(null);
  }
}
