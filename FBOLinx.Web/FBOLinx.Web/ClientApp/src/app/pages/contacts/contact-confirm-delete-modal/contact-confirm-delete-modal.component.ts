import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

@Component({
    selector: 'app-contacts-dialog-confirm-contact-delete',
    styleUrls: ['./contact-confirm-delete-modal.component.scss'],
    templateUrl: './contact-confirm-delete-modal.component.html',
})
export class ContactsDialogConfirmContactDeleteComponent {
    private contactId = 0;

    constructor(
        public dialogRef: MatDialogRef<ContactsDialogConfirmContactDeleteComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        if (data) {
            this.contactId = data.contactId;
        }
    }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close('cancel');
    }

    public saveEdit() {
        this.dialogRef.close(this.contactId);
    }
}
