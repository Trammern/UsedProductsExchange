import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import {AppRoutingModule} from './app-routing.module';
import {ReactiveFormsModule} from '@angular/forms';
import {AuthGuard} from './_guard/auth.guard';
import {AuthenticationService} from './_services/authentication.service';
import {HttpClientModule} from '@angular/common/http';
import { NavigationBarComponent } from './components/navigation-bar/navigation-bar.component';
import { SearchbarComponent } from './components/searchbar/searchbar.component';
import { CategoriesComponent } from './components/categories/categories.component';
import { CategoryAddComponent } from './components/categories/category-add/category-add.component';
import { CategoryEditComponent } from './components/categories/category-edit/category-edit.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import {CategoriesService} from './_services/categories.service';
import { AdBannerComponent } from './components/ad-banner/ad-banner.component';
import { ProductListingComponent } from './components/product-listing/product-listing.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ProfileViewComponent } from './profile-view/profile-view.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavigationBarComponent,
    SearchbarComponent,
    CategoriesComponent,
    CategoryAddComponent,
    CategoryEditComponent,
    HomeComponent,
    RegisterComponent,
    AdBannerComponent,
    SearchbarComponent,
    ProductListingComponent,
    ProductDetailsComponent,
    ProfileViewComponent
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
