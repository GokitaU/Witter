import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {
  adminUsers: User[];
  bannedUsers: User[];
  notBannedUsers: User[];

  constructor(private userService: UserService, private alertify: AlertifyService, private authService: AuthService) { }

  ngOnInit() {
    this.authService.throwOutUser();
    this.getAdminUsers();
    this.getBannedUsers();
    this.getNotBannedUsers();
  }

  getAdminUsers() {
    this.userService.getAdminUsers().subscribe((adminUsers: User[]) => {
      this.adminUsers = adminUsers;
    }, error => {
      this.alertify.error(error);
      });
  }

  getBannedUsers() {
    this.userService.getBannedUsers().subscribe((users: User[]) => {
      this.bannedUsers = users;
    }, error => {
      this.alertify.error(error);
    });
  }

  getNotBannedUsers() {
    this.userService.getNotBannedUsers().subscribe((users: User[]) => {
      this.notBannedUsers = users;
    }, error => {
      this.alertify.error(error);
    });
  }

  permBan(user: User) {
    return user.permanentBan;
  }

  adminRights(id) {
    this.userService.adminRights(id).subscribe(() => {
      this.alertify.success('Changed admin rights successfully');
      this.ngOnInit();
    }, error => {
      this.alertify.error(error);
      });
  }

  unban(id) {
    this.userService.unban(id).subscribe(() => {
      this.alertify.success('Successfully unbanned user');
      this.ngOnInit();
    }, error => {
      this.alertify.error(error);
      });
  }
}
