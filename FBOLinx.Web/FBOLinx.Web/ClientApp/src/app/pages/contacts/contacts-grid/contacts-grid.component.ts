import { Component, EventEmitter, Input, Output, OnInit, ViewChild} from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import * as _ from 'lodash';

//Services
import { ContactsService } from '../../../services/contacts.service';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { CustomercontactsService } from '../../../services/customercontacts.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { ContactinfobygroupsService } from '../../../services/contactinfobygroups.service';
import { SharedService } from '../../../layouts/shared-service';
import { ContactsDialogNewContactComponent } from '../contacts-edit-modal/contacts-edit-modal.component';


@Component({
    selector: 'app-contacts-grid',
    templateUrl: './contacts-grid.component.html',
    styleUrls: ['./contacts-grid.component.scss']
})
/** contacts-grid component*/
export class ContactsGridComponent {

    @Output() contactDeleted = new EventEmitter<any>();
    @Output() newContactClicked = new EventEmitter<any>();
    @Output() editContactClicked = new EventEmitter<any>();
    @Input() contactsData: Array<any>;
    public currentContactInfoByGroup: any;
    contactsDataSource: MatTableDataSource<any> = null;
    displayedColumns: string[] = ['firstName', 'lastName', 'title', 'email', 'phone', 'copyAlerts', 'delete'];
    public copyAll: boolean = false;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    /** contacts-grid ctor */
    constructor(public deleteUserDialog: MatDialog,
        private contactsService: ContactsService,
        private contactInfoByGroupsService: ContactinfobygroupsService,
        private sharedService: SharedService,
        private customerContactsService: CustomercontactsService, public newContactDialog: MatDialog) {
        
    }

    ngOnInit() {
        if (!this.contactsData) return;

        let foundedIndex = _.findIndex(this.contactsData, contact => {
            return !contact.copyAlerts;
        });
        this.copyAll = foundedIndex >= 0 ? false : true;
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
        this.contactsDataSource = new MatTableDataSource(this.contactsData);
        this.contactsDataSource.sort = this.sort;
        this.contactsDataSource.paginator = this.paginator;
    }

    //Public Methods
    public deleteRecord(record) {
        this.customerContactsService.remove(record.customerContactId).subscribe((data: any) => {
            this.contactInfoByGroupsService.remove(record.contactInfoByGroupId).subscribe((data: any) => {
                let index = this.contactsData.findIndex(d => d.customerContactId === record.customerContactId); //find index in your array
                this.contactsData.splice(index, 1);//remove element from array
                this.contactsDataSource = new MatTableDataSource(this.contactsData);
                this.contactsDataSource.sort = this.sort;
                this.contactsDataSource.paginator = this.paginator;
            });
        });
    }

    public editRecord(record, $event) {
        if ($event.target) {
            if ($event.target.className.indexOf('mat-slide-toggle') > -1) {
                $event.stopPropagation();
                return false;
            }
            else {
                const clonedRecord = Object.assign({}, record);
                this.editContactClicked.emit(clonedRecord);;
            }
        }
        else {
            const clonedRecord = Object.assign({}, record);
            this.editContactClicked.emit(clonedRecord);
        }
        
    }

    public EditContactPopup(record) {
        const dialogRef = this.newContactDialog.open(ContactsDialogNewContactComponent, {
            data: record
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result != 'cancel') {
                if (result.toDelete) {

                    this.customerContactsService.remove(record.customerContactId).subscribe((data: any) => {
                        this.contactInfoByGroupsService.remove(record.contactInfoByGroupId).subscribe((data: any) => {
                            let index = this.contactsData.findIndex(d => d.customerContactId === record.customerContactId); //find index in your array
                            this.contactsData.splice(index, 1);//remove element from array
                            this.contactsDataSource = new MatTableDataSource(this.contactsData);
                            this.contactsDataSource.sort = this.sort;
                            this.contactsDataSource.paginator = this.paginator;
                        });
                    });
                }
                else {
                    if (record.firstName) {
                        this.contactInfoByGroupsService.get({ oid: record.contactInfoByGroupId }).subscribe((data: any) => {
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

                                    this.contactInfoByGroupsService.update(this.currentContactInfoByGroup).subscribe((data: any) => {

                                    });

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
        }
        else {
            value.copyAlerts = true;
        }
        let unselectedIndex = _.findIndex(this.contactsData, contact => {
            return !contact.copyAlerts;
        });
        this.copyAll = unselectedIndex >= 0 ? false : true;
        
        value.GroupId = this.sharedService.currentUser.groupId;
        this.contactInfoByGroupsService.update(value).subscribe((data: any) => {
        });
    }

    public UpdateAllCopyAlertsValues() {
        this.copyAll = !this.copyAll;
        _.forEach(this.contactsData, contact => {
            contact.copyAlerts = this.copyAll;
            contact.GroupId = this.sharedService.currentUser.groupId;
            this.contactInfoByGroupsService.update(contact).subscribe((data: any) => {
            }); 
        });
    }
}
