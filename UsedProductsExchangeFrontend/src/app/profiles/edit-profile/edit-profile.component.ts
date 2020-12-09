import { Component, OnInit } from '@angular/core';
import {User} from '../../_models/user';
import {AuthenticationService} from '../../_services/authentication.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  user: User;

  constructor(private authenticationService: AuthenticationService) {
    authenticationService.user.subscribe(user => this.setUser(user));
  }

  ngOnInit(): void {
    this.user = this.authenticationService.getUser();
  }

  setUser(user: User): void {
    this.user = user;
  }
}
