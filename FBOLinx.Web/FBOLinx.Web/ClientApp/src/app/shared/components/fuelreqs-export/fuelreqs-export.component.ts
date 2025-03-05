import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

@Component({
    selector: 'app-fuelreqs-export-modal-component',
    styleUrls: ['./fuelreqs-export.component.scss'],
    templateUrl: './fuelreqs-export.component.html',
})
export class FuelReqsExportModalComponent {
    constructor(
        public dialogRef: MatDialogRef<FuelReqsExportModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
