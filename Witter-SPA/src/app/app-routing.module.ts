import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { MatchesComponent } from './matches/matches.component';
import { AdminMatchesComponent } from './admin/admin-matches/admin-matches.component';
import { AdminMatchesFormComponent } from './admin/admin-matches-form/admin-matches-form.component';
import { AdminMatchesFormEditComponent } from './admin/admin-matches-form-edit/admin-matches-form-edit.component';
import { AdminScoreFormComponent } from './admin/admin-score-form/admin-score-form.component';
import { AdminTeamsComponent } from './admin/admin-teams/admin-teams.component';
import { AdminTeamsFormComponent } from './admin/admin-teams-form/admin-teams-form.component';
import { AdminTeamsFormEditComponent } from './admin/admin-teams-form-edit/admin-teams-form-edit.component';
import { AdminUsersComponent } from './admin/admin-users/admin-users.component';
import { AdminUsersFormBanComponent } from './admin/admin-users-form-ban/admin-users-form-ban.component';
import { RankingComponent } from './ranking/ranking.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserProfileEditComponent } from './user-profile-edit/user-profile-edit.component';


const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'matches', component: MatchesComponent },
  { path: 'ranking', component: RankingComponent },
  { path: 'admin/matches', component: AdminMatchesComponent },
  { path: 'admin/matches/add', component: AdminMatchesFormComponent },
  { path: 'admin/matches/update/:id', component: AdminMatchesFormEditComponent },
  { path: 'admin/matches/score/:id', component: AdminScoreFormComponent },
  { path: 'admin/teams', component: AdminTeamsComponent },
  { path: 'admin/teams/add', component: AdminTeamsFormComponent },
  { path: 'admin/teams/update/:id', component: AdminTeamsFormEditComponent },
  { path: 'admin/users', component: AdminUsersComponent },
  { path: 'admin/users/ban/:id', component: AdminUsersFormBanComponent },
  { path: 'user/:id', component: UserProfileComponent },
  { path: 'user/:id/edit', component: UserProfileEditComponent },
  { path: '**', redirectTo: 'matches', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
