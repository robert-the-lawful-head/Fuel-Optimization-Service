import { Component, Inject } from '@angular/core';
import {
    MatDialogRef,
    MAT_DIALOG_DATA,
} from '@angular/material/dialog';

export interface NewCustomerCompanyTypeDialogData {
    oid: number;
    fboId: number;
    groupId: number;
    name: string;
    allowMultiplePricingTemplates: boolean;
}

@Component({
    selector: 'app-customer-company-type-dialog',
    templateUrl: './customer-company-type-dialog.component.html',
    styleUrls: ['./customer-company-type-dialog.component.scss'],
})
export class CustomerCompanyTypeDialogComponent {
    constructor(
        public dialogRef: MatDialogRef<CustomerCompanyTypeDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewCustomerCompanyTypeDialogData
    ) {}

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
