import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Bet } from '../_models/bet';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

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

  getBetsByUser(userId, page?): Observable<PaginatedResult<Bet[]>> {
    const paginatedResult: PaginatedResult<Bet[]> = new PaginatedResult<Bet[]>();
    let params = new HttpParams();

    if (page != null) {
      params = params.append('pageNumber', page);
    }

    return this.http.get<Bet[]>(this.baseUrl + 'user/' + userId, { observe: 'response', params })
      .pipe(
      map(response => {
        paginatedResult.result = response.body;

        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }

        return paginatedResult;

        })
      );
  }

  getPastBetsByUser(userId, page?): Observable<PaginatedResult<Bet[]>> {
    const paginatedResult: PaginatedResult<Bet[]> = new PaginatedResult<Bet[]>();
    let params = new HttpParams();

    if (page != null) {
      params = params.append('pageNumber', page);
    }

    return this.http.get<Bet[]>(this.baseUrl + 'user/' + userId + '/past', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;

          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }

          return paginatedResult;

        })
      );
  }
}
