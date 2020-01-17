import { Component, EventEmitter, Input, Output, OnInit, ViewChild} from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { ContactsService } from '../../../services/contacts.service';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { CustomercontactsService } from '../../../services/customercontacts.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { ContactinfobygroupsService } from '../../../services/contactinfobygroups.service';

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

    contactsDataSource: MatTableDataSource<any> = null;
    displayedColumns: string[] = ['firstName', 'lastName', 'title', 'email', 'phone', 'copyAlerts', 'delete'];

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    /** contacts-grid ctor */
    constructor(public deleteUserDialog: MatDialog,
        private contactsService: ContactsService,
        private contactInfoByGroupsService: ContactinfobygroupsService,
        private customerContactsService: CustomercontactsService) {
        
    }

    ngOnInit() {
        if (!this.contactsData)
            return;
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
        this.contactsDataSource = new MatTableDataSource(this.contactsData);
        this.contactsDataSource.sort = this.sort;
        this.contactsDataSource.paginator = this.paginator;
    }

    //Public Methods
    public deleteRecord(record) {
        this.customerContactsService.remove(record.customerContactId).subscribe((data: any) => {
            this.contactInfoByGroupsService.remove(record.contactInfoByGroupId).subscribe((data: any) => {
                //delete this.contactsData[contact];
                //this.contactsData = this.cont.splice(contact.contactInfoByGroupId, 1);
                let index = this.contactsData.findIndex(d => d.customerContactId === record.customerContactId); //find index in your array
                this.contactsData.splice(index, 1);//remove element from array
                this.contactsDataSource = new MatTableDataSource(this.contactsData);
                this.contactsDataSource.sort = this.sort;
                this.contactsDataSource.paginator = this.paginator;
            });
        });
        //this.contactDeleted.emit(record);
    }

    public editRecord(record,$event) {
        const clonedRecord = Object.assign({}, record);
        console.log(clonedRecord);
        this.editContactClicked.emit(clonedRecord);;
    }

    public newRecord() {
        this.newContactClicked.emit();
    }
}
