import { Component, OnInit } from '@angular/core';
import {User} from '../../_models/user';
import {AuthenticationService} from '../../_services/authentication.service';
import {UsersService} from '../../_services/users.service';
import {ActivatedRoute} from '@angular/router';
import {AppComponent} from '../../app.component';
import {FormControl, FormGroup} from '@angular/forms';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {

  user:User;
  updatedUser: User;
  profileForm = new FormGroup({
  name: new FormControl(''),
  address: new FormControl(''),
  username: new FormControl(''),
  email: new FormControl('')
});

  constructor(private userService: UsersService,
              private route: ActivatedRoute,
              private loginStatus: AppComponent) {
    this.user = new User();
  }

  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.userService.getUser(id).subscribe(userFromApi => {
    this.user = userFromApi;
    });
  }

  setUser(user: User): void {
    this.user = user;
  }

  put() {

    this.user.name = this.profileForm.get('name').value;
    this.user.username = this.profileForm.get('username').value;
    this.user.address = this.profileForm.get('address').value;
    this.user.email = this.profileForm.get('email').value;


    this.userService.put(this.user).subscribe();

  }
}
