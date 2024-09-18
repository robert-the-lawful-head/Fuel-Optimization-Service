import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackBarService {
    
    constructor(private snackBar: MatSnackBar) { }
    public showErrorSnackBar(message: string, duration = 2000): void {
        this.snackBar.open(
            message,
            '',
            {
                duration: duration,
                panelClass: ['error-snackbar'],
            }
        );
    }
    public showSuccessSnackBar(message: string,duration = 2000): void {
        this.snackBar.open(
            message,
            '',
            {
                duration: duration,
                panelClass: ['blue-snackbar'],
            }
        );
    }
}
