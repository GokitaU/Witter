import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from '../_models/user';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-profile-edit',
  templateUrl: './user-profile-edit.component.html',
  styleUrls: ['./user-profile-edit.component.css']
})
export class UserProfileEditComponent implements OnInit {
  passForm: FormGroup;
  user: User;

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService, private alertify: AlertifyService, private fb: FormBuilder, private userService: UserService) { }

  ngOnInit() {
    this.throwOutUser();
    this.generatePassForm();
  }

  throwOutUser() {
    if (this.route.snapshot.paramMap.get("id") != this.authService.getId()) {
      this.alertify.error('You shall not pass');
      this.router.navigate(['/user/' + this.route.snapshot.paramMap.get("id")]);
    }
  }

  generatePassForm() {
    this.passForm = this.fb.group({
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    }, { validator: this.checkPasswordMatch });
  }

  checkPasswordMatch(form: FormGroup) {
    return form.get('password').value === form.get('confirmPassword').value ? null : { 'mismatch': true };
  }

  return() {
    this.router.navigate(['/user/' + this.route.snapshot.paramMap.get("id")]);
  }

  updatePass() {
    if (this.passForm.valid) {
      this.user = Object.assign({}, this.passForm.value);

      this.userService.updatePass(this.route.snapshot.paramMap.get("id"), this.user).subscribe(() => {
        this.alertify.success('Updated password successfully');
        this.router.navigate(['/user/' + this.route.snapshot.paramMap.get("id")]);
      }, error => {
        this.alertify.error(error);
        });
    }
  }

}
