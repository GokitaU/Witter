import { Component, OnInit } from '@angular/core';
import { TeamService } from 'src/app/_services/team.service';
import { Team } from 'src/app/_models/team';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatchService } from 'src/app/_services/match.service';
import { Match } from 'src/app/_models/match';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-matches-form',
  templateUrl: './admin-matches-form.component.html',
  styleUrls: ['./admin-matches-form.component.css']
})
export class AdminMatchesFormComponent implements OnInit {
  teams: Team[];
  matchForm: FormGroup;
  match: Match;

  constructor(private teamService: TeamService, private alertify: AlertifyService, private fb: FormBuilder, private matchService: MatchService, private router: Router) { }

  ngOnInit() {
    this.getTeams();
    this.buildMatchForm();
  }

  getTeams() {
    this.teamService.getTeams().subscribe((teams: Team[]) => {
      this.teams = teams;
    }, error => {
      this.alertify.error(error);
      });
  }

  buildMatchForm() {
    this.matchForm = this.fb.group({
      teamAId: ["1", Validators.required],
      teamBId: ["2", Validators.required],
      date: ['', Validators.required],
      teamAOdds: ['', Validators.required],
      teamBOdds: ['', Validators.required],
      drawOdds: ['', Validators.required]
    }, { validator: this.customValidator });
  }

  customValidator(form: FormGroup) {
    var errors: { [k: string]: any } = {};

    if (form.get('teamAId').value === form.get('teamBId').value) {
      errors.sameTeam = true;
    }

    if (new Date(form.get('date').value).getTime() < new Date().getTime()) {
      errors.pastDate = true;
    }

    return errors;
  }

  addMatch() {
    if (this.matchForm.valid) {
      this.match = Object.assign({}, this.matchForm.value);
      console.log(this.match)

      this.matchService.addMatch(this.match).subscribe(() => {
        this.alertify.success("Added match successfully");
      }, error => {
          this.alertify.error(error)
        }, () => {
          this.router.navigate(['/admin/matches']);
        });
    }
  }
}
