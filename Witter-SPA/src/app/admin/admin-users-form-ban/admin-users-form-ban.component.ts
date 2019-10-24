import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-admin-users-form-ban',
  templateUrl: './admin-users-form-ban.component.html',
  styleUrls: ['./admin-users-form-ban.component.css']
})
export class AdminUsersFormBanComponent implements OnInit {
  user: User;
  bannedUser: User;
  banForm: FormGroup;

  constructor(private authService: AuthService, private userService: UserService, private route: ActivatedRoute, private alertify: AlertifyService, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.authService.throwOutUser();
    this.getUser();
    this.generateBanForm();
  }

  getUser() {
    this.userService.getUser(this.route.snapshot.paramMap.get("id")).subscribe((user: User) => {
      this.user = user;
    }, error => {
      this.alertify.error(error);
      });
  }

  generateBanForm() {
    this.banForm = this.fb.group({
      ban: [{ value: '', disabled: false }],
      permanentBan: ['', []]
    });
  }

  banUser() {
    this.bannedUser = Object.assign({}, this.banForm.value);

    if (this.banForm.get('ban').value == '') {
      this.bannedUser.ban = null;
    }

    if (this.banForm.get('permanentBan').value == '') {
      this.bannedUser.permanentBan = false;
    }

    this.userService.ban(this.route.snapshot.paramMap.get("id"), this.bannedUser).subscribe(() => {
      console.log(this.banForm.value);
      this.alertify.success("Banned user successfully");
    }, error => {
      this.alertify.error(error);
      }, () => {
        this.router.navigate(['/admin/users']);
      });
  }

  permBanSelected() {
    console.log(this.banForm.get('permanentBan').value)
    return this.banForm.get('permanentBan').value == "true" ? true : false;
  }

  toggle() {
    console.log("aaa");
    var control = this.banForm.get('ban')
    control.disabled ? control.enable() : control.disable()
  }

}
