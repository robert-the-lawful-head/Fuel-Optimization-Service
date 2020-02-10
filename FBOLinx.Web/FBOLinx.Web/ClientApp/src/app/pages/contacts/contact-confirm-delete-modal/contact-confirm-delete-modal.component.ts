import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'app-contacts-dialog-confirm-contact-delete',
    templateUrl: './contact-confirm-delete-modal.component.html',
    styleUrls: ['./contact-confirm-delete-modal.component.scss']
})

export class ContactsDialogConfirmContactDeleteComponent {
    private contactId: number = 0;
    constructor(public dialogRef: MatDialogRef<ContactsDialogConfirmContactDeleteComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {
        if (data) {
            this.contactId = data.contactId;
            console.log(data);
        }
    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close('cancel');
    }

    public saveEdit() {
        this.dialogRef.close(this.contactId);
    }

    public ConfirmDelete(data) {
        console.log(data);
    }
}
