import { Component, Inject } from "@angular/core";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

// Services
import { AuthenticationService } from "../../../../services/authentication.service";

// Interfaces
export interface ForgotPasswordDialogData {
    username: string;
}

@Component({
    selector: "app-forgot-password-dialog",
    templateUrl: "./forgot-password-dialog.component.html",
    styleUrls: ["./forgot-password-dialog.component.scss"],
})
export class ForgotPasswordDialogComponent {
    constructor(
        public dialogRef: MatDialogRef<ForgotPasswordDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ForgotPasswordDialogData
    ) {}

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
