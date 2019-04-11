import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface NewCustomerDialogData {
    oid: number;
    company: string;
    groupId: number;
    customerId: number;
    username: string;
    password: string;
    mainPhone: string;
    address: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
    website: string;
    sfid: string;
    emailSubscription: boolean;
}

@Component({
    selector: 'app-customers-dialog-new-customer',
    templateUrl: './customers-dialog-new-customer.component.html',
    styleUrls: ['./customers-dialog-new-customer.component.scss']
})
/** customers-dialog-new-customer component*/
export class CustomersDialogNewCustomerComponent {
    /** customers-dialog-new-customer ctor */
    constructor(public dialogRef: MatDialogRef<CustomersDialogNewCustomerComponent>, @Inject(MAT_DIALOG_DATA) public data: NewCustomerDialogData) {
        data.emailSubscription = true;
    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
