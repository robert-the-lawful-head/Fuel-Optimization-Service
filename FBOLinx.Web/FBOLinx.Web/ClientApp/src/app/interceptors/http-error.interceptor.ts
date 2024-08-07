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

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  private errorSnackBarDuration: number = 4000;
  constructor(private snackbarService: SnackBarService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        console.log("🚀 ~ HttpErrorInterceptor ~ catchError ~ error:", error)
        let errorMessage = '';
        let displayError = 'An unexpected error occurred, try reloading the page, if the problem persists contact support';
        if (error.error instanceof ErrorEvent) {
          errorMessage = `Error: ${error.error.message}`;
        } else {
          errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }
        if (error.status === 401 || error.status === 403) {
          displayError = 'You are not authorized to access this resource';
        } else if (error.status === 404) {
          displayError = 'The resource you are looking for is not found';
        }

        console.error(errorMessage);
        this.snackbarService.showErrorSnackBar(displayError,this.errorSnackBarDuration);
        return throwError(errorMessage);
      })
    );
  }
}
