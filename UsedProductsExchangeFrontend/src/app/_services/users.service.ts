import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AuthenticationService} from './authentication.service';
import {Observable} from 'rxjs';
import {Category} from '../_models/category';
import {environment} from '../../environments/environment';
import {User} from '../_models/user';
import {getEntryPointInfo} from '@angular/compiler-cli/ngcc/src/packages/entry_point';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(environment.apiUrl + '/user/' + id);
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(environment.apiUrl);
  }



}





