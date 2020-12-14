import { Component, OnInit } from '@angular/core';
import {User} from '../../_models/user';
import {AuthenticationService} from '../../_services/authentication.service';
import {UsersService} from '../../_services/users.service';
import {ActivatedRoute, Router} from '@angular/router';
import {AppComponent} from '../../app.component';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {catchError, first, switchMap, take, tap} from 'rxjs/operators';
import {Product} from '../../_models/product';
import {Bid} from '../../_models/bid';
import {ProductsService} from '../../_services/products.service';
import {Observable, of, pipe} from 'rxjs';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {

  currentUser: User;

  userUpdateForm: FormGroup;
  updateObservable$: Observable<any>;
  errString: string;
  err: any;
  products: Product[];
  bids: Bid[];
  user: User;

  constructor(private fb: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private userService: UsersService,
              private productService: ProductsService,
              private authenticationService: AuthenticationService) {
  }

  ngOnInit(): void {

    this.user = this.authenticationService.getUser()



    this.userUpdateForm = this.fb.group({
      name: [this.user.name],
      username:[this.user.username],
      email: [this.user.email],
      address:[this.user.address]
    });

    }

  update() {

    this.errString = '';
    this.user.name = this.userUpdateForm.get('name').value;
    this.user.address = this.userUpdateForm.get('address').value;
    this.user.email = this.userUpdateForm.get('email').value;
    this.user.username = this.userUpdateForm.get('username').value;
    console.log('I was pressed');


    this.userService.put(this.user).pipe(
      catchError(err => {
        this.errString = err.error ?? err.message;
        return of()
      })
    )
      .subscribe(user => {
        console.log('user' ,user);
      });
  }
}
