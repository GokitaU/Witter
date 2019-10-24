import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + 'admin/';

  constructor(private http: HttpClient) { }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  getAdminUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'users/admin');
  }

  getBannedUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'users/banned');
  }

  getNotBannedUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'users/notbanned');
  }

  adminRights(id) {
    return this.http.put(this.baseUrl + 'rights/' + id, {});
  }

  ban(id: any, user: User) {
    return this.http.put(this.baseUrl + 'ban/' + id, user);
  }

  unban(id) {
    return this.http.put(this.baseUrl + 'unban/' + id, {});
  }
}
