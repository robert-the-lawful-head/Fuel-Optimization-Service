import {
    Component,
    EventEmitter,
    Inject,
    Output,
    ViewChild,
} from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
} from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as _ from 'lodash';

import * as SharedEvents from '../../../models/sharedEvents';
import { SharedService } from '../../../layouts/shared-service';
import { SystemcontactsNewContactModalComponent } from '../../../pages/contacts/systemcontacts-new-contact-modal/systemcontacts-new-contact-modal.component';
import { ContactsService } from '../../../services/contacts.service';
import { FbocontactsService } from '../../../services/fbocontacts.service';
import { FbosService } from '../../../services/fbos.service';
import { FbopricesService } from '../../../services/fboprices.service';
import { FbopreferencesService } from '../../../services/fbopreferences.service';
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
    enableJetA: boolean;
    enableSaf: boolean;
}

@Component({
    providers: [SharedService],
    selector: 'app-account-profile',
    styleUrls: ['./account-profile.component.scss'],
    templateUrl: './account-profile.component.html',
})
export class AccountProfileComponent {
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @Output() editContactClicked = new EventEmitter<any>();
    @Output() newContactClicked = new EventEmitter<any>();
    @Output() productChanged = new EventEmitter<any>();


    // Members
    fboInfo: any;
    fboPreferencesData: any;
    contactsData: any[];
    currentContact: any;
    availableroles: any[];
    systemContactsForm: FormGroup;
    emailDistributionForm: FormGroup;
    productsForm: FormGroup;
    companyForm: FormGroup;
    theFile: any = null;
    logoUrl: string;
    isUploadingLogo: boolean;
    contactsDataSource: MatTableDataSource<any> = null;
    public copyAllAlerts = false;
    public copyAllOrders = false;

    constructor(
        public dialogRef: MatDialogRef<AccountProfileComponent>,
        @Inject(MAT_DIALOG_DATA) public data: AccountProfileDialogData,
        private sharedService: SharedService,
        private contactsService: ContactsService,
        private fboContactsService: FbocontactsService,
        private fbosService: FbosService,
        private fboPreferencesService: FbopreferencesService,
        private fboPricesService: FbopricesService,
        private usersService: UserService,
        private formBuilder: FormBuilder,
        public newContactDialog: MatDialog
    ) {
        this.systemContactsForm = this.formBuilder.group({
            fuelDeskEmail: new FormControl('', [
                Validators.required,
                Validators.email,
            ]),
        });
        this.emailDistributionForm = this.formBuilder.group({
            replyTo: new FormControl('', [
                Validators.required,
                Validators.email,
            ]),
            senderAddress: new FormControl('', [
                Validators.required,
                Validators.pattern('[a-zA-Z0-9-]*'),
            ]),
        });
        this.productsForm = new FormGroup({
            enableJetA: new FormControl(),
            enableSaf: new FormControl()
        });

        this.companyForm = new FormGroup({
            orderNotifications: new FormControl(),
            directOrderNotifications: new FormControl(),
        });


        this.loadFboInfo();
        this.loadFboPreferences();
        this.loadAvailableRoles();
    }

    get fuelDeskEmail() {
        return this.systemContactsForm.get('fuelDeskEmail');
    }

    get replyTo() {
        return this.emailDistributionForm.get('replyTo');
    }

    get senderAddress() {
        return this.emailDistributionForm.get('senderAddress');
    }

    get isCsr() {
        return this.sharedService.currentUser.role === 5;
    }

    // Methods
    onCancelClick(): void {
        this.dialogRef.close();
    }

    contactDeleted(record) {
        this.fboContactsService.remove(record).subscribe(() => {
            this.fboContactsService
                .getForFbo(this.fboInfo)
                .subscribe((data: any) => (this.contactsData = data));
        });
    }

    saveEditContactClicked() {
        this.contactsService.update(this.currentContact ).subscribe(() => {
            this.currentContact = null;
        });
    }

    cancelEditContactClicked() {
        this.currentContact = null;
    }

    onSaveSystemContacts() {
        if (this.systemContactsForm.valid) {
            this.fboInfo.fuelDeskEmail =
                this.systemContactsForm.value.fuelDeskEmail;
            this.fbosService.update(this.fboInfo).subscribe(() => {
                this.fboContactsService
                    .updateFuelvendor({
                        fboId: this.fboInfo.oid,
                    })
                    .subscribe(() => {});
            });
        }
    }

    onSaveEmailDistribution() {
        if (this.emailDistributionForm.valid) {
            this.fboInfo.SenderAddress =
                this.emailDistributionForm.value.senderAddress;
            this.fboInfo.ReplyTo = this.emailDistributionForm.value.replyTo;
            this.fbosService.update(this.fboInfo).subscribe(() => {
                this.dialogRef.close();
            });
        }
    }

    onFileChange(event) {
        this.theFile = null;
        if (event.target.files && event.target.files.length > 0) {
            // Set theFile property
            this.theFile = event.target.files[0];
        }
    }

    uploadFile(): void {
        if (this.theFile != null) {
            this.isUploadingLogo = true;
            this.readAndUploadFile(this.theFile);
        }
    }

