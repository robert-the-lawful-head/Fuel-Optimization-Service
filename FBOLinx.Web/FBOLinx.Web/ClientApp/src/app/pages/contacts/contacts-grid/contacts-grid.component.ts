import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
} from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { MatDialog } from "@angular/material/dialog";
import * as _ from "lodash";
import FlatfileImporter from "flatfile-csv-importer";

// Services
import { CustomercontactsService } from "../../../services/customercontacts.service";
import { ContactinfobygroupsService } from "../../../services/contactinfobygroups.service";
import { SharedService } from "../../../layouts/shared-service";
import { ContactsDialogNewContactComponent } from "../contacts-edit-modal/contacts-edit-modal.component";

@Component({
    selector: "app-contacts-grid",
    templateUrl: "./contacts-grid.component.html",
    styleUrls: ["./contacts-grid.component.scss"],
})
export class ContactsGridComponent implements OnInit {
    @Output() contactDeleted = new EventEmitter<any>();
    @Output() newContactClicked = new EventEmitter<any>();
    @Output() editContactClicked = new EventEmitter<any>();
    @Input() contactsData: Array<any>;
    public currentContactInfoByGroup: any;
    contactsDataSource: MatTableDataSource<any> = null;
    displayedColumns: string[] = [
        "firstName",
        "lastName",
        "title",
        "email",
        "phone",
        "copyAlerts",
        "delete",
    ];
    public copyAll = false;

    @ViewChild(MatSort, { static: true }) sort: MatSort;

    LICENSE_KEY = "9eef62bd-4c20-452c-98fd-aa781f5ac111";

    results = "[]";

    private importer: FlatfileImporter;

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

        const foundedIndex = _.findIndex(this.contactsData, (contact) => {
            return !contact.copyAlerts;
        });
        this.copyAll = foundedIndex >= 0 ? false : true;
        this.sort.sortChange.subscribe(() => {});
        this.contactsDataSource = new MatTableDataSource(this.contactsData);
        this.contactsDataSource.sort = this.sort;

        FlatfileImporter.setVersion(2);
        this.initializeImporter();
        this.importer.setCustomer({
            userId: "1",
            name: "WebsiteImport",
        });
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
            if ($event.target.className.indexOf("mat-slide-toggle") > -1) {
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

    public EditContactPopup(record) {
        const dialogRef = this.newContactDialog.open(
            ContactsDialogNewContactComponent,
            {
                data: record,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (result !== "cancel") {
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
                                    this.contactsDataSource = new MatTableDataSource(
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
        const unselectedIndex = _.findIndex(this.contactsData, (contact) => {
            return !contact.copyAlerts;
        });
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

    async launchImporter() {
        if (!this.LICENSE_KEY) {
            return alert("Set LICENSE_KEY on Line 13 before continuing.");
        }
        try {
            const results = await this.importer.requestDataFromUser();
            this.importer.displayLoader();
            const customerId = this.route.snapshot.paramMap.get("id");
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
                                "Data successfully imported!"
                            );
                        }
                    });
            }
        } catch (e) { }
    }

    initializeImporter() {
        this.importer = new FlatfileImporter(this.LICENSE_KEY, {
            fields: [
                {
                    label: "First Name",
                    alternates: ["first name"],
                    key: "FirstName",
                    description: "Contact First Name",
                    validators: [
                        {
                            validate: "required",
                            error: "this field is required",
                        },
                    ],
                },
                {
                    label: "Last Name",
                    alternates: ["last name"],
                    key: "LastName",
                    description: "Contact Last Name",
                    validators: [
                        {
                            validate: "required",
                            error: "this field is required",
                        },
                    ],
                },
                {
                    label: "Title",
                    alternates: ["title"],
                    key: "Title",
                    description: "Contact Title",
                },
                {
                    label: "Email",
                    alternates: ["email", "email address"],
                    key: "Email",
                    description: "Email Address",
                },
                {
                    label: "Phone Number",
                    alternates: ["phone", "phone number"],
                    key: "PhoneNumber",
                    description: "Phone Number",
                },
                {
                    label: "Extension",
                    alternates: ["extension"],
                    key: "Extension",
                    description: "Phone Extension",
                },
                {
                    label: "Mobile",
                    alternates: ["mobile", "cell", "mobile phone", "cell phone"],
                    key: "MobilePhone",
                    description: "Mobile Phone",
                },
                {
                    label: "Fax",
                    alternates: ["fax"],
                    key: "Fax",
                    description: "Fax",
                },
                {
                    label: "Address",
                    alternates: ["address", "street address"],
                    key: "Address",
                    description: "Street Address",
                },
                {
                    label: "City",
                    alternates: ["city", "town"],
                    key: "City",
                    description: "City",
                },
                {
                    label: "State",
                    alternates: ["state"],
                    key: "State",
                    description: "State",
                },
                {
                    label: "Country",
                    alternates: ["country"],
                    key: "Country",
                    description: "Country",
                },
                {
                    label: "Primary",
                    alternates: ["primary"],
                    key: "PrimaryContact",
                    description: "Primary",
                },
                {
                    label: "Copy on Distribution",
                    alternates: ["copy on distribution"],
                    key: "CopyAlertsContact",
                    description: "Copy Contact on Distribution",
                },
            ],
            type: "Contacts",
            allowInvalidSubmit: true,
            managed: true,
            allowCustom: true,
            disableManualInput: false,
        });
    }
}
