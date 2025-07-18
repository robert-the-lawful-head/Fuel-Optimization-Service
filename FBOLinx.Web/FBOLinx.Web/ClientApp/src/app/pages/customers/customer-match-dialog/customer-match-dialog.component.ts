import { Component, Inject, OnInit } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

export interface CustomerMatchDialogComponent {
    currentCustomerId: number;
    matchCustomerId: number;
    currentCustomerName: string;
    matchCustomerName: string;
    isAircraftMatch: boolean;
    isNameMatch: boolean;
    isContactMatch: boolean;
    matchNameCustomerId: number;
    matchNameCustomerOId: number;
    matchNameCustomer: string;
    matchContactCustomerOid: number;
    matchNameCustomerOid: number;
    matchCustomerOid: number;
    aircraftTails: [];
    result: string;
}

@Component({
    selector: 'customer-match-dialog',
    styleUrls: ['./customer-match-dialog.component.scss'],
    templateUrl: './customer-match-dialog.component.html',
})
export class CustomerMatchDialogComponent implements OnInit {
    constructor(
        public dialogRef: MatDialogRef<CustomerMatchDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: CustomerMatchDialogComponent
    ) {}

    ngOnInit(): void {}

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public keepSeparate(): void {
        this.data.result = 'KeepSeparate';
        this.dialogRef.close('KeepSeparate');
    }

    public mergeCustomers(): void {
        this.data.result = 'Merge';
        this.dialogRef.close(this.data);
    }
}
