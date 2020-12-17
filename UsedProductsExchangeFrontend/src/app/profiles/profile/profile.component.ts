import { Component, OnInit } from '@angular/core';
import {User} from '../../_models/user';
import {ActivatedRoute} from '@angular/router';
import {Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {UsersService} from '../../_services/users.service';
import { formatCurrency } from '@angular/common';
import {AuthenticationService} from '../../_services/authentication.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  err: any;
  chosenUser$: Observable<User>;

  constructor(private usersService: UsersService,
              private activeRoute: ActivatedRoute,
              public authenticationsService: AuthenticationService) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe(routeParams => {
      this.chosenUser$ = this.usersService.getItem(routeParams.id);
      console.log(this.chosenUser$);
    });
  }

  public createImgPath = (serverPath: string) => {
    return environment.url + '/' + serverPath;
  }
  formatCurrency(price: number, locale: string, currency: string): string {
    return formatCurrency(price, locale, currency);
  }

}
