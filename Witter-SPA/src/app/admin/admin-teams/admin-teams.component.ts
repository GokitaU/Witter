import { Component, OnInit } from '@angular/core';
import { TeamService } from 'src/app/_services/team.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { Team } from 'src/app/_models/team';

@Component({
  selector: 'app-admin-teams',
  templateUrl: './admin-teams.component.html',
  styleUrls: ['./admin-teams.component.css']
})
export class AdminTeamsComponent implements OnInit {
  teams: Team[];

  constructor(private teamService: TeamService, private alertify: AlertifyService, private authService: AuthService) { }

  ngOnInit() {
    this.authService.throwOutUser();
    this.getTeams();
  }

  getTeams() {
    this.teamService.getTeams().subscribe((teams: Team[]) => {
      this.teams = teams;
    }, error => {
      this.alertify.error(error);
      });
  }

  deleteTeam(id: any) {
    this.teamService.deleteTeam(id).subscribe(() => {
      this.alertify.success("Team deleted successfully");
      this.ngOnInit();
    }, error => {
      this.alertify.error(error);
      });
  }

}
