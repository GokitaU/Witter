import { Component, OnInit } from '@angular/core';
import { TeamService } from 'src/app/_services/team.service';
import { Team } from 'src/app/_models/team';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-admin-teams-form-edit',
  templateUrl: './admin-teams-form-edit.component.html',
  styleUrls: ['./admin-teams-form-edit.component.css']
})
export class AdminTeamsFormEditComponent implements OnInit {
  team: Team;
  teamForm: FormGroup;

  constructor(private teamService: TeamService, private alertify: AlertifyService, private fb: FormBuilder, private router: Router, private authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.authService.throwOutUser();
    this.getTeam();
  }

  getTeam() {
    this.teamService.getTeam(this.route.snapshot.paramMap.get("id")).subscribe((team: Team) => {
      this.team = team;
      this.generateForm();
    }, error => {
      this.alertify.error(error);
      });
  }

  generateForm() {
    this.teamForm = this.fb.group({
      name: [this.team.name, Validators.required],
      coach: [this.team.coach, Validators.required]
    });
  }

    updateTeam() {
      if (this.teamForm.valid) {
        this.team = Object.assign({}, this.teamForm.value);

        this.teamService.updateTeam(this.route.snapshot.paramMap.get("id"), this.team).subscribe(() => {
          this.alertify.success("Updated team successfully");
        }, error => {
          this.alertify.error(error)
        }, () => {
          this.router.navigate(['/admin/teams']);
        });
      }
    }
}
