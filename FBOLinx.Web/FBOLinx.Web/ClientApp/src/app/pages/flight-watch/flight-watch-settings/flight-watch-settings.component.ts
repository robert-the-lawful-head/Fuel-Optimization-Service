import { ChangeDetectionStrategy, OnInit, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { Swim } from 'src/app/models/swim';
import {
    ColumnType,
    TableSettingsComponent,
} from 'src/app/shared/components/table-settings/table-settings.component';
import { SharedService } from '../../../layouts/shared-service';
import { FlightWatch } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from '../flight-watch-map/aircraft-images';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-settings',
    styleUrls: ['./flight-watch-settings.component.scss'],
    templateUrl: './flight-watch-settings.component.html',
})
export class FlightWatchSettingsComponent implements OnInit {
    @Input() tableData: Observable<FlightWatch[]>;
    @Input() swimArrivals: Swim[];
    @Input() swimDepartures: Swim[];
    @Input() icao: string;
    @Input() icaoList: string[];

    @Input() filteredTypes: string[];
    @Output() typesFilterChanged = new EventEmitter<string[]>();
    @Output() filterChanged = new EventEmitter<string>();
    @Output() icaoChanged = new EventEmitter<string>();


    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;

    displayedColumns: string[] = [
        "flightDepartment",
        "tailNumber",
        "make-model",
        "eta",
        "ete",
        "origin-destination",
        "city",
        "altitude",
        "isAircraftOnGround",
    ];

    tableLocalStorageKey: string;
    columns: ColumnType[] = [];
    searchIcaoTxt: string;

    constructor(private tableSettingsDialog: MatDialog,
        private sharedService: SharedService) {
        this.initColumns();
    }

    get visibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.id) || []
        );
    }

    ngOnInit() {
        this.sort.sortChange.subscribe(() => {
            this.columns = this.columns.map((column) =>
                column.id === this.sort.active
                ? { ...column, sort: this.sort.direction }
                : {
                    hidden: column.hidden,
                    id: column.id,
                    name: column.name,
                }
            );

            this.saveSettings();
        });
    }

    initColumns() {
        this.tableLocalStorageKey = `flight-watch-settings-${this.sharedService.currentUser.fboId}`;
        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(
                localStorage.getItem(this.tableLocalStorageKey)
            );
        } else {
            this.columns = [
                {
                    id: 'tailNumber',
                    name: 'tailNumber',
                },
                {
                    id: 'flightDepartment',
                    name: 'flightDepartment',
                    sort: 'desc',
                },
                {
                    id: 'make-model',
                    name: 'Make/Model',
                },
                {
                    id: 'ete',
                    name: 'ETE',
                },
                {
                    id: 'eta',
                    name: 'ETA',
                },
                {
                    id: 'origin-destination',
                    name: 'Origin/Destination',
                },
                {
                    id: 'city',
                    name: 'City'
                },
                {
                    id: 'altitude',
                    name: 'Altitude'
                },
                {
                    id: 'isAircraftOnGround',
                    name: 'On Ground',
                },
                {
                    id: 'eta-atd',
                    name: 'ETA/ATD',
                },
                {
                    id: 'itpMarginTemplate',
                    name: 'ITP Margin Template',
                },
                {
                    id: 'ppg',
                    name: 'PPG',
                },
                {
                    id: 'fuelCapacityGal',
                    name: 'Fuel Capacity',
                }
            ];
        }
    }

    get aircraftTypes() {
        return AIRCRAFT_IMAGES.filter((type) => type.label !== 'Other')
            .map((type) => ({
                aircraftType: type.id,
                color: type.fillColor,
                description: type.description,
                label: type.label,
            }))
            .concat({
                aircraftType: 'default',
                color: '#5fb4e6',
                description: '',
                label: 'Other',
            });
    }

    applyFilter(event: Event) {
        this.filterChanged.emit((event.target as HTMLInputElement).value);
    }

    toggleType(type: string) {
        if (this.filteredTypes.includes(type)) {
            if (type === 'default') {
                this.typesFilterChanged.emit(
                    this.filteredTypes.filter(
                        (ft) => !['B0', 'B3', 'default'].includes(ft)
                    )
                );
            } else {
                this.typesFilterChanged.emit(
                    this.filteredTypes.filter((ft) => ft !== type)
                );
            }
        } else {
            if (type === 'default') {
                this.typesFilterChanged.emit([
                    ...this.filteredTypes,
                    'B0',
                    'B3',
                    'default',
                ]);
            } else {
                this.typesFilterChanged.emit([...this.filteredTypes, type]);
            }
        }
    }

    refreshSort() {
        const sortedColumn = this.columns.find(
            (column) => !column.hidden && column.sort
        );
        this.sort.sort({
            disableClear: false,
            id: null,
            start: sortedColumn?.sort || 'asc',
        });
        this.sort.sort({
            disableClear: false,
            id: sortedColumn?.id,
            start: sortedColumn?.sort || 'asc',
        });
        (
            this.sort.sortables.get(sortedColumn?.id) as MatSortHeader
        )?._setAnimationTransitionState({ toState: 'active' });
    }

    openSettings() {
        const dialogRef = this.tableSettingsDialog.open(
            TableSettingsComponent,
            {
                data: this.columns,
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.columns = [...result];

            this.refreshSort();
            this.saveSettings();
        });
    }

    saveSettings() {
        localStorage.setItem(
            this.tableLocalStorageKey,
            JSON.stringify(this.columns)
        );
    }
    updateIcao(event: any ){
        console.log("🚀 ~ file: flight-watch-settings.component.ts ~ line 240 ~ FlightWatchSettingsComponent ~ updateIcao ~ event", event);
        this.icaoChanged.emit(event);
    }
}
