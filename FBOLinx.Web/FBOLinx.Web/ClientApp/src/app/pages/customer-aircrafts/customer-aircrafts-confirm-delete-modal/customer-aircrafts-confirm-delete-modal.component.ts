import { Component, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

@Component({
    selector: "app-aircraft-dialog-confirm-delete",
    templateUrl: "./customer-aircrafts-confirm-delete-modal.component.html",
    styleUrls: ["./customer-aircrafts-confirm-delete-modal.component.scss"],
})
export class DialogConfirmAircraftDeleteComponent {
    private aircraftId = 0;
    constructor(
        public dialogRef: MatDialogRef<DialogConfirmAircraftDeleteComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        if (data) {
            console.log(data);
        }
    }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close("cancel");
    }

    public saveEdit() {
        this.dialogRef.close(this.aircraftId);
    }
}
