import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { UserService } from '../../../services/user.service';

//Interfaces
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
    templateUrl: './users-dialog-new-user.component.html',
    styleUrls: ['./users-dialog-new-user.component.scss']
})
/** users-dialog-new-user component*/
export class UsersDialogNewUserComponent {

    //Public Members
    public availableroles: any[];

    /** users-dialog-new-user ctor */
    constructor(public dialogRef: MatDialogRef<UsersDialogNewUserComponent>, @Inject(MAT_DIALOG_DATA) public data: NewUserDialogData,
        private userService: UserService) {

        this.loadAvailableRoles();
    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }

    //Private Methods
    private loadAvailableRoles() {
        this.userService.getRoles().subscribe((data: any) => {
            var supportedRoleValues = [4];
            this.availableroles = [];
            if (this.data.fboId > 0) {
                supportedRoleValues = [1, 4];
            }
            else if (this.data.groupId > 0) {
                supportedRoleValues = [2];
            }
            for (let role of data) {
                if (supportedRoleValues.indexOf(role.value) > -1)
                    this.availableroles.push(role);
            }
            if (this.data.role > 0)
                return;
            this.data.role = this.availableroles[this.availableroles.length - 1].value;
        });
    }
}
