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
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  private errorSnackBarDuration: number = 4000;
  constructor(private snackbarService: SnackBarService,private authenticationService: AuthenticationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {

        if (error.status === 401) {
          if(!this.authenticationService.currentUserValue.remember){
            this.authenticationService.logout();
            location.reload();
            return;
          }
          return throwError(error);
        }

        let displayError = error.error?.message ?? 'An unexpected error occurred, try reloading the page, if the problem persists contact support';

        if(error.error.message != "Username or password is incorrect"){
          displayError = error.error.message;
        }

        if (error.status === 403) {
          displayError = 'You are not authorized to access this resource';
        } else if (error.status === 404) {
          displayError = 'The resource you are looking for is not found';
        }

        console.error(displayError);
        this.snackbarService.showErrorSnackBar(displayError,this.errorSnackBarDuration);
        return throwError(displayError);
      })
    );
  }
}
