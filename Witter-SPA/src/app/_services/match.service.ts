import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Match } from '../_models/match';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Score } from '../_models/score';

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  baseUrl = environment.apiUrl + 'matches/'

  constructor(private http: HttpClient) { }

  getMatch(id: any): Observable<Match> {
    return this.http.get<Match>(this.baseUrl + id);
  }

  getMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(this.baseUrl + 'upcoming');
  }

  getMatchesForAdmin(): Observable<Match[]> {
    return this.http.get<Match[]>(this.baseUrl + 'adminView');
  }

  deleteMatch(id: number) {
    return this.http.post(this.baseUrl + 'delete/' + id, {});
  }

  addMatch(match: Match) {
    return this.http.post(this.baseUrl + 'add', match);
  }

  updateMatch(id: any, match: Match) {
    return this.http.put(this.baseUrl + 'update/' + id, match);
  }

  updateScore(id: any, score: Score) {
    return this.http.put(this.baseUrl + 'score/' + id, score);
  }
}
