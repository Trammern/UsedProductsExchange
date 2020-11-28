import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
// import {HomeComponent} from './home/home.component';
import { AuthGuard } from './_guard/auth.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  // { path: '', component: HomeComponent, canActivate: [AuthGuard] },

  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
