import { Injectable } from '@angular/core';
import { Team } from '../_models/team';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  baseUrl = environment.apiUrl + 'teams/';

  constructor(private http: HttpClient) {
  }

  getTeams(): Observable<Team[]> {
    return this.http.get<Team[]>(this.baseUrl);
  }

  getTeam(id): Observable<Team> {
    return this.http.get<Team>(this.baseUrl + id);
  }

  deleteTeam(id) {
    return this.http.post(this.baseUrl + 'delete/' + id, {});
  }

  addTeam(team: Team) {
    return this.http.post(this.baseUrl + 'add', team);
  }

  updateTeam(id: any, team: Team) {
    return this.http.put(this.baseUrl + 'update/' + id, team);
  }
}
