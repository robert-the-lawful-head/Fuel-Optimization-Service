import { Component, Inject } from '@angular/core';
import {
    MatDialogRef,
    MAT_DIALOG_DATA,
} from '@angular/material/dialog';

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
    styleUrls: ['./close-confirmation.component.scss'],
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
