import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

export interface ICsvExportModalData {
    title: string;
    filterStartDate: Date;
    filterEndDate: Date;
}

@Component({
    selector: 'app-csv-export-modal-component',
    styleUrls: ['./csv-export-modal.component.scss'],
    templateUrl: './csv-export-modal.component.html',
})
export class CsvExportModalComponent {
    constructor(
        public dialogRef: MatDialogRef<CsvExportModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ICsvExportModalData
    ) {}

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
