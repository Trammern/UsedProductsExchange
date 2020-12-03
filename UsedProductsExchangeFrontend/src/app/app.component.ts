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
  profileOpened = false;

  constructor(private authenticationService: AuthenticationService) {
    authenticationService.user.subscribe(user => this.setUser(user));
  }

  ngOnInit(): void {
    this.user = this.authenticationService.getUser();
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

  acceptCookies(): void {
    localStorage.setItem('acceptCookies', JSON.stringify({ accepted: true }));
  }

  getCookieStatus(): boolean {
    const cookieStatus = JSON.parse(localStorage.getItem('acceptCookies'));
    if (cookieStatus) {
      return cookieStatus.accepted;
    } else {
      return null;
    }
  }

  toggleProfile(): void {
    this.profileOpened = !this.profileOpened;
  }
}
