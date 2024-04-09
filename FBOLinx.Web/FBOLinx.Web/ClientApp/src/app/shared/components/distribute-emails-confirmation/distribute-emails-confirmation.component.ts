import { Component, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'app-distribute-emails-confirmation',
    styleUrls: ['./distribute-emails-confirmation.component.scss'],
    templateUrl: './distribute-emails-confirmation.component.html',
})
export class DistributeEmailsConfirmationComponent {
    constructor(
        public dialogRef: MatDialogRef<DistributeEmailsConfirmationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any[]
    ) {}

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
