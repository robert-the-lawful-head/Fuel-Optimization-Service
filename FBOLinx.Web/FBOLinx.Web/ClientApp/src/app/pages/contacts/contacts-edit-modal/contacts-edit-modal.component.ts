import { Component, Inject } from '@angular/core';
import {
    MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA,
    MatLegacyDialog as MatDialog,
    MatLegacyDialogRef as MatDialogRef,
} from '@angular/material/legacy-dialog';

import { ContactsDialogConfirmContactDeleteComponent } from '../contact-confirm-delete-modal/contact-confirm-delete-modal.component';

export interface NewContactDialogData {
    oid: number;

    firstName: string;
    lastName: string;
    title: string;
    email: string;
    phone: string;
    extension: string;
    mobile: string;
    fax: string;
    address: string;
    city: string;
    state: string;
    country: string;
    primary: boolean;
    copyAlerts: boolean;
}

@Component({
    selector: 'app-contacts-dialog-new-contact',
    styleUrls: ['./contacts-edit-modal.component.scss'],
    templateUrl: './contacts-edit-modal.component.html',
})
export class ContactsDialogNewContactComponent {
    // Masks
    phoneMask: any[] = [
        '+',
        '1',
        ' ',
        '(',
        /[1-9]/,
        /\d/,
        /\d/,
        ')',
        ' ',
        /\d/,
        /\d/,
        /\d/,
        '-',
        /\d/,
        /\d/,
        /\d/,
        /\d/,
    ];

    constructor(
        public dialogRef: MatDialogRef<ContactsDialogNewContactComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewContactDialogData,
        public dialogContactDeleteRef: MatDialog
    ) {}

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close('cancel');
    }

    public ConfirmDelete(data) {
        const dialogRef = this.dialogContactDeleteRef.open(
            ContactsDialogConfirmContactDeleteComponent,
            {
                data,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result === 'cancel') {
            } else if (result.contactId) {
                result.toDelete = true;
                this.dialogRef.close(result);
            }
        });
    }
}
