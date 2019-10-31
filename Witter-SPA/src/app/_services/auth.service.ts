import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { AlertifyService } from './alertify.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService;
  decodedToken: any;

  constructor(private http: HttpClient, private router: Router, private alertify: AlertifyService, private userService: UserService) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {
          const user = response;

          if (user) {
            localStorage.setItem('token', user.token);
            localStorage.setItem('role', user.siteRole);
            localStorage.setItem('user', user.userToReturn.id);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);

            this.autoUnban(user.userToReturn);
          }
        })
      );
  }

  autoUnban(user: User) {
    if (user.ban != null) {
      if (new Date(user.ban).getTime() < new Date().getTime()) {
        this.userService.unban(user.id);
      }
    }
  }

  register(user: User) {
    return this.http.post(this.baseUrl + 'register', user);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  getRole() {
    return localStorage.getItem('role');
  }

  getId() {
    return localStorage.getItem('user');
  }

  throwOutUser() {
    if (this.getRole() != "Admin") {
      this.alertify.error('You are not allowed to access this page!');
      this.router.navigate(['/matches']);
    }
  }

  throwOutNotLoggedIn() {
    if (!this.loggedIn()) {
      this.alertify.error('You are not allowed to access this page!');
      this.router.navigate(['/matches']);
    }
  }
}
