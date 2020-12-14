import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AppRoutingModule } from './app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthGuard } from './_guard/auth.guard';
import { AuthenticationService } from './_services/authentication.service';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryAddComponent } from './categories/category-add/category-add.component';
import { CategoryEditComponent } from './categories/category-edit/category-edit.component';
import { RegisterComponent } from './register/register.component';
import { CategoriesService } from './_services/categories.service';
import { HomeComponent } from './home/home.component';
import { ProductsService } from './_services/products.service';
import { ProductsComponent } from './products/products.component';
import { ProductAddComponent } from './products/product-add/product-add.component';
import { ProductEditComponent } from './products/product-edit/product-edit.component';
import {AuthInterceptor} from './_interceptors/auth.interceptor';
import { ProfileComponent } from './profiles/profile/profile.component';
import { CategoryShowComponent } from './categories/category-show/category-show.component';
import { ProductShowComponent } from './products/product-show/product-show.component';
import {AdminGuard} from './_guard/admin.guard';
import { UploadComponent } from './_components/upload/upload.component';
import { EditProfileComponent } from './profiles/edit-profile/edit-profile.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    CategoriesComponent,
    CategoryAddComponent,
    CategoryEditComponent,
    ProductsComponent,
    ProductAddComponent,
    ProductEditComponent,
    ProfileComponent,
    EditProfileComponent,
    CategoryShowComponent,
    ProductShowComponent,
    UploadComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    AuthGuard,
    AdminGuard,
    AuthenticationService,
    CategoriesService,
    ProductsService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
