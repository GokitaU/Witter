import { Component, OnInit } from '@angular/core';
import { TeamService } from 'src/app/_services/team.service';
import { Team } from 'src/app/_models/team';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-admin-teams-form',
  templateUrl: './admin-teams-form.component.html',
  styleUrls: ['./admin-teams-form.component.css']
})
export class AdminTeamsFormComponent implements OnInit {
  teamForm: FormGroup;
  team: Team;

  constructor(private teamService: TeamService, private alertify: AlertifyService, private fb: FormBuilder, private router: Router, private authService: AuthService) { }

  ngOnInit() {
    this.authService.throwOutUser();
    this.generateForm();
  }

  generateForm() {
    this.teamForm = this.fb.group({
      name: ['', Validators.required],
      coach: ['', Validators.required]
    });
  }

  addTeam() {
    if (this.teamForm.valid) {
      this.team = Object.assign({}, this.teamForm.value);

      this.teamService.addTeam(this.team).subscribe(() => {
        this.alertify.success("Added team successfully");
      }, error => {
        this.alertify.error(error)
        }, () => {
          this.router.navigate(['/admin/teams']);
        });
    }
  }

}
