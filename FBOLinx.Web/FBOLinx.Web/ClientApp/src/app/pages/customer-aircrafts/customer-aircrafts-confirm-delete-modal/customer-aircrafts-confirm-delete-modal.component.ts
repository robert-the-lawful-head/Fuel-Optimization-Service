import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, } from '@angular/material/dialog';

@Component({
    selector: 'app-aircraft-dialog-confirm-delete',
    styleUrls: [ './customer-aircrafts-confirm-delete-modal.component.scss' ],
    templateUrl: './customer-aircrafts-confirm-delete-modal.component.html',
})
export class DialogConfirmAircraftDeleteComponent {
    private aircraftId = 0;

    constructor(
        public dialogRef: MatDialogRef<DialogConfirmAircraftDeleteComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
    }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close('cancel');
    }

    public saveEdit() {
        this.dialogRef.close(this.aircraftId);
    }
}
