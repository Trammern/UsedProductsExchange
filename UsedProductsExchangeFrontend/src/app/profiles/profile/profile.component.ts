import { Component, OnInit } from '@angular/core';
import {UsersService} from "../../_services/users.service";
import {User} from "../../_models/user";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  user: User;
  constructor(private userService: UsersService,
              private route: ActivatedRoute) { }

  ngOnInit(): void {

    const id = +this.route.snapshot.paramMap.get('id');
    this.userService.getUser(id).subscribe(userFromApi => {
      this.user = userFromApi;
    });
    const username = this.user.username;
    debugger;
  }

}
