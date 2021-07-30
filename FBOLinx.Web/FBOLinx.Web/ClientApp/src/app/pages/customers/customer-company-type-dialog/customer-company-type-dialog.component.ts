import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, } from '@angular/material/dialog';

export interface NewCustomerCompanyTypeDialogData {
    oid: number;
    fboId: number;
    groupId: number;
    name: string;
    allowMultiplePricingTemplates: boolean;
}

@Component({
    selector: 'app-customer-company-type-dialog',
    styleUrls: [ './customer-company-type-dialog.component.scss' ],
    templateUrl: './customer-company-type-dialog.component.html',
})
export class CustomerCompanyTypeDialogComponent {
    constructor(
        public dialogRef: MatDialogRef<CustomerCompanyTypeDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewCustomerCompanyTypeDialogData
    ) {
    }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
