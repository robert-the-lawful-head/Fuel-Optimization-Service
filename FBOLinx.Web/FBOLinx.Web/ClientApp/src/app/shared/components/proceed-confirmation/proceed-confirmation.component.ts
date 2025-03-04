import { Component, Inject, Input } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

export interface ProceedConfirmationData {
    description: string;
    tableItemsList: string[];
    buttonText: string;
    title: string;
    additionalInfo: string;
    listItemsList: string[];
}

@Component({
    selector: 'app-proceed-confirmation',
    styleUrls: ['./proceed-confirmation.component.scss'],
    templateUrl: './proceed-confirmation.component.html',
})
export class ProceedConfirmationComponent {
    @Input() buttonText = 'Confirm';
    @Input() title = 'Distribute?';

    constructor(
        public dialogRef: MatDialogRef<ProceedConfirmationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ProceedConfirmationData
    ) {
        if (data.buttonText) {
            this.buttonText = data.buttonText;
        }

        if (data.title) {
            this.title = data.title;
        }
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
