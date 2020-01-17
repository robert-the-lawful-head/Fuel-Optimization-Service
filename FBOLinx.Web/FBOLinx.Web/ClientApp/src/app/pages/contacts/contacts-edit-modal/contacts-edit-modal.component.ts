import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface NewContactDialogData {
    oid: number;
    firstName: string;
    lastName: string;
    title: string;
    email: string;
    phone: string;
    extension: string;
}

@Component({
    selector: 'app-contacts-dialog-new-contact',
    templateUrl: './contacts-edit-modal.component.html',
    styleUrls: ['./contacts-edit-modal.component.scss']
})

export class ContactsDialogNewContactComponent {
    //Masks
    phoneMask: any[] = ['+', '1', ' ', '(', /[1-9]/, /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];

    constructor(public dialogRef: MatDialogRef<ContactsDialogNewContactComponent>, @Inject(MAT_DIALOG_DATA) public data: NewContactDialogData) {
        if (data) {
            console.log(data);
        }
    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close('cancel');
    }

    public saveEdit() {
        
    }
}
