import { Component, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

@Component({
    selector: "app-manage-confirmation",
    templateUrl: "./manage-confirmation.component.html",
    styleUrls: ["./manage-confirmation.component.scss"],
})
export class ManageConfirmationComponent {
    constructor(
        public dialogRef: MatDialogRef<ManageConfirmationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {}

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
