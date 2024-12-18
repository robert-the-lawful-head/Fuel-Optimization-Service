import {
    HttpErrorResponse,
    HttpEvent,
    HttpHandler,
    HttpHeaders,
    HttpInterceptor,
    HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';

import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { User } from '../models/user';
import { AuthenticationService } from '../services/security/authentication.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    private isRefreshing = false; // Flag to prevent multiple refreshes at the same time
    private refreshTokenSubject: BehaviorSubject<any> =
        new BehaviorSubject<any>(null);

    constructor(
        private authenticationService: AuthenticationService
    ) {}

    intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        // add authorization header with jwt token if available
        const currentUser = this.authenticationService.currentUserValue;

        if (currentUser) {
            request = this.addRequiredHeadersToRequest(request);
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
    private addRequiredHeadersToRequest(
        req: HttpRequest<any>
    ): HttpRequest<any> {
        return req.clone({
            withCredentials: true,
            headers: new HttpHeaders({
                'Content-Type': 'application/json; charset=utf-8',
            })
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

            return this.authenticationService.refreshToken().pipe(
                switchMap((newToken: any) => {
                    this.isRefreshing = false;
                    this.refreshTokenSubject.next(newToken);

                    return next.handle(
                        this.addRequiredHeadersToRequest(req)
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
                switchMap((token) => {
                    return next.handle(this.addRequiredHeadersToRequest(req))
                })
            );
        }
    }
}
