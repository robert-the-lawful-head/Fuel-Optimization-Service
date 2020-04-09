import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
} from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";

// Services
import { SharedService } from "../../../layouts/shared-service";

// Shared components
import { FuelReqsExportModalComponent } from "../../../shared/components/fuelreqs-export/fuelreqs-export.component";

@Component({
    selector: "app-fuelreqs-grid",
    templateUrl: "./fuelreqs-grid.component.html",
    styleUrls: ["./fuelreqs-grid.component.scss"],
})
export class FuelreqsGridComponent implements OnInit {
    @Output() dateFilterChanged = new EventEmitter<any>();
    @Output() exportTriggered = new EventEmitter<any>();
    @Input() fuelreqsData: Array<any>;
    @Input() filterStartDate: Date;
    @Input() filterEndDate: Date;

    fuelreqsDataSource: MatTableDataSource<any> = null;
    resultsLength = 0;
    displayedColumns: string[] = [
        "oid",
        "customer",
        "eta",
        "etd",
        "quotedVolume",
        "quotedPpg",
        "tailNumber",
        "fboEmail",
        "fboPhoneNumber",
        "dispatchNotes",
        "source",
    ];
    public dashboardSettings: any;

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor(
        private sharedService: SharedService,
        public exportDialog: MatDialog
    ) {
        this.dashboardSettings = this.sharedService.dashboardSettings;
    }

    ngOnInit() {
        if (!this.fuelreqsData) {
            return;
        }
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.fuelreqsDataSource = new MatTableDataSource(this.fuelreqsData);
        this.sort.active = "eta";
        this.fuelreqsDataSource.sort = this.sort;
        this.fuelreqsDataSource.paginator = this.paginator;
        this.resultsLength = this.fuelreqsData.length;
    }

    public applyFilter(filterValue: string) {
        this.fuelreqsDataSource.filter = filterValue.trim().toLowerCase();
    }

    public applyDateFilterChange() {
        this.dateFilterChanged.emit({
            filterStartDate: this.filterStartDate,
            filterEndDate: this.filterEndDate,
        });
    }

    public export() {
        const dialogRef = this.exportDialog.open(
            FuelReqsExportModalComponent,
            {
                data: {
                    filterStartDate: this.filterStartDate,
                    filterEndDate: this.filterEndDate,
                },
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.exportTriggered.emit(result);
        });
    }
}
