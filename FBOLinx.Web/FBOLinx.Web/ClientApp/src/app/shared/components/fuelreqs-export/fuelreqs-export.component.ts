import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, } from '@angular/material/dialog';

@Component({
    selector: 'app-fuelreqs-export-modal-component',
    templateUrl: './fuelreqs-export.component.html',
    styleUrls: [ './fuelreqs-export.component.scss' ],
})
export class FuelReqsExportModalComponent {
    constructor(
        public dialogRef: MatDialogRef<FuelReqsExportModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
