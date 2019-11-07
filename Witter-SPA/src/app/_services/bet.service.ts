import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Bet } from '../_models/bet';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BetService {
  baseUrl = environment.apiUrl + 'bet/';

  constructor(private http: HttpClient) {
  }

  placeBet(bet: Bet) {
    return this.http.post(this.baseUrl + 'place/', bet);
  }

  getBet(matchId: number): Observable<Bet> {
    return this.http.get<Bet>(this.baseUrl + 'user/match/' + matchId);
  }

  getBetsByUser(userId): Observable<Bet[]> {
    return this.http.get<Bet[]>(this.baseUrl + 'user/' + userId);
  }

  getPastBetsByUser(userId): Observable<Bet[]> {
    return this.http.get<Bet[]>(this.baseUrl + 'user/' + userId + '/past');
  }
}
