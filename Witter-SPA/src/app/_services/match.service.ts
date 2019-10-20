import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Match } from '../_models/match';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  baseUrl = environment.apiUrl + 'matches/'

  constructor(private http: HttpClient) { }

  getMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(this.baseUrl + 'upcoming');
  }
}
