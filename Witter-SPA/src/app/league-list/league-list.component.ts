import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { LeagueService } from '../_services/league.service';
import { AlertifyService } from '../_services/alertify.service';
import { League } from '../_models/league';

@Component({
  selector: 'app-league-list',
  templateUrl: './league-list.component.html',
  styleUrls: ['./league-list.component.css']
})
export class LeagueListComponent implements OnInit {
  leagues: League[];
  leaguesWithUser: League[];
  leaguesWithoutUser: League[];

  constructor(private authService: AuthService, private leagueService: LeagueService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.authService.throwOutNotLoggedIn();

    if (this.authService.loggedIn()) {
      this.getLeaguesWithUser();
      this.getLeaguesWithoutUser();
    }
    else {
      this.getLeagues();
    }
  }

  getLeagues() {
    this.leagueService.getLeagues().subscribe((leagues: League[]) => {
      this.leagues = leagues;
    }, error => {
      this.alertify.error(error);
      });
  }

  getLeaguesWithUser() {
    this.leagueService.getLeaguesByUser(this.authService.getId()).subscribe((leagues: League[]) => {
      this.leaguesWithUser = leagues;
    }, error => {
      this.alertify.error(error);
    });
  }

  getLeaguesWithoutUser() {
    this.leagueService.getLeaguesWithoutUser(this.authService.getId()).subscribe((leagues: League[]) => {
      this.leaguesWithoutUser = leagues;
      console.log(leagues);
    }, error => {
      this.alertify.error(error);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

}
