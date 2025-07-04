import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

export interface SaveConfirmationData {
    description: string;
    customTitle: string;
    customText: string;
    save: string;
    discard: string;
    cancel: string;
}

@Component({
    selector: 'app-save-confirmation',
    styleUrls: ['./save-confirmation.component.scss'],
    templateUrl: './save-confirmation.component.html',
})
export class SaveConfirmationComponent {
    constructor(
        public dialogRef: MatDialogRef<SaveConfirmationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: SaveConfirmationData
    ) {}

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
