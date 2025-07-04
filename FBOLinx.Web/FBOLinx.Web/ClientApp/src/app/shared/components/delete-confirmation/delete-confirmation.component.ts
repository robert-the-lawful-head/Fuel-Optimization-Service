import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

export interface DeleteConfirmationData {
    item: any;
    description?: string;
    fullDescription?: string;
    includeThis: boolean;
}

@Component({
    selector: 'app-delete-confirmation',
    styleUrls: ['./delete-confirmation.component.scss'],
    templateUrl: './delete-confirmation.component.html',
})
export class DeleteConfirmationComponent {
    constructor(
        public dialogRef: MatDialogRef<DeleteConfirmationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DeleteConfirmationData
    ) {}

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
