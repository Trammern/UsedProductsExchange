import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import {AppRoutingModule} from './app-routing.module';
import {ReactiveFormsModule} from '@angular/forms';
import {AuthGuard} from './_guard/auth.guard';
import {AuthenticationService} from './_services/authentication.service';
import {HttpClientModule} from '@angular/common/http';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { SearchbarComponent } from './searchbar/searchbar.component';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryAddComponent } from './categories/category-add/category-add.component';
import { CategoryEditComponent } from './categories/category-edit/category-edit.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import {CategoriesService} from './_services/categories.service';
import { AdBannerComponent } from './ad-banner/ad-banner.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavigationBarComponent,
    CategoriesComponent,
    CategoryAddComponent,
    CategoryEditComponent,
    HomeComponent,
    RegisterComponent,
    AdBannerComponent,
    SearchbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    AuthGuard,
    AuthenticationService,
    CategoriesService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
