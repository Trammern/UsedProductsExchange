import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthenticationService} from '../_services/authentication.service';
import {User} from '../_models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  submitted = false;
  loading = false;
  errormessage = '';

  constructor(private formBuilder: FormBuilder, private router: Router, private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    //  Initialize the form group
    this.registerForm = this.formBuilder.group({
      name: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', Validators.required],
      address: ['', Validators.required],
      email: ['', Validators.required],
    });

    // reset login status
    this.authenticationService.logout();
  }

  // Getters for easy access to form fields
  get name() { return this.registerForm.get('name'); }
  get username() { return this.registerForm.get('username'); }
  get password() { return this.registerForm.get('password'); }
  get address() { return this.registerForm.get('address'); }
  get email() { return this.registerForm.get('email'); }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }

    this.loading = true;

    const user: { Password: string; Address: string; Name: string; Email: string; Username: string } = {
      Name: this.name.value,
      Username: this.username.value,
      Password: this.password.value,
      Address: this.address.value,
      Email: this.email.value,
    };
    this.authenticationService.register(this.name.value, this.username.value, this.password.value, this.address.value, this.email.value)
      .subscribe(
        success => {
          this.router.navigate(['/products']);
        },
        error => {
          this.errormessage = error.error;
          this.loading = false;
        });
  }

}
