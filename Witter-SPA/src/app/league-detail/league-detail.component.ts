import { Component, OnInit } from '@angular/core';
import { League } from '../_models/league';
import { LeagueService } from '../_services/league.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/user';

@Component({
  selector: 'app-league-detail',
  templateUrl: './league-detail.component.html',
  styleUrls: ['./league-detail.component.css']
})
export class LeagueDetailComponent implements OnInit {
  league: League;
  ranking: User[];

  constructor(private leagueService: LeagueService, private alertify: AlertifyService, private route: ActivatedRoute, private authService: AuthService) { }

  ngOnInit() {
    this.authService.throwOutNotLoggedIn();

    this.getLeague();
    this.getRanking();
  }

  getLeague() {
    this.leagueService.getLeague(this.route.snapshot.paramMap.get('id')).subscribe((league: League) => {
      this.league = league;
    }, error => {
      this.alertify.error(error);
      });
  }

  getRanking() {
    this.leagueService.getRanking(this.route.snapshot.paramMap.get('id')).subscribe((ranking: User[]) => {
      this.ranking = ranking;
    }, error => {
      this.alertify.error(error);
      });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  isItMe(user: User) {
    return this.authService.getId() == String(user.id);
  }
}
