import { Component, OnInit } from '@angular/core';
import {AuthenticationService} from './_services/authentication.service';
import {User} from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Used Products Exchange';
  user: User = null;
  
  constructor(private authenticationService: AuthenticationService) {
    authenticationService.user.subscribe(user => this.setUser(user));
  }

  ngOnInit(): void {
    console.log(this.user);
    this.user = this.authenticationService.getUser();
    console.log(this.user);
  }

  setUser(user: User): void {
    this.user = user;
  }

  userIsLoggedIn(): boolean {
    return this.user != null;
  }

  logout(): void {
    this.authenticationService.logout();
  }
}
