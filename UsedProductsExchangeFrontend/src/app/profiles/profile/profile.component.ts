import { Component, OnInit } from '@angular/core';
import {UsersService} from "../../_services/users.service";
import {User} from "../../_models/user";
import {ActivatedRoute} from "@angular/router";
import {AppComponent} from "../../app.component";
import {Product} from '../../_models/product';
import {Bid} from "../../_models/bid";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  user: User;
  products: Product[];
  bids: Bid[];

  constructor(private userService: UsersService,
              private route: ActivatedRoute,
              private loginStatus: AppComponent) {
  }

  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.userService.getUser(id).subscribe(userFromApi => {
      this.user = userFromApi;
      this.products = userFromApi.products;
      this.bids = userFromApi.bids;
    });
  }

  isLoggedIn(){
    if (this.user.userId === this.loginStatus.user.userId)
    {
      return true;
    }
  }

}