    deleteFile(): void {
        this.fbosService
            .deleteLogo(this.fboInfo.oid)
            .subscribe((logoData: any) => {
                this.logoUrl = '';
            });
    }

    onProductsChange(product) {
        if (product == "JetA")
            this.fboPreferencesData.enableJetA = !this.productsForm.value.enableJetA;
        else
            this.fboPreferencesData.enableSaf = !this.productsForm.value.enableSaf;

        this.fboPreferencesService.update(this.fboPreferencesData).subscribe(() => {
            this.fboPricesService.removePricing(this.sharedService.currentUser.fboId, product).subscribe(() => {
                this.productChanged.emit(this.fboPreferencesData);
            });
        });
    }

    onOrderNotificationsChange() {
        this.fboPreferencesData.OrderNotificationsEnabled = !this.companyForm.value.orderNotifications;

        this.fboPreferencesService.update(this.fboPreferencesData).subscribe((data: any) => {
            console.log(data);
        }, (error: any) => {
            console.log(error);
        });
    }
    onDirectOrderNotificationsChange() {
        this.fboPreferencesData.DirectOrderNotificationsEnabled = !this.companyForm.value.directOrderNotifications;

        this.fboPreferencesService.update(this.fboPreferencesData).subscribe((data: any) => {
            console.log(data);
        }, (error: any) => {
            console.log(error);
        });
    }

    public newRecord(e: any) {
        e.preventDefault();

        const dialogRef = this.newContactDialog.open(
            SystemcontactsNewContactModalComponent,
            {
                data: {},
                height: '300px',
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            const payload = {
                ...result,
                fboId: this.sharedService.currentUser.fboId,
            };
            this.fboContactsService
                .addnewcontact(payload)
                .subscribe((newFbocontact) => {
                    this.contactsData = null;
                    this.fboContactsService
                        .getForFbo(this.fboInfo)
                        .subscribe((data: any) => {
                            this.contactsData = data;
                        });
                    this.fboContactsService
                        .updateFuelvendor(payload)
                        .subscribe();
                });
        });
    }

    // Private Methods
    private readAndUploadFile(theFile: any) {
        const file = {
            ContentType: theFile.type,

            FboId: this.fboInfo.oid,

            FileData: null,
            // Set File Information
            FileName: theFile.name,
        };

        // Use FileReader() object to get file to upload
        // NOTE: FileReader only works with newer browsers
        const reader = new FileReader();

        // Setup onload event for reader
        reader.onload = () => {
            // Store base64 encoded representation of file
            file.FileData = reader.result.toString();

            // POST to server
            this.fbosService.uploadLogo(file).subscribe((resp: any) => {
                this.isUploadingLogo = false;
                this.logoUrl = resp.message;
            });
        };

        // Read the file
        reader.readAsDataURL(theFile);
    }

    private loadFboInfo(): void {
        if (
            !this.sharedService.currentUser.fboId ||
            this.sharedService.currentUser.fboId === 0
        ) {
            return;
        }
        this.fbosService
            .get({
                oid: this.sharedService.currentUser.fboId,
            })
            .subscribe((fboData: any) => {
                this.systemContactsForm.setValue({
                    fuelDeskEmail: fboData.fuelDeskEmail,
                });
                this.emailDistributionForm.setValue({
                    replyTo: fboData.replyTo,
                    senderAddress: fboData.senderAddress,
                });
                this.fboInfo = fboData;
                this.fboContactsService
                    .getForFbo(this.fboInfo)
                    .subscribe((data: any) => {
                        this.contactsData = data;
                        this.fbosService
                            .getLogo(this.fboInfo.oid)
                            .subscribe((logoData: any) => {
                                this.logoUrl = logoData.message;
                            });
                    });
            });
    }

    private loadFboPreferences(): void {
        if (
            !this.sharedService.currentUser.fboId ||
            this.sharedService.currentUser.fboId === 0
        ) {
            return;
        }
        this.fboPreferencesService
            .getForFbo(this.sharedService.currentUser.fboId)
            .subscribe((fboPreferencesData: any) => {
                this.fboPreferencesData = fboPreferencesData;
                this.productsForm.setValue({
                    enableJetA: this.fboPreferencesData.enableJetA,
                    enableSaf: this.fboPreferencesData.enableSaf
                });
                this.companyForm.setValue({
                    orderNotifications: this.fboPreferencesData.orderNotificationsEnabled ?? true,
                    directOrderNotifications: this.fboPreferencesData.directOrderNotificationsEnabled ?? true
                });
            });
    }

    private loadAvailableRoles() {
        this.usersService.getRoles().subscribe((data: any) => {
            let supportedRoleValues = [4];
            this.availableroles = [];
            if (this.data.fboId > 0) {
                supportedRoleValues = [1, 4, 5];
            } else if (this.data.groupId > 0) {
                supportedRoleValues = [2];
            }
            for (const role of data) {
                if (supportedRoleValues.indexOf(role.value) > -1) {
                    this.availableroles.push(role);
                }
            }

            if (!this.data.role || this.data.role === 0) {
                if (this.availableroles.length > 1) {
                    this.data.role =
                        this.availableroles[
                            this.availableroles.length - 1
                        ].value;
                } else {
                    this.data.role = this.availableroles[0].value;
                }
            }
        });
    }
}
