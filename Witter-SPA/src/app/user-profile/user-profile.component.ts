import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { Bet } from '../_models/bet';
import { BetService } from '../_services/bet.service';
import { Match } from '../_models/match';
import { AuthService } from '../_services/auth.service';
import { League } from '../_models/league';
import { LeagueService } from '../_services/league.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  user: User;
  bets: Bet[];
  leaguesWithUser: League[];

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute, private leagueService: LeagueService, private betService: BetService, private authService: AuthService) { }

  ngOnInit() {
    this.getUser();
    this.getBets();
    this.getLeaguesWithUser();
  }

  getUser() {
    this.userService.getUser(this.route.snapshot.paramMap.get("id")).subscribe((user: User) => {
      this.user = user;
    }, error => {
      this.alertify.error(error);
    });
  }

  isItMe() {
    return this.route.snapshot.paramMap.get("id") == this.authService.getId();
  }

  getBets() {
    this.betService.getBetsByUser(this.route.snapshot.paramMap.get("id")).subscribe((bets: Bet[]) => {
      this.bets = bets;
    }, error => {
      this.alertify.error(error);
    });
  }

  getLeaguesWithUser() {
    this.leagueService.getLeaguesByUser(this.route.snapshot.paramMap.get("id")).subscribe((leagues: League[]) => {
      this.leaguesWithUser = leagues;
    }, error => {
      this.alertify.error(error);
    });
  }

  matchPast(match: Match) {
    if (new Date(match.date).getTime() < new Date().getTime()) {
      return true;
    }

    return false;
  }

  isScoreUpdated(bet: Bet) {
    return bet.match.score != null;
  }

  isBetWon(bet: Bet) {
    if (bet.prediction == 0 && bet.match.score.teamAGoals == bet.match.score.teamBGoals) {
      return true;
    }

    if (bet.prediction == 1 && bet.match.score.teamAGoals > bet.match.score.teamBGoals) {
      return true;
    }

    if (bet.prediction == 2 && bet.match.score.teamAGoals < bet.match.score.teamBGoals) {
      return true;
    }

    return false;
  }

}
