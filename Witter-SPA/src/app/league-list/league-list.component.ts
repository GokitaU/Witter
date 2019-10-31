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
  leaguesWithUser: League[];
  leaguesWithoutUser: League[];

  constructor(private authService: AuthService, private leagueService: LeagueService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.authService.throwOutNotLoggedIn();

      this.getLeaguesWithUser();
      this.getLeaguesWithoutUser();
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
    }, error => {
      this.alertify.error(error);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  joinLeague(leagueId) {
    this.leagueService.joinLeague(leagueId).subscribe(() => {
      this.alertify.success('Joined league successfully');
      this.ngOnInit();
    }, error => {
      this.alertify.error(error);
      });
  }

  leaveLeague(leagueId) {
    this.leagueService.leaveLeague(leagueId).subscribe(() => {
      this.alertify.success('Left league successfully');
      this.ngOnInit();
    }, error => {
      this.alertify.error(error);
      });
  }

  deleteLeague(leagueId) {
    this.leagueService.deleteLeague(leagueId).subscribe(() => {
      this.alertify.success('Deleted league successfully');
      this.ngOnInit();
    }, error => {
      this.alertify.error(error);
      });
  }

  amIAdmin(league: League) {
    return this.authService.getId() == league.admin.id.toString();
  }

}
