import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from '../_models/user';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  user: User;

  constructor(private fb: FormBuilder, private alertify: AlertifyService, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    if (this.authService.loggedIn()) {
      this.router.navigate(['/matches']);
    }

    this.buildRegisterForm();
  }

  buildRegisterForm() {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }, {validator: this.checkPasswordMatch});
  }

  checkPasswordMatch(form: FormGroup) {
    return form.get('password').value === form.get('confirmPassword').value ? null : { 'mismatch': true };
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(() => {
        this.alertify.success("Registration successful!");
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/matches']);
        })
      })
    }
    }
  }
