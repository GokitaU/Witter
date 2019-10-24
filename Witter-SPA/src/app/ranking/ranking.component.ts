import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { User } from '../_models/user';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.css']
})
export class RankingComponent implements OnInit {
  users: User[];

  constructor(private userService: UserService, private alertify: AlertifyService, private authService: AuthService) { }

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this.userService.getUserRanking().subscribe((users: User[]) => {
      this.users = users;
    }, error => {
      this.alertify.error(error);
      });
  }

  isItMe(user: User) {
    return this.authService.getId() == String(user.id);
  }
}
