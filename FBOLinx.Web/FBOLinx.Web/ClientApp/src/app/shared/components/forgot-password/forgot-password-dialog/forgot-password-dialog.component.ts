import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, } from '@angular/material/dialog';

// Services

// Interfaces
export interface ForgotPasswordDialogData {
    email: string;
}

@Component({
    selector: 'app-forgot-password-dialog',
    styleUrls: [ './forgot-password-dialog.component.scss' ],
    templateUrl: './forgot-password-dialog.component.html',
})
export class ForgotPasswordDialogComponent {
    constructor(
        public dialogRef: MatDialogRef<ForgotPasswordDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ForgotPasswordDialogData
    ) {
    }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
