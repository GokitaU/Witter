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
}
