import { Component, Inject, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import {
    MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA,
    MatLegacyDialog as MatDialog,
    MatLegacyDialogRef as MatDialogRef,
} from '@angular/material/legacy-dialog';

import { ContactsDialogConfirmContactDeleteComponent } from '../contact-confirm-delete-modal/contact-confirm-delete-modal.component';

export interface NewContactDialogData {
    firstName?: string;
    lastName?: string;
    email: string;
    copyAlerts?: boolean;
    copyOrders?: boolean;
}

@Component({
    selector: 'app-systemcontacts-new-contact-modal',
    styleUrls: ['./systemcontacts-new-contact-modal.component.scss'],
    templateUrl: './systemcontacts-new-contact-modal.component.html',
})
export class SystemcontactsNewContactModalComponent implements OnInit {
    contactForm: UntypedFormGroup;

    constructor(
        public dialogRef: MatDialogRef<SystemcontactsNewContactModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewContactDialogData,
        public dialogContactDeleteRef: MatDialog
    ) {}

    ngOnInit(): void {
        if (!this.data.email) {
            this.data.copyAlerts = true;
            this.data.copyOrders = true;
        }

        this.contactForm = new UntypedFormGroup({
            copyAlerts: new UntypedFormControl(this.data.copyAlerts),
            copyOrders: new UntypedFormControl(this.data.copyOrders),
            email: new UntypedFormControl(this.data.email, [
                Validators.required,
                Validators.email,
            ]),
            firstName: new UntypedFormControl(this.data.firstName),
            lastName: new UntypedFormControl(this.data.lastName),
        });
    }

    // Public Methods
    public onSave(): void {
        if (this.contactForm.valid) {
            const result: NewContactDialogData = {
                ...this.data,
                copyAlerts: this.contactForm.value.copyAlerts,
                copyOrders: this.contactForm.value.copyOrders,
                email: this.contactForm.value.email,
                firstName: this.contactForm.value.firstName,
                lastName: this.contactForm.value.lastName,
            };
            this.dialogRef.close(result);
        }
    }

    public confirmDelete(data: NewContactDialogData) {
        const dialogRef = this.dialogContactDeleteRef.open(
            ContactsDialogConfirmContactDeleteComponent,
            {
                data,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result || result !== 'cancel') {
                this.dialogRef.close('delete');
            }
        });
    }
}
