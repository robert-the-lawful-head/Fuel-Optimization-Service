import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../models/user';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  public currentUser: Observable<User>;

  private headers: HttpHeaders;
  private accessPointUrl: string;
  private currentUserSubject: BehaviorSubject<User>;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8',
    });
    this.accessPointUrl = baseUrl + 'api/users';
    const user: User = JSON.parse(localStorage.getItem('currentUser'));
    this.currentUserSubject = new BehaviorSubject<User>(user);
    this.currentUser = this.currentUserSubject.asObservable();
  }

  get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(username: string, password: string, remember = false) {
    return this.http
      .post<any>(
        this.accessPointUrl + '/authenticate', {
          username,
          password
        }, {
          headers: this.headers
        }
      )
      .pipe(
        map((user) => {
          // login successful if there's a jwt token in the response
          if (user && user.token) {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem(
              'currentUser',
              JSON.stringify(user)
            );
            this.currentUserSubject.next(user);
          }

          return user;
        })
      );
  }

  preAuth(token) {
    const tempUser = {
      oid: 0,
      username: '',
      password: '',
      firstName: '',
      lastName: '',
      token,
      role: 0,
      fboId: 0,
      groupId: 0,
      impersonatedRole: null,
      managerGroupId: 0
    };
    this.currentUserSubject.next(tempUser);
    return this.http
      .get<any>(this.accessPointUrl + '/current', {
        headers: this.headers,
      })
      .pipe(
        map((user) => {
          // login successful if there's a jwt token in the response
          if (user) {
            user.token = token;
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem(
              'currentUser',
              JSON.stringify(user)
            );
            this.currentUserSubject.next(user);
          }

          return user;
        })
      );
  }

  postAuth() {
    return this.http.post(this.accessPointUrl + '/run-login-checks', {
      headers: this.headers,
    });
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
