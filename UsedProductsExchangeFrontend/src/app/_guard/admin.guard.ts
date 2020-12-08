import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable()
export class AdminGuard implements CanActivate {

  constructor(private router: Router, private authService: AuthenticationService) { }
    canActivate() {
      // Check if user is admin
      if (this.authService.getUser().isAdmin) {
          return true;
      }

      // User is not admin, return to home.
      this.router.navigate(['/']);
      return false;
    }
}
