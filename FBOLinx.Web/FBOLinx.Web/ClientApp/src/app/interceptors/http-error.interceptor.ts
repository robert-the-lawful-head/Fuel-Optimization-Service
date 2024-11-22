import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SnackBarService } from '../services/utils/snackBar.service';
import { AppService } from '../services/app.service';
import { AuthenticationService } from '../services/security/authentication.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  private errorSnackBarDuration: number = 4000;
  private refreshtokenUrl: string = 'app-refresh-token';
  private logTitle: string = 'HttpErrorInterceptor';
  
  constructor(private snackbarService: SnackBarService,
    private authenticationService: AuthenticationService,
    private appService: AppService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        this.appService.logData(this.logTitle,JSON.stringify(error)).subscribe();
        
        if (error.status === 401) {
          if(!this.authenticationService.currentUserValue.remember || error.url.includes(this.refreshtokenUrl)){
            this.authenticationService.logout();
            return;
          }
          return throwError(error);
        }

        let displayError = error.error?.message ?? 'An unexpected error occurred, try reloading the page. If the problem persists, please contact support.';

        if(error.error.message != "Username or password is incorrect"){
          displayError = error.error.message;
        }

        if (error.status === 403) {
          displayError = 'You are not authorized to access this resource';
        } else if (error.status === 404) {
          displayError = 'The resource you are looking for is not found';
        }

        console.error(error);
        this.snackbarService.showErrorSnackBar(displayError,this.errorSnackBarDuration);
        return throwError(displayError);
      })
    );
  }
}
