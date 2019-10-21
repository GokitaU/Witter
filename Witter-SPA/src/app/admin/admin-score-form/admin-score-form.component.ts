import { Component, OnInit } from '@angular/core';
import { TeamService } from 'src/app/_services/team.service';
import { Team } from 'src/app/_models/team';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatchService } from 'src/app/_services/match.service';
import { Match } from 'src/app/_models/match';
import { Router, ActivatedRoute } from '@angular/router';
import { Score } from 'src/app/_models/score';

@Component({
  selector: 'app-admin-score-form',
  templateUrl: './admin-score-form.component.html',
  styleUrls: ['./admin-score-form.component.css']
})
export class AdminScoreFormComponent implements OnInit {
  scoreForm: FormGroup;
  match: Match;
  score: Score;

  constructor(private teamService: TeamService, private alertify: AlertifyService, private fb: FormBuilder, private matchService: MatchService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.getMatch(this.route.snapshot.paramMap.get("id"));
  }

  getMatch(id: any) {
    this.matchService.getMatch(id).subscribe((match: Match) => {
      this.match = match;
      this.buildScoreForm();
    }, error => {
      this.alertify.error(error);
    });
  }

  buildScoreForm() {
    this.scoreForm = this.fb.group({
      teamAGoals: ['', Validators.required],
      teamBGoals: ['', Validators.required]
    });
  }

  updateScore() {
    if (this.scoreForm.valid) {
      this.score = Object.assign({}, this.scoreForm.value);

      this.matchService.updateScore(this.route.snapshot.paramMap.get("id"), this.score).subscribe(() => {
        this.alertify.success("Score update successful.")
      }, error => {
        this.alertify.error(error);
        }, () => {
          this.router.navigate(['/admin/matches']);
        });
    }
  }

}
