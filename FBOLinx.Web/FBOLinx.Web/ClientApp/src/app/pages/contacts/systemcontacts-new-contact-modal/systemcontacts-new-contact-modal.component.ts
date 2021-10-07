import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
    MAT_DIALOG_DATA,
    MatDialog,
    MatDialogRef,
} from '@angular/material/dialog';

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
    contactForm: FormGroup;

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

        this.contactForm = new FormGroup({
            copyAlerts: new FormControl(this.data.copyAlerts),
            copyOrders: new FormControl(this.data.copyOrders),
            email: new FormControl(this.data.email, [
                Validators.required,
                Validators.email,
            ]),
            firstName: new FormControl(this.data.firstName),
            lastName: new FormControl(this.data.lastName),
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
