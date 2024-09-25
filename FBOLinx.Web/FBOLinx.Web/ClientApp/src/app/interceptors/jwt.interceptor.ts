import {
    HttpErrorResponse,
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';

import { AuthenticationService } from '../services/authentication.service';
import { localStorageAccessConstant } from '../constants/LocalStorageAccessConstant';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { OAuthService } from '../services/oauth.service';
import { User } from '../models/user';
import { ExchangeRefreshTokenResponse } from '../models/authorization/tokenRefres';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    private isRefreshing = false; // Flag to prevent multiple refreshes at the same time
    private refreshTokenSubject: BehaviorSubject<any> =
        new BehaviorSubject<any>(null);

    constructor(
        private authenticationService: AuthenticationService,
        private oAuthService: OAuthService
    ) {}

    intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        // add authorization header with jwt token if available
        const currentUser = this.authenticationService.currentUserValue;

        if (currentUser && currentUser.token) {
            request = this.addTokenToRequest(request, currentUser.token);
        }

        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === 401 && currentUser.remember) {
                    return this.handle401Error(request, next, currentUser);
                }
                return throwError(() => error);
            })
        );
    }
    // Attach the token to the request
    private addTokenToRequest(
        req: HttpRequest<any>,
        token: string
    ): HttpRequest<any> {
        return req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`,
            },
        });
    }
    // Handle 401 errors and refresh the token if necessary
    private handle401Error(
        req: HttpRequest<any>,
        next: HttpHandler,
        user: User
    ): Observable<HttpEvent<any>> {
        if (!this.isRefreshing) {
            this.isRefreshing = true;
            this.refreshTokenSubject.next(null);

            return this.oAuthService.refreshToken(user).pipe(
                switchMap((newToken: any) => {
                    this.isRefreshing = false;
                    this.updateUserTokens(user, newToken);
                    this.refreshTokenSubject.next(newToken);

                    return next.handle(
                        this.addTokenToRequest(req, newToken.authToken)
                    );
                }),
                catchError((error) => {
                    this.isRefreshing = false;
                    return throwError(() => error);
                })
            );
        } else {
            return this.refreshTokenSubject.pipe(
                filter((token: any) => token != null),
                take(1),
                switchMap((token) =>
                    next.handle(this.addTokenToRequest(req, token.authToken))
                )
            );
        }
    }
    private updateUserTokens(user: User, tokens: ExchangeRefreshTokenResponse) {
        user.token = tokens.authToken;
        user.refreshToken = tokens.refreshToken;
        user.tokenExpirationDate = tokens.authTokenExpiration;
        user.refreshTokenExpirationDate = tokens.refreshTokenExpiration;

        localStorage.setItem(
            localStorageAccessConstant.currentUser,
            JSON.stringify(user)
        );
    }
}
