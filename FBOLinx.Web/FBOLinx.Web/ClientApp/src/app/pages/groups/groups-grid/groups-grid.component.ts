import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
} from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatDialog } from "@angular/material/dialog";

// Services
import { GroupsService } from "../../../services/groups.service";
import { SharedService } from "../../../layouts/shared-service";

// Components
import { GroupsDialogNewGroupComponent } from "../groups-dialog-new-group/groups-dialog-new-group.component";
import { DeleteConfirmationComponent } from "../../../shared/components/delete-confirmation/delete-confirmation.component";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "/default-layout",
    },
    {
        title: "Groups",
        link: "",
    },
];

@Component({
    selector: "app-groups-grid",
    templateUrl: "./groups-grid.component.html",
    styleUrls: ["./groups-grid.component.scss"],
})
export class GroupsGridComponent implements OnInit {
    // Input/Output Bindings
    @Output() recordDeleted = new EventEmitter<any>();
    @Output() newGroupClicked = new EventEmitter<any>();
    @Output() editGroupClicked = new EventEmitter<any>();
    @Input() groupsData: Array<any>;

    // Public Members
    public pageTitle = "Groups";
    public breadcrumb: any[] = BREADCRUMBS;
    public groupsDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ["group", "active", "delete"];
    public resultsLength = 0;

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor(
        public newGroupDialog: MatDialog,
        private groupsService: GroupsService,
        private sharedService: SharedService,
        public deleteGroupDialog: MatDialog,
        private snackBar: MatSnackBar
    ) {}

    ngOnInit() {
        this.sharedService.titleChange(this.pageTitle);
        if (!this.groupsData) {
            return;
        }
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.groupsDataSource = new MatTableDataSource(this.groupsData);
        this.groupsDataSource.sort = this.sort;
        this.groupsDataSource.paginator = this.paginator;
        this.resultsLength = this.groupsData.length;
    }

    public deleteRecord(record) {
        const dialogRef = this.deleteGroupDialog.open(
            DeleteConfirmationComponent,
            {
                data: { item: record, fullDescription: "You are about to remove this group. This will remove the fbos and all the other data related to the group. Are you sure?" },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            const deleteIndex = this.groupsData.indexOf(record);
            const filter = this.groupsDataSource.filter;
            this.groupsService.remove(record).subscribe(() => {
                this.groupsData.splice(deleteIndex, 1);
                this.groupsDataSource = new MatTableDataSource(this.groupsData);
                this.groupsDataSource.sort = this.sort;
                this.groupsDataSource.paginator = this.paginator;
                this.groupsDataSource.filter = filter;
                this.snackBar.open(record.groupName + " is deleted", "", {
                    duration: 2000,
                    panelClass: ["blue-snackbar"],
                });
            });
            this.recordDeleted.emit(record);
        });
    }

    public editRecord(record) {
        const clonedRecord = Object.assign({}, record);
        this.editGroupClicked.emit(clonedRecord);
    }

    public newRecord() {
        const dialogRef = this.newGroupDialog.open(
            GroupsDialogNewGroupComponent,
            {
                width: "450px",
                data: { oid: 0, initialSetupPhase: true },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
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
