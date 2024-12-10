import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { User } from '../../models/user';
import { localStorageAccessConstant } from '../../constants/LocalStorageAccessConstant';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    public currentUser: Observable<User>;

    private headers: HttpHeaders;
    private accessPointUrl: string;
    private currentUserSubject: BehaviorSubject<User>;
    private authenticationAccessPointUrl: string;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string,
        private router: Router
    ) {
        this.headers = new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8',
        });
        this.accessPointUrl = baseUrl + 'api/users';
        this.authenticationAccessPointUrl = baseUrl + 'api/oauth';
        const user: User = JSON.parse(localStorage.getItem('currentUser'));
        this.currentUserSubject = new BehaviorSubject<User>(user);
        this.currentUser = this.currentUserSubject.asObservable();
    }

    get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    login(username: string, password: string, remember: boolean): Observable<any> {
        return this.http
            .post<any>(
                this.accessPointUrl + '/authenticate',
                {
                    password,
                    username,
                    remember,
                },
                {
                    headers: this.headers,
                }
            )
            .pipe(
                map((user) => {
                    // login successful if there's a jwt token in the response
                    if (user) {
                        if (user.fbo != null)
                            user.integrationStatus = user.fbo.integrationStatus;

                        if (user.role === 7)
                            user.integrationStatus = true;

                        localStorage.setItem(
                            localStorageAccessConstant.currentUser,
                            JSON.stringify(user)
                        );
                        localStorage.setItem(
                            localStorageAccessConstant.groupId,
                            JSON.stringify(user.groupId)
                        );

                        this.currentUserSubject.next(user);
                    }

                    return user;
                })
            );
    }

    getAuthToken(token): Observable<any> {
        const tempToken = { accessToken: token };
        return this.http
            .post<any>(
                this.authenticationAccessPointUrl + '/authtoken',
                tempToken
            )
            .pipe(map((authToken) => authToken));
    }

    preAuth(token): Observable<any> {
        const tempUser: User = new User();
        this.currentUserSubject.next(tempUser);

        //call prepare session controller method

        return this.http
            .get<any>(this.accessPointUrl + '/prepare-token-auth', {
                headers: this.headers,
            })
            .pipe(
                map((user) => {
                    // login successful if there's a jwt token in the response
                    if (user) {

                        // store user details and jwt token in local storage to keep user logged in between page refreshes
                        localStorage.setItem(
                            localStorageAccessConstant.currentUser,
                            JSON.stringify(user)
                        );
                        this.currentUserSubject.next(user);
                    }

                    return user;
                })
            );
    }

    postAuth(): Observable<any> {
        return this.http.post(this.accessPointUrl + '/run-login-checks', {
            headers: this.headers,
        });
    }

    logout(): void {
        // remove user from local storage to log user out
        localStorage.removeItem(localStorageAccessConstant.currentUser);
        localStorage.removeItem(localStorageAccessConstant.impersonatedrole);
        localStorage.removeItem(localStorageAccessConstant.fboId);
        localStorage.removeItem(localStorageAccessConstant.managerGroupId);
        localStorage.removeItem(localStorageAccessConstant.groupId);
        localStorage.removeItem(localStorageAccessConstant.conductorFbo);
        localStorage.removeItem(localStorageAccessConstant.icao);
        localStorage.removeItem(localStorageAccessConstant.accountType);
        localStorage.removeItem(localStorageAccessConstant.isNetworkFbo);
        localStorage.removeItem(localStorageAccessConstant.isSingleSourceFbo);

        this.currentUserSubject.next(null);

        this.http.post(this.accessPointUrl + '/logout', {}).subscribe(() => {
            console.log('User logged out');
        });

        this.router.navigate(['/landing-site-layout']);

    }
    public refreshToken() {
        return this.http.post(
            `${this.accessPointUrl}/app-refresh-token`,
            {
                headers: this.headers,
            }
        )
    }
}
