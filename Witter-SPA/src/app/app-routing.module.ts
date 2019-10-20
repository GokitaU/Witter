import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { MatchesComponent } from './matches/matches.component';


const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'matches', component: MatchesComponent },
  { path: '**', redirectTo: 'matches', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
