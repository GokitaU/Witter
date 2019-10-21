import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { MatchesComponent } from './matches/matches.component';
import { AdminMatchesComponent } from './admin/admin-matches/admin-matches.component';
import { AdminMatchesFormComponent } from './admin/admin-matches-form/admin-matches-form.component';
import { AdminMatchesFormEditComponent } from './admin/admin-matches-form-edit/admin-matches-form-edit.component';
import { AdminScoreFormComponent } from './admin/admin-score-form/admin-score-form.component';


const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'matches', component: MatchesComponent },
  { path: 'admin/matches', component: AdminMatchesComponent },
  { path: 'admin/matches/add', component: AdminMatchesFormComponent },
  { path: 'admin/matches/update/:id', component: AdminMatchesFormEditComponent },
  { path: 'admin/matches/score/:id', component: AdminScoreFormComponent },
  { path: '**', redirectTo: 'matches', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
