import { Component, OnInit } from '@angular/core';
import { Match } from '../_models/match';
import { MatchService } from '../_services/match.service';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-matches',
  templateUrl: './matches.component.html',
  styleUrls: ['./matches.component.css']
})
export class MatchesComponent implements OnInit {
  matches: Match[];

  constructor(private matchService: MatchService, private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadMatches();
  }

  loadMatches() {
    this.matchService.getMatches().subscribe((matches: Match[]) => {
      this.matches = matches;
      console.log(matches);
    }, error => {
      this.alertify.error(error);
      });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  isBanned() {
    return this.authService.getRole() === "Banned" ? true : false;
  }
}
