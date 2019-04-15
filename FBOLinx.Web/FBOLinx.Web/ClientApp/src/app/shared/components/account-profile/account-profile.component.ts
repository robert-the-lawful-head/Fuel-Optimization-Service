import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface AccountProfileDialogData {
    firstName: string;
    lastName: string;
    username: string;
    password: string;
    role: number;
    fboId: number;
    groupId: number;
    newPassword: string;
    confirmPassword: string;
}

@Component({
    selector: 'app-account-profile',
    templateUrl: './account-profile.component.html',
    styleUrls: ['./account-profile.component.scss']
})
/** account-profile component*/
export class AccountProfileComponent {
    /** account-profile ctor */
    constructor(public dialogRef: MatDialogRef<AccountProfileComponent>, @Inject(MAT_DIALOG_DATA) public data: AccountProfileDialogData) {

    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }
}
