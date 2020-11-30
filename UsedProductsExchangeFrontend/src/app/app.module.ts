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

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavigationBarComponent,
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
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
