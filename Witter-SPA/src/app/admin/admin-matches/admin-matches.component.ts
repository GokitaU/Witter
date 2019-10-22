import { Component, OnInit } from '@angular/core';
import { MatchService } from 'src/app/_services/match.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Match } from 'src/app/_models/match';
import { Router } from '@angular/router';

import * as alertify from 'alertifyjs';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-admin-matches',
  templateUrl: './admin-matches.component.html',
  styleUrls: ['./admin-matches.component.css']
})
export class AdminMatchesComponent implements OnInit {
  matches: Match[];

  constructor(private matchService: MatchService, private alertify: AlertifyService, private router: Router, private authService: AuthService) { }

  ngOnInit() {
    this.authService.throwOutUser();
    this.loadMatches();
  }

  loadMatches() {
    this.matchService.getMatchesForAdmin().subscribe((matches: Match[]) => {
      this.matches = matches;
    }, error => {
      this.alertify.error(error);
    });
  }

  checkDateTime(match: Match) {
    var date = new Date();
    var matchDate = new Date(match.date);

    if (date.getTime() > matchDate.getTime()) {
      return true;
    }
    return false;
  }

  deleteMatch(id: number) {
        this.matchService.deleteMatch(id).subscribe(() => {
          this.alertify.success("Deleted match successfully.");
          this.ngOnInit();
        }, error => {
          this.alertify.error(error);
        });
  }

}
