import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
/*import FlatfileImporter from 'flatfile-csv-importer';*/
import * as _ from 'lodash';

import { SharedService } from '../../../layouts/shared-service';
import { ContactinfobygroupsService } from '../../../services/contactinfobygroups.service';
// Services
import { CustomercontactsService } from '../../../services/customercontacts.service';
import { ContactsDialogNewContactComponent } from '../contacts-edit-modal/contacts-edit-modal.component';

@Component({
    selector: 'app-contacts-grid',
    styleUrls: ['./contacts-grid.component.scss'],
    templateUrl: './contacts-grid.component.html',
})
export class ContactsGridComponent implements OnInit {
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
        'title',
        'email',
        'phone',
        'copyAlerts',
        'delete',
    ];
    public copyAll = false;

    /*LICENSE_KEY = '9eef62bd-4c20-452c-98fd-aa781f5ac111';*/

    results = '[]';

    /*private importer: FlatfileImporter;*/

    constructor(
        public deleteUserDialog: MatDialog,
        private contactInfoByGroupsService: ContactinfobygroupsService,
        private sharedService: SharedService,
        private customerContactsService: CustomercontactsService,
        public newContactDialog: MatDialog,
        private route: ActivatedRoute
    ) {}

    ngOnInit() {
        if (!this.contactsData) {
            return;
        }

        const foundedIndex = _.findIndex(
            this.contactsData,
            (contact) => !contact.copyAlerts
        );
        this.copyAll = foundedIndex >= 0 ? false : true;
        this.sort.sortChange.subscribe(() => {});
        this.contactsDataSource = new MatTableDataSource(this.contactsData);
        this.contactsDataSource.sort = this.sort;

        /*this.initializeImporter();*/
    }

    // Public Methods
    public deleteRecord(record) {
        this.customerContactsService
            .remove(record.customerContactId)
            .subscribe(() => {
                this.contactInfoByGroupsService
                    .remove(record.contactInfoByGroupId)
                    .subscribe(() => {
                        const index = this.contactsData.findIndex(
                            (d) =>
                                d.customerContactId === record.customerContactId
                        ); // find index in your array
                        this.contactsData.splice(index, 1); // remove element from array
                        this.contactsDataSource = new MatTableDataSource(
                            this.contactsData
                        );
                        this.contactsDataSource.sort = this.sort;
                    });
            });
    }

    public editRecord(record, $event) {
        if ($event.target) {
            if ($event.target.className.indexOf('mat-slide-toggle') > -1) {
                $event.stopPropagation();
                return false;
            } else {
                const clonedRecord = Object.assign({}, record);
                this.editContactClicked.emit(clonedRecord);
            }
        } else {
            const clonedRecord = Object.assign({}, record);
            this.editContactClicked.emit(clonedRecord);
        }
    }

    public editContactPopup(record) {
        const dialogRef = this.newContactDialog.open(
            ContactsDialogNewContactComponent,
            {
                data: record,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result !== 'cancel') {
                if (result.toDelete) {
                    this.customerContactsService
                        .remove(record.customerContactId)
                        .subscribe(() => {
                            this.contactInfoByGroupsService
                                .remove(record.contactInfoByGroupId)
                                .subscribe(() => {
                                    const index = this.contactsData.findIndex(
                                        (d) =>
                                            d.customerContactId ===
                                            record.customerContactId
                                    ); // find index in your array
                                    this.contactsData.splice(index, 1); // remove element from array
                                    this.contactsDataSource =
                                        new MatTableDataSource(
                                            this.contactsData
                                        );
                                    this.contactsDataSource.sort = this.sort;
                                });
                        });
                } else {
                    if (record.firstName) {
                        this.contactInfoByGroupsService
                            .get({ oid: record.contactInfoByGroupId })
                            .subscribe((data: any) => {
                                if (data) {
                                    this.currentContactInfoByGroup = data;
                                    if (this.currentContactInfoByGroup.oid) {
                                        data.email = record.email;
                                        data.firstName = record.firstName;
                                        data.lastName = record.lastName;
                                        data.title = record.title;
                                        data.phone = record.phone;
                                        data.extension = record.extension;
                                        data.mobile = record.mobile;
                                        data.fax = record.fax;
                                        data.address = record.address;
                                        data.city = record.city;
                                        data.state = record.state;
                                        data.country = record.country;
                                        data.primary = record.primary;
                                        data.copyAlerts = record.copyAlerts;

                                        this.contactInfoByGroupsService
                                            .update(
                                                this.currentContactInfoByGroup
                                            )
                                            .subscribe(() => {});
                                    }
                                }
                            });
                    }
                }
            }
        });
    }

    public newRecord() {
        this.newContactClicked.emit();
    }

    public UpdateCopyAlertsValue(value) {
        if (value.copyAlerts) {
            value.copyAlerts = !value.copyAlerts;
        } else {
            value.copyAlerts = true;
        }
        const unselectedIndex = _.findIndex(
            this.contactsData,
            (contact) => !contact.copyAlerts
        );
        this.copyAll = unselectedIndex >= 0 ? false : true;

        value.groupId = this.sharedService.currentUser.groupId;

        this.contactInfoByGroupsService
            .update(value)
            .subscribe((data: any) => {});
    }

    public UpdateAllCopyAlertsValues() {
        this.copyAll = !this.copyAll;
        _.forEach(this.contactsData, (contact) => {
            contact.copyAlerts = this.copyAll;
            contact.GroupId = this.sharedService.currentUser.groupId;
            this.contactInfoByGroupsService
                .update(contact)
                .subscribe((data: any) => {});
        });
    }

    //[#hz0jtd] FlatFile importer was requested to be removed
    //async launchImporter() {
    //    if (!this.LICENSE_KEY) {
    //        return alert('Set LICENSE_KEY on Line 13 before continuing.');
    //    }
    //    try {
    //        const results = await this.importer.requestDataFromUser();
    //        this.importer.displayLoader();
    //        const customerId = this.route.snapshot.paramMap.get('id');
    //        if (results) {
    //            results.data.forEach((result) => {
    //                result.groupid = this.sharedService.currentUser.groupId;
    //                result.customerId = customerId;
    //            });
    //            this.contactInfoByGroupsService
    //                .import(results.data)
    //                .subscribe((data: any) => {
    //                    if (data) {
    //                        data.forEach((result) => {
    //                            this.contactsData.push(result);
    //                        });

    //                        this.contactsDataSource = new MatTableDataSource(
    //                            this.contactsData
    //                        );
    //                        this.contactsDataSource.sort = this.sort;

    //                        this.importer.displaySuccess(
    //                            'Data successfully imported!'
    //                        );
    //                    }
    //                });
    //        }
    //    } catch (e) {}
    //}

    //initializeImporter() {
    //    FlatfileImporter.setVersion(2);
    //    this.importer = new FlatfileImporter(this.LICENSE_KEY, {
    //        allowCustom: true,
    //        allowInvalidSubmit: true,
    //        disableManualInput: false,
    //        fields: [
    //            {
    //                alternates: ['first name'],
    //                description: 'Contact First Name',
    //                key: 'FirstName',
    //                label: 'First Name',
    //                validators: [
    //                    {
    //                        error: 'this field is required',
    //                        validate: 'required',
    //                    },
    //                ],
    //            },
    //            {
    //                alternates: ['last name'],
    //                description: 'Contact Last Name',
    //                key: 'LastName',
    //                label: 'Last Name',
    //                validators: [
    //                    {
    //                        error: 'this field is required',
    //                        validate: 'required',
    //                    },
    //                ],
    //            },
    //            {
    //                alternates: ['title'],
    //                description: 'Contact Title',
    //                key: 'Title',
    //                label: 'Title',
    //            },
    //            {
    //                alternates: ['email', 'email address'],
    //                description: 'Email Address',
    //                key: 'Email',
    //                label: 'Email',
    //            },
    //            {
    //                alternates: ['phone', 'phone number'],
    //                description: 'Phone Number',
    //                key: 'PhoneNumber',
    //                label: 'Phone Number',
    //            },
    //            {
    //                alternates: ['extension'],
    //                description: 'Phone Extension',
    //                key: 'Extension',
    //                label: 'Extension',
    //            },
    //            {
    //                alternates: [
    //                    'mobile',
    //                    'cell',
    //                    'mobile phone',
    //                    'cell phone',
    //                ],
    //                description: 'Mobile Phone',
    //                key: 'MobilePhone',
    //                label: 'Mobile',
    //            },
    //            {
    //                alternates: ['fax'],
    //                description: 'Fax',
    //                key: 'Fax',
    //                label: 'Fax',
    //            },
    //            {
    //                alternates: ['address', 'street address'],
    //                description: 'Street Address',
    //                key: 'Address',
    //                label: 'Address',
    //            },
    //            {
    //                alternates: ['city', 'town'],
    //                description: 'City',
    //                key: 'City',
    //                label: 'City',
    //            },
    //            {
    //                alternates: ['state'],
    //                description: 'State',
    //                key: 'State',
    //                label: 'State',
    //            },
    //            {
    //                alternates: ['country'],
    //                description: 'Country',
    //                key: 'Country',
    //                label: 'Country',
    //            },
    //            {
    //                alternates: ['primary'],
    //                description: 'Primary',
    //                key: 'PrimaryContact',
    //                label: 'Primary',
    //            },
    //            {
    //                alternates: ['copy on distribution'],
    //                description: 'Copy Contact on Distribution',
    //                key: 'CopyAlertsContact',
    //                label: 'Copy on Distribution',
    //            },
    //        ],
    //        managed: true,
    //        type: 'Contacts',
    //    });
    //    this.importer.setCustomer({
    //        name: 'WebsiteImport',
    //        userId: '1',
    //    });
    //}
}
