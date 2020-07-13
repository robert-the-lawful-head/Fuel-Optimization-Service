import { Component, Inject } from "@angular/core";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

import { ContactsService } from "../../../services/contacts.service";
import { FbosService } from "../../../services/fbos.service";
import { FbocontactsService } from "../../../services/fbocontacts.service";
import { SharedService } from "../../../layouts/shared-service";
import { UserService } from "../../../services/user.service";

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
    selector: "app-account-profile",
    templateUrl: "./account-profile.component.html",
    styleUrls: ["./account-profile.component.scss"],
    providers: [SharedService],
})
export class AccountProfileComponent {
    // Public Members
    public fboInfo: any;
    public contactsData: any[];
    public currentContact: any;
    public availableroles: any[];
    public systemContactsForm: FormGroup;

    constructor(
        public dialogRef: MatDialogRef<AccountProfileComponent>,
        @Inject(MAT_DIALOG_DATA) public data: AccountProfileDialogData,
        private sharedService: SharedService,
        private contactsService: ContactsService,
        private fboContactsService: FbocontactsService,
        private fbosService: FbosService,
        private usersService: UserService,
        private formBuilder: FormBuilder
    ) {
        this.systemContactsForm = this.formBuilder.group({
            fuelDeskEmail: new FormControl(""),
        });
        this.loadFboInfo();
        this.loadAvailableRoles();
    }

    // Public Methods
    public onCancelClick(): void {
        this.dialogRef.close();
    }

    public contactDeleted(record) {
        this.fboContactsService.remove(record).subscribe(() => {
            this.fboContactsService
                .getForFbo(this.fboInfo)
                .subscribe((data: any) => (this.contactsData = data));
        });
    }

    public saveEditContactClicked() {
        this.contactsService.update(this.currentContact).subscribe(() => {
            this.currentContact = null;
        });
    }

    public cancelEditContactClicked() {
        this.currentContact = null;
    }

    public onSaveSystemContacts() {
        if (this.systemContactsForm.valid) {
            this.fboInfo.fuelDeskEmail = this.systemContactsForm.value.fuelDeskEmail;
            this.fbosService.update(this.fboInfo).subscribe(() => {
                this.fboContactsService.updateFuelvendor({ fboId: this.fboInfo.oid }).subscribe(() => {
                    this.dialogRef.close();
                });
            });
        }
    }

    // Private Methods
    private loadFboInfo(): void {
        if (
            !this.sharedService.currentUser.fboId ||
            this.sharedService.currentUser.fboId === 0
        ) {
            return;
        }
        this.fbosService
            .get({ oid: this.sharedService.currentUser.fboId })
            .subscribe((fboData: any) => {
                this.systemContactsForm.setValue({
                    fuelDeskEmail: fboData.fuelDeskEmail,
                });
                this.fboInfo = fboData;
                this.fboContactsService
                    .getForFbo(this.fboInfo)
                    .subscribe((data: any) => (this.contactsData = data));
            });
    }

    private loadAvailableRoles() {
        this.usersService.getRoles().subscribe((data: any) => {
            let supportedRoleValues = [4];
            this.availableroles = [];
            if (this.data.fboId > 0) {
                supportedRoleValues = [1, 4];
            } else if (this.data.groupId > 0) {
                supportedRoleValues = [2];
            }
            for (const role of data) {
                if (supportedRoleValues.indexOf(role.value) > -1) {
                    this.availableroles.push(role);
                }
            }

            if (this.availableroles.length > 1) {
                this.data.role = this.availableroles[
                    this.availableroles.length - 1
                ].Value;
            } else {
                this.data.role = this.availableroles[0].value;
            }
        });
    }
}
