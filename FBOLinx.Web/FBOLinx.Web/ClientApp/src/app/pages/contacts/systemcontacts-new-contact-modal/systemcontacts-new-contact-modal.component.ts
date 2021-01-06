import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef, } from '@angular/material/dialog';
import { ContactsDialogConfirmContactDeleteComponent } from '../contact-confirm-delete-modal/contact-confirm-delete-modal.component';
import { FormControl, FormGroup, Validators } from '@angular/forms';

export interface NewContactDialogData {
    firstName?: string;
    lastName?: string;
    email: string;
    copyAlerts?: boolean;
}

@Component({
    selector: 'app-systemcontacts-new-contact-modal',
    templateUrl: './systemcontacts-new-contact-modal.component.html',
    styleUrls: [ './systemcontacts-new-contact-modal.component.scss' ],
})
export class SystemcontactsNewContactModalComponent implements OnInit {
    contactForm: FormGroup;

    constructor(
        public dialogRef: MatDialogRef<SystemcontactsNewContactModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewContactDialogData,
        public dialogContactDeleteRef: MatDialog
    ) {
    }

    ngOnInit(): void {
        this.contactForm = new FormGroup({
            email: new FormControl(this.data.email, [
                Validators.required,
                Validators.email,
            ]),
            firstName: new FormControl(this.data.firstName),
            lastName: new FormControl(this.data.lastName),
            copyAlerts: new FormControl(this.data.copyAlerts),
        });
    }

    // Public Methods
    public onSave(): void {
        if (this.contactForm.valid) {
            const result: NewContactDialogData = {
                ...this.data,
                firstName: this.contactForm.value.firstName,
                lastName: this.contactForm.value.lastName,
                email: this.contactForm.value.email,
                copyAlerts: this.contactForm.value.copyAlerts,
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
