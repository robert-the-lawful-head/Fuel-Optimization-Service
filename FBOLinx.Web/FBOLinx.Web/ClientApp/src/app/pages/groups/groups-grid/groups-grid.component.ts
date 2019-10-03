import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { GroupsService } from '../../../services/groups.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { GroupsDialogNewGroupComponent } from '../groups-dialog-new-group/groups-dialog-new-group.component';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Groups',
        link: ''
    }
];

@Component({
    selector: 'app-groups-grid',
    templateUrl: './groups-grid.component.html',
    styleUrls: ['./groups-grid.component.scss']
})
/** groups-grid component*/
export class GroupsGridComponent implements OnInit  {

    //Input/Output Bindings
    @Output() recordDeleted = new EventEmitter<any>();
    @Output() newGroupClicked = new EventEmitter<any>();
    @Output() editGroupClicked = new EventEmitter<any>();
    @Input() groupsData: Array<any>;

    //Public Members
    public pageTitle: string = 'Groups';
    public breadcrumb: any[] = BREADCRUMBS;
    public groupsDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['group', 'active', 'delete'];
    public resultsLength: number = 0;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    /** groups-grid ctor */
    constructor(public newGroupDialog: MatDialog,
        private groupsService: GroupsService,
        private sharedService: SharedService,
        public deleteGroupDialog: MatDialog) {

    }

    ngOnInit() {
        this.sharedService.emitChange(this.pageTitle);
        if (!this.groupsData)
            return;
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
        this.groupsDataSource = new MatTableDataSource(this.groupsData);
        this.groupsDataSource.sort = this.sort;
        this.groupsDataSource.paginator = this.paginator;
        this.resultsLength = this.groupsData.length;
    }

    /** Public Methods */
    public deleteRecord(record) {
        const dialogRef = this.deleteGroupDialog.open(DeleteConfirmationComponent, {
            data: { item: record, description: 'group' }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            const deleteIndex = this.groupsData.indexOf(record);
            this.groupsService.remove(record).subscribe(
                result => { this.groupsData.splice(deleteIndex, 1); }
            );
            this.recordDeleted.emit(record);
        });
    }

    public editRecord(record) {
        const clonedRecord = Object.assign({}, record);
        this.editGroupClicked.emit(clonedRecord);
    }

    public newRecord() {
        const dialogRef = this.newGroupDialog.open(GroupsDialogNewGroupComponent, {
            width: '450px',
            data: { oid: 0, initialSetupPhase: true }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('Dialog data: ', result);
            result.active = true;
            this.groupsService.add(result).subscribe((data: any) => {
                this.editRecord(data);
            });
        });
        this.newGroupClicked.emit();
    }

    public applyFilter(filterValue: string) {
        this.groupsDataSource.filter = filterValue.trim().toLowerCase();
    }
}
