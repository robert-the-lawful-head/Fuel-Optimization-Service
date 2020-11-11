import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
    ChangeDetectionStrategy,
    OnChanges,
    SimpleChanges
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

// Services
import { SharedService } from '../../../layouts/shared-service';

// Shared components
import { FuelReqsExportModalComponent } from '../../../shared/components/fuelreqs-export/fuelreqs-export.component';

@Component({
    selector: 'app-fuelreqs-grid',
    templateUrl: './fuelreqs-grid.component.html',
    styleUrls: ['./fuelreqs-grid.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuelreqsGridComponent implements OnInit, OnChanges {
    @Output() dateFilterChanged = new EventEmitter<any>();
    @Output() exportTriggered = new EventEmitter<any>();
    @Input() fuelreqsData: any[];
    @Input() filterStartDate: Date;
    @Input() filterEndDate: Date;

    public pageIndex = 0;
    public pageSize = 100;


    fuelreqsDataSource: MatTableDataSource<any> = null;
    resultsLength = 0;
    displayedColumns: string[] = [
        'oid',
        'customer',
        'pricingTemplateName',
        'eta',
        'etd',
        'quotedVolume',
        'quotedPpg',
        'tailNumber',
        'email',
        'phoneNumber',
        'source',
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
    ngOnChanges(changes: SimpleChanges): void {
        if (changes.fuelreqsData) {
            this.refreshTable();
        }
    }

    ngOnInit() {
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.sort.active = 'eta';

        if (localStorage.getItem('pageIndexFuelReqs')) {
            this.paginator.pageIndex = localStorage.getItem('pageIndexFuelReqs') as any;
        } else {
            this.paginator.pageIndex = 0;
        }

        if (sessionStorage.getItem('pageSizeValueFuelReqs')) {
            this.pageSize = sessionStorage.getItem('pageSizeValueFuelReqs') as any;
        } else {
            this.pageSize = 100;
        }
        this.refreshTable();
    }

    public refreshTable() {
        let filter = '';
        if (this.fuelreqsDataSource) {
            filter = this.fuelreqsDataSource.filter;
        }
        this.fuelreqsDataSource = new MatTableDataSource(this.fuelreqsData);
        this.fuelreqsDataSource.sort = this.sort;
        this.fuelreqsDataSource.paginator = this.paginator;
        this.fuelreqsDataSource.filter = filter;
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

    onPageChanged(event: any) {
        localStorage.setItem('pageIndexFuelReqs', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeValueFuelReqs',
            this.paginator.pageSize.toString()
        );
    }
}
