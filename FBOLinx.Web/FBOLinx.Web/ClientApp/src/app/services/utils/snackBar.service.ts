import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackBarService {

    constructor(private snackBar: MatSnackBar) { }
    public showErrorSnackBar(message: string): void {
        this.snackBar.open(
            message,
            '',
            {
                duration: 2000,
                panelClass: ['error-snackbar'],
            }
        );
    }
    public showSuccessSnackBar(message: string): void {
        this.snackBar.open(
            message,
            '',
            {
                duration: 2000,
                panelClass: ['blue-snackbar'],
            }
        );
    }
}
