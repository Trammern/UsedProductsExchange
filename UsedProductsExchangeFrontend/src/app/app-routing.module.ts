import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guard/auth.guard';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryAddComponent } from './categories/category-add/category-add.component';
import { CategoryEditComponent } from './categories/category-edit/category-edit.component';
import {ProductsComponent} from './products/products.component';
import {ProductAddComponent} from './products/product-add/product-add.component';
import {ProductEditComponent} from './products/product-edit/product-edit.component';
import {ProfileComponent} from "./profiles/profile/profile.component";
import {EditProfileComponent} from './profiles/edit-profile/edit-profile.component';
import {AdminGuard} from './_guard/admin.guard';
import {CategoryShowComponent} from './categories/category-show/category-show.component';
import {ProductShowComponent} from './products/product-show/product-show.component';


const routes: Routes = [
  // { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'categories', component: CategoriesComponent },
  { path: 'categories/add', component: CategoryAddComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: 'categories/edit/:id', component: CategoryEditComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: 'categories/:id', component: CategoryShowComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'user/:id', component: ProfileComponent },
  { path: 'user/edit/:id', component: EditProfileComponent, canActivate: [AuthGuard]},
  { path: 'products/add', component: ProductAddComponent, canActivate: [AuthGuard] },
  { path: 'products/edit/:id', component: ProductEditComponent, canActivate: [AuthGuard] },
  { path: 'products/:id', component: ProductShowComponent },


  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
