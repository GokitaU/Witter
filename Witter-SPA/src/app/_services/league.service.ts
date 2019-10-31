import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { League } from '../_models/league';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class LeagueService {
  baseUrl = environment.apiUrl + 'leagues/'

  constructor(private http: HttpClient) { }

  getLeagues(): Observable<League[]> {
    return this.http.get<League[]>(this.baseUrl);
  }

  getLeaguesByUser(id): Observable<League[]> {
    return this.http.get<League[]>(this.baseUrl + 'user/' + id);
  }

  getLeaguesWithoutUser(id): Observable<League[]> {
    return this.http.get<League[]>(this.baseUrl + 'user/not/' + id);
  }

  getLeague(id): Observable<League> {
    return this.http.get<League>(this.baseUrl + id);
  }

  getRanking(id): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + id + '/rank');
  }
}
