import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { League } from '../_models/league';
import { LeagueService } from '../_services/league.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-league-create-form',
  templateUrl: './league-create-form.component.html',
  styleUrls: ['./league-create-form.component.css']
})
export class LeagueCreateFormComponent implements OnInit {
  leagueForm: FormGroup;
  league: League;

  constructor(private fb: FormBuilder, private leagueService: LeagueService, private alertify: AlertifyService, private router: Router, private authService: AuthService) { }

  ngOnInit() {
    this.authService.throwOutNotLoggedIn();
    this.generateLeagueForm();
  }

  generateLeagueForm() {
    this.leagueForm = this.fb.group({
      name: ['', Validators.required],
      prize: ['', []]
    });
  }

  addLeague() {
    if (this.leagueForm.valid) {
      this.league = Object.assign({}, this.leagueForm.value);

      this.leagueService.addLeague(this.league).subscribe(() => {
        this.alertify.success('League added successfully');
      }, error => {
        this.alertify.error(error);
        }, () => {
          this.router.navigate(['/leagues']);
        });
    }
  }
}
