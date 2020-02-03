import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { ContactsService } from '../../../services/contacts.service';
import { FbosService } from '../../../services/fbos.service';
import { FbocontactsService } from '../../../services/fbocontacts.service';
import { SharedService } from '../../../layouts/shared-service';
import { UserService } from '../../../services/user.service';

export interface AccountProfileDialogData {
    oid: number;
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
    styleUrls: ['./account-profile.component.scss'],
    providers: [SharedService]
})
/** account-profile component*/
export class AccountProfileComponent {
    //Public Members
    public fboInfo: any;
    public contactsData: any[];
    public currentContact: any;
    public availableroles: any[];

    //Private Members
    private selectedContactRecord: any;

    /** account-profile ctor */
    constructor(public dialogRef: MatDialogRef<AccountProfileComponent>,
        @Inject(MAT_DIALOG_DATA) public data: AccountProfileDialogData,
        private sharedService: SharedService,
        private contactsService: ContactsService,
        private fboContactsService: FbocontactsService,
        private fbosService: FbosService,
        private usersService: UserService) {

        this.loadFboInfo();
        this.loadAvailableRoles();
    }

    //Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public contactDeleted(record) {
        this.fboContactsService.remove(record).subscribe(() => {
            this.fboContactsService.getForFbo(this.fboInfo).subscribe((data: any) => this.contactsData = data);
        });
    }

    public newContactClicked() {
        this.currentContact = {
            oid: 0
        }
    }

    public editContactClicked(record) {
        this.selectedContactRecord = record;
        this.contactsService.get({ oid: record.contactId }).subscribe((data: any) => this.currentContact = data);
    }

    public saveEditContactClicked() {
        this.contactsService.update(this.currentContact).subscribe(() => {
            this.currentContact = null;
        });
    }

    public cancelEditContactClicked() {
        this.currentContact = null;
    }

    //Private Methods
    private loadFboInfo(): void {
        if (!this.sharedService.currentUser.fboId || this.sharedService.currentUser.fboId == 0)
            return;
        this.fbosService.get({ oid: this.sharedService.currentUser.fboId }).subscribe((fboData: any) => {
            this.fboContactsService.getForFbo(this.fboInfo).subscribe((data: any) => this.contactsData = data);
        });
        
    }

    private loadAvailableRoles() {
        this.usersService.getRoles().subscribe((data: any) => {
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
            //if (this.data.role > 0)
            //    return;

            if (this.availableroles.length > 1) {
                this.data.role = this.availableroles[this.availableroles.length - 1].Value;
            }
            else {
                this.data.role = this.availableroles[0].value;
            }

            //this.data.role = this.availableroles[this.availableroles.length - 1].Value;
        });
    }
}
