import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

export interface CloseConfirmationData {
    description: string;
    customTitle: string;
    customText: string;
    ok: string;
    cancel: string;
}

@Component({
    selector: 'app-close-confirmation',
    styleUrls: ['./close-confirmation.component.scss'],
    templateUrl: './close-confirmation.component.html',
})
export class CloseConfirmationComponent {
    constructor(
        public dialogRef: MatDialogRef<CloseConfirmationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: CloseConfirmationData
    ) {}

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
