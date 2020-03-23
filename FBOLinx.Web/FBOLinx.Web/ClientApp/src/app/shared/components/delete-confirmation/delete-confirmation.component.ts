import { Component, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

export interface DeleteConfirmationData {
    item: any;
    description: string;
}

@Component({
    selector: "app-delete-confirmation",
    templateUrl: "./delete-confirmation.component.html",
    styleUrls: ["./delete-confirmation.component.scss"],
})
export class DeleteConfirmationComponent {
    constructor(
        public dialogRef: MatDialogRef<DeleteConfirmationComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DeleteConfirmationData
    ) {}

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
