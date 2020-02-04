import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CloseConfirmationComponent } from '../../../shared/components/close-confirmation/close-confirmation.component';

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
    constructor(
        public dialogRef: MatDialogRef<CustomersDialogNewCustomerComponent>,
        public closeConfirmationDialog: MatDialog,
        @Inject(MAT_DIALOG_DATA) public data: NewCustomerDialogData
    ) {
        data.emailSubscription = true;
        
        // Prevent modal close on outside click
        dialogRef.disableClose = true;
        dialogRef.backdropClick().subscribe(() => {
            if (!this.data.company) {
                dialogRef.close();
            } else {
                const closeDialogRef = this.closeConfirmationDialog.open(CloseConfirmationComponent, {
                    data: {
                        customTitle: 'Discard Change?',
                        customText: 'You are about to close this modal. Are you sure?',
                        ok: 'Discard',
                        cancel: 'Cancel'
                    }
                });
                closeDialogRef.afterClosed().subscribe(result => {
                    if (result === true) {
                        dialogRef.close();
                    }
                });
            }
        })
    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
