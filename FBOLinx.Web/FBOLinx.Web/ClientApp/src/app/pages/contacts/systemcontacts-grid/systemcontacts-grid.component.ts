import { Component, EventEmitter, Input, OnInit, Output, ViewChild, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import * as _ from 'lodash';
import FlatfileImporter from 'flatfile-csv-importer';

// Services
import { ContactinfobygroupsService } from '../../../services/contactinfobygroups.service';
import { FbocontactsService } from '../../../services/fbocontacts.service';
import { ContactsService } from '../../../services/contacts.service';
import { SharedService } from '../../../layouts/shared-service';
import { SystemcontactsNewContactModalComponent } from '../systemcontacts-new-contact-modal/systemcontacts-new-contact-modal.component';

@Component({
    selector: 'app-systemcontacts-grid',
    templateUrl: './systemcontacts-grid.component.html',
    styleUrls: [ './systemcontacts-grid.component.scss' ],
})
export class SystemcontactsGridComponent implements OnInit {
    @Output() contactDeleted = new EventEmitter<any>();
    @Output() newContactClicked = new EventEmitter<any>();
    @Output() editContactClicked = new EventEmitter<any>();
    @Input() contactsData: Array<any>;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    public currentContactInfoByGroup: any;
    contactsDataSource: MatTableDataSource<any> = null;
    displayedColumns: string[] = [
        'firstName',
        'lastName',
        'email',
        'copyAlerts',
        'copyOrders',
        'delete',
    ];
    public copyAllAlerts = false;
    public copyAllOrders = false;

    LICENSE_KEY = '9eef62bd-4c20-452c-98fd-aa781f5ac111';

    results = '[]';

    private importer: FlatfileImporter;

    constructor(
        private route: ActivatedRoute,
        public deleteUserDialog: MatDialog,
        private fbocontactsService: FbocontactsService,
        private contactsService: ContactsService,
        private contactInfoByGroupsService: ContactinfobygroupsService,
        private sharedService: SharedService,
        public newContactDialog: MatDialog
    ) {
    }

    ngOnInit() {
        if (!this.contactsData) {
            return;
        }

        this.refreshTable();

        FlatfileImporter.setVersion(2);
        this.initializeImporter();
        this.importer.setCustomer({
            userId: '1',
            name: 'WebsiteImport',
        });
    }

    // Public Methods
    public refreshTable() {
        this.sort.sortChange.subscribe(() => {
        });
        this.contactsDataSource = new MatTableDataSource(this.contactsData);
        this.contactsDataSource.sort = this.sort;
        const unselectedIndexAlerts = _.findIndex(this.contactsData, (contact) => !contact.copyAlerts);
        this.copyAllAlerts = this.contactsData.length && unselectedIndexAlerts === -1 ? true : false;
        const unselectedIndexOrders = _.findIndex(this.contactsData, (contact) => !contact.copyOrders);
        this.copyAllOrders = this.contactsData.length && unselectedIndexOrders === -1 ? true : false;
    }

    public editRecord(record: any) {
        const dialogRef = this.newContactDialog.open(
            SystemcontactsNewContactModalComponent,
            {
                height: '350px',
                width: '1100px',
                data: Object.assign({}, record),
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            if (result === 'delete') {
                this.fbocontactsService
                    .remove(record)
                    .subscribe(() => {
                        this.contactsData = this.contactsData.filter(c => c.oid !== record.oid);
                        this.refreshTable();
                        this.fbocontactsService.updateFuelvendor({ fboId: this.sharedService.currentUser.fboId }).subscribe();
                    });
            } else {
                const updatedContact = {
                    ...record,
                    ...result,
                    oid: result.contactId,
                };

                this.contactsService.update(updatedContact).subscribe(() => {
                    for (let i = 0; i < this.contactsData.length; i++) {
                        if (this.contactsData[i].oid === result.oid) {
                            this.contactsData[i] = result;
                        }
                    }
                    this.refreshTable();
                    this.fbocontactsService.updateFuelvendor({ fboId: this.sharedService.currentUser.fboId }).subscribe();
                });
            }
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
        dialogRef.afterClosed().subscribe(result => {
            if (!result) {
                return;
            }

            const payload = {
                ...result,
                fboId: this.sharedService.currentUser.fboId,
            };
            this.fbocontactsService.addnewcontact(payload).subscribe(newFbocontact => {
                this.contactsData.push(newFbocontact);
                this.refreshTable();
                this.fbocontactsService.updateFuelvendor(payload).subscribe();
            });
        });
    }

    public updateCopyAlertsValue(value: any) {
        const unselectedIndexAlerts = _.findIndex(this.contactsData, (contact) => !contact.copyAlerts);
        this.copyAllAlerts = unselectedIndexAlerts >= 0 ? false : true;

        value.groupId = this.sharedService.currentUser.groupId;
        const updatedContact = Object.assign({}, value);
        updatedContact.oid = value.contactId;

        this.contactsService.update(updatedContact).subscribe();
    }

    public updateAllCopyAlertsValues() {
        this.copyAllAlerts = !this.copyAllAlerts;
        _.forEach(this.contactsData, (contact) => {
            contact.copyAlerts = this.copyAllAlerts;
            contact.GroupId = this.sharedService.currentUser.groupId;
            const updatedContact = Object.assign({}, contact);
            updatedContact.oid = contact.contactId;

            this.contactsService.update(updatedContact).subscribe();
        });
    }

    public updateCopyOrdersValue(value: any) {
        const unselectedIndexOrders = _.findIndex(this.contactsData, (contact) => !contact.copyOrders);
        this.copyAllOrders = unselectedIndexOrders >= 0 ? false : true;

        value.groupId = this.sharedService.currentUser.groupId;
        const updatedContact = Object.assign({}, value);
        updatedContact.oid = value.contactId;

        this.contactsService.update(updatedContact).subscribe();
    }

    public updateAllCopyOrdersValues() {
        this.copyAllOrders = !this.copyAllOrders;
        _.forEach(this.contactsData, (contact) => {
            contact.copyOrders = this.copyAllOrders;
            contact.GroupId = this.sharedService.currentUser.groupId;
            const updatedContact = Object.assign({}, contact);
            updatedContact.oid = contact.contactId;

            this.contactsService.update(updatedContact).subscribe();
        });
    }

    async launchImporter() {
        if (!this.LICENSE_KEY) {
            return alert('Set LICENSE_KEY on Line 13 before continuing.');
        }
        try {
            const results = await this.importer.requestDataFromUser();
            this.importer.displayLoader();
            const customerId = this.route.snapshot.paramMap.get('id');
            if (results) {
                results.data.forEach((result) => {
                    result.groupid = this.sharedService.currentUser.groupId;
                    result.customerId = customerId;
                });
                this.contactInfoByGroupsService
                    .import(results.data)
                    .subscribe((data: any) => {
                        if (data) {
                            data.forEach((result) => {
                                this.contactsData.push(result);
                            });

                            this.contactsDataSource = new MatTableDataSource(
                                this.contactsData
                            );
                            this.contactsDataSource.sort = this.sort;

                            this.importer.displaySuccess(
                                'Data successfully imported!'
                            );
                        }
                    });
            }
        } catch (e) {
        }
    }

    initializeImporter() {
        this.importer = new FlatfileImporter(this.LICENSE_KEY, {
            fields: [
                {
                    label: 'First Name',
                    alternates: [ 'first name' ],
                    key: 'FirstName',
                    description: 'Contact First Name',
                    validators: [
                        {
                            validate: 'required',
                            error: 'this field is required',
                        },
                    ],
                },
                {
                    label: 'Last Name',
                    alternates: [ 'last name' ],
                    key: 'LastName',
                    description: 'Contact Last Name',
                    validators: [
                        {
                            validate: 'required',
                            error: 'this field is required',
                        },
                    ],
                },
                {
                    label: 'Title',
                    alternates: [ 'title' ],
                    key: 'Title',
                    description: 'Contact Title',
                },
                {
                    label: 'Email',
                    alternates: [ 'email', 'email address' ],
                    key: 'Email',
                    description: 'Email Address',
                },
                {
                    label: 'Phone Number',
                    alternates: [ 'phone', 'phone number' ],
                    key: 'PhoneNumber',
                    description: 'Phone Number',
                },
                {
                    label: 'Extension',
                    alternates: [ 'extension' ],
                    key: 'Extension',
                    description: 'Phone Extension',
                },
                {
                    label: 'Mobile',
                    alternates: [ 'mobile', 'cell', 'mobile phone', 'cell phone' ],
                    key: 'MobilePhone',
                    description: 'Mobile Phone',
                },
                {
                    label: 'Fax',
                    alternates: [ 'fax' ],
                    key: 'Fax',
                    description: 'Fax',
                },
                {
                    label: 'Address',
                    alternates: [ 'address', 'street address' ],
                    key: 'Address',
                    description: 'Street Address',
                },
                {
                    label: 'City',
                    alternates: [ 'city', 'town' ],
                    key: 'City',
                    description: 'City',
                },
                {
                    label: 'State',
                    alternates: [ 'state' ],
                    key: 'State',
                    description: 'State',
                },
                {
                    label: 'Country',
                    alternates: [ 'country' ],
                    key: 'Country',
                    description: 'Country',
                },
                {
                    label: 'Primary',
                    alternates: [ 'primary' ],
                    key: 'PrimaryContact',
                    description: 'Primary',
                },
                {
                    label: 'Copy on Distribution',
                    alternates: [ 'copy on distribution' ],
                    key: 'CopyAlertsContact',
                    description: 'Copy Contact on Distribution',
                },
            ],
            type: 'Contacts',
            allowInvalidSubmit: true,
            managed: true,
            allowCustom: true,
            disableManualInput: false,
        });
    }
}
