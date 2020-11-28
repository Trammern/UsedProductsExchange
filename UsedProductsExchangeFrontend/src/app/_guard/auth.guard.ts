import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private authService: AuthenticationService) { }
    canActivate() {
      // Check if user is logged in
      if (this.authService.getToken()) {
          return true;
      }

      // Login failed - Redirect to login
      this.router.navigate(['/login']);
      return false;
    }
}
