import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef, } from '@angular/material/dialog';

// Services
import { Router } from '@angular/router';

@Component({
    selector: 'app-copy-confirmation',
    templateUrl: './copy-confirmation.component.html',
    styleUrls: ['./copy-confirmation.component.scss'],
})
export class CopyConfirmationComponent {
    constructor(
        public dialogRef: MatDialogRef<CopyConfirmationComponent>,
        public closeConfirmationDialog: MatDialog,
        public router: Router,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        // Prevent modal close on outside click
        dialogRef.disableClose = true;
    }

    public onCancelClick(): void {
        this.dialogRef.close('cancel');
    }
}
