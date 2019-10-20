import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Bet } from '../_models/bet';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BetService {
  baseUrl = environment.apiUrl + 'bet/';

  constructor(private http: HttpClient) {
  }

  placeBet(bet: Bet) {
    console.log(bet);
    return this.http.post(this.baseUrl + 'place/', bet);
  }
}
