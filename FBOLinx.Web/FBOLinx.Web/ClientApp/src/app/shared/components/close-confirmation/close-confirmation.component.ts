import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface CloseConfirmationData {
    description: string;
    customTitle: string;
    customText: string;
    ok: string;
    cancel: string;
}

@Component({
    selector: 'app-close-confirmation',
    templateUrl: './close-confirmation.component.html',
    styleUrls: ['./close-confirmation.component.scss']
})
/** close-confirmation component*/
export class CloseConfirmationComponent {
    /** close-confirmation ctor */
    constructor(public dialogRef: MatDialogRef<CloseConfirmationComponent>, @Inject(MAT_DIALOG_DATA) public data: CloseConfirmationData) {

    }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
