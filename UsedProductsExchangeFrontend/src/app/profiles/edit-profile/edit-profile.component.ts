import { Component, OnInit } from '@angular/core';
import {User} from '../../_models/user';
import {AuthenticationService} from '../../_services/authentication.service';
import {UsersService} from '../../_services/users.service';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {catchError, switchMap, take, tap} from 'rxjs/operators';
import {Product} from '../../_models/product.model';
import {Bid} from '../../_models/bid';
import {Observable, of} from 'rxjs';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  updateObservable$: Observable<User[]>;
  submitted = false;
  loading = false;
  userUpdateForm: FormGroup;
  errString: string;
  err: any;
  user: User;

  constructor(private fb: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private userService: UsersService,
              public authenticationService: AuthenticationService) {}

  ngOnInit(): void {
    this.updateObservable$ = this.route.paramMap
      .pipe(
        take(1),
        switchMap(params => {
          if (!(this.authenticationService.getUser().userId === +params.get('id') || this.authenticationService.getUser().isAdmin))
          {
            this.router.navigateByUrl('/');
          }
          this.errString = '';
          const id = +params.get('id');
          return this.userService.getItem(id);
        }),
        tap(user => {
          console.log(user.userId);
          this.userUpdateForm.patchValue(user);
          this.userUpdateForm.patchValue({
            userIdAfter: user.userId
          });
        }),
        catchError(this.err)
      );

    this.userUpdateForm = this.fb.group({
      userId: [''],
      userIdAfter: [''],
      name: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', Validators.required],
      address: [''],
      isAdmin: ['']
    });

  }

  onSubmit(): void {
    this.submitted = true;

    // stop here if form is invalid
    if (this.userUpdateForm.invalid) {
      return;
    }

    this.loading = true;

    const updatedUser = this.userUpdateForm.value;
    updatedUser.userId = updatedUser.userIdAfter;

    console.log(updatedUser.userIdAfter);

    this.userService.update(updatedUser)
      .pipe(
        catchError(err => {
          this.errString = err.error ?? err.message;
          return of();
        })
      )
      .subscribe(user => {
        console.log('user', user);
        // this.router.navigateByUrl('users');
      });
  }
}
