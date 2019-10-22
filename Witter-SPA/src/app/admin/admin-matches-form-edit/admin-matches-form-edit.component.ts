import { Component, OnInit } from '@angular/core';
import { TeamService } from 'src/app/_services/team.service';
import { Team } from 'src/app/_models/team';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatchService } from 'src/app/_services/match.service';
import { Match } from 'src/app/_models/match';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-admin-matches-form-edit',
  templateUrl: './admin-matches-form-edit.component.html',
  styleUrls: ['./admin-matches-form-edit.component.css']
})
export class AdminMatchesFormEditComponent implements OnInit {
  teams: Team[];
  matchForm: FormGroup;
  match: Match;

  constructor(private teamService: TeamService, private alertify: AlertifyService, private fb: FormBuilder, private authService: AuthService, private matchService: MatchService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.authService.throwOutUser();
    this.getTeams();
    this.getMatch(this.route.snapshot.paramMap.get("id"));
  }

  getMatch(id: any) {
    this.matchService.getMatch(id).subscribe((match: Match) => {
      this.match = match;
      this.buildMatchForm();
    }, error => {
      this.alertify.error(error);
      });
  }

  getTeams() {
    this.teamService.getTeams().subscribe((teams: Team[]) => {
      this.teams = teams;
    }, error => {
      this.alertify.error(error);
    });
  }

  buildMatchForm() {
    console.log(this.match)
    this.matchForm = this.fb.group({
      teamAId: [this.match.teamA.id, Validators.required],
      teamBId: [this.match.teamB.id, Validators.required],
      date: [this.match.date, Validators.required],
      teamAOdds: [this.match.teamAOdds, Validators.required],
      teamBOdds: [this.match.teamBOdds, Validators.required],
      drawOdds: [this.match.drawOdds, Validators.required]
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

      this.matchService.updateMatch(this.route.snapshot.paramMap.get("id"), this.match).subscribe(() => {
        this.alertify.success("Updated match successfully");
      }, error => {
        this.alertify.error(error)
      }, () => {
        this.router.navigate(['/admin/matches']);
      });
    }
  }
}
