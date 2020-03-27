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

// Services
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-fuelreqs-grid",
    templateUrl: "./fuelreqs-grid.component.html",
    styleUrls: ["./fuelreqs-grid.component.scss"],
})
export class FuelreqsGridComponent implements OnInit {
    @Output() fuelreqDeleted = new EventEmitter<any>();
    @Output() newFuelreqClicked = new EventEmitter<any>();
    @Output() editFuelreqClicked = new EventEmitter<any>();
    @Output() dateFilterChanged = new EventEmitter<any>();
    @Input() fuelreqsData: Array<any>;
    @Input() filterStartDate: Date;
    @Input() filterEndDate: Date;

    fuelreqsDataSource: MatTableDataSource<any> = null;
    resultsLength = 0;
    displayedColumns: string[] = [
        "oid",
        "customer",
        "eta",
        "icao",
        "tailNumber",
        "fbo",
        "dispatchNotes",
        "source",
    ];
    public dashboardSettings: any;

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor(private sharedService: SharedService) {
        this.dashboardSettings = this.sharedService.dashboardSettings;
    }

    ngOnInit() {
        if (!this.fuelreqsData) {
            return;
        }
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.fuelreqsDataSource = new MatTableDataSource(this.fuelreqsData);
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
            filterEndDate: this.filterEndDate
        });
    }
}
