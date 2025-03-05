import { NgZone } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

export interface NewCustomerTagDialogData {
    oid: number;
    customerId: number;
    groupId: number;
    name: string;
}

@Component({
    selector: 'app-customer-tag-dialog',
    styleUrls: [],
    templateUrl: './customer-tag-dialog.component.html',
})
export class CustomerTagDialogComponent {
    constructor(
        public dialogRef: MatDialogRef<CustomerTagDialogComponent>,
        private ngZone: NgZone,
        @Inject(MAT_DIALOG_DATA) public data: NewCustomerTagDialogData
    ) { }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
