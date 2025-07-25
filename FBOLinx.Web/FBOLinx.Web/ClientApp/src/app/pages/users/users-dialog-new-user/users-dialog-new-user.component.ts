import { Component, Inject } from '@angular/core';
import { MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';

// Services
import { UserService } from '../../../services/user.service';
import { SharedService } from '../../../layouts/shared-service';

// Interfaces
export interface NewUserDialogData {
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
    selector: 'app-users-dialog-new-user',
    styleUrls: ['./users-dialog-new-user.component.scss'],
    templateUrl: './users-dialog-new-user.component.html',
})
export class UsersDialogNewUserComponent {
    // Public Members
    public availableroles: any[];
    public emailExists = false;

    constructor(
        public dialogRef: MatDialogRef<UsersDialogNewUserComponent>,
        @Inject(MAT_DIALOG_DATA) public data: NewUserDialogData,
        private userService: UserService,
        private sharedService: SharedService
    ) {
        this.loadAvailableRoles();
    }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public onSaveClick(): void {
        this.emailExists = false;
        this.userService.checkemailexists(this.data.username).subscribe(
            (data: any) => {
                this.dialogRef.close(this.data);
            },
            (err: any) => {
                console.log(err);
                if (err === 'Conflict') {
                    this.emailExists = true;
                }
            }
        );
    }

    // Private Methods
    private loadAvailableRoles() {
        this.userService.getRoles().subscribe((data: any) => {
            let supportedRoleValues = [4];
            this.availableroles = [];
            if (this.data.fboId > 0) {
                supportedRoleValues = [1, 4, 5];
            } else if (this.data.groupId > 0) {
                supportedRoleValues = [2];
            }

            if (this.sharedService.currentUser.role === 3) {
                supportedRoleValues.push(7);
            }

            for (const role of data) {
                if (supportedRoleValues.indexOf(role.value) > -1) {
                    this.availableroles.push(role);
                }
            }
            if (this.data.role > 0) {
                return;
            }
            this.data.role = this.availableroles[0].value;
        });
    }
}
