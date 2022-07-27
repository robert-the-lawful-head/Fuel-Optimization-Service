import { ChangeDetectionStrategy, OnInit, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { Observable } from 'rxjs';
import { SharedService } from 'src/app/layouts/shared-service';
import { SwimFilter } from 'src/app/models/filter';
import { Swim } from 'src/app/models/swim';
import { ColumnType, TableSettingsComponent } from 'src/app/shared/components/table-settings/table-settings.component';
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
    @Output() filterChanged = new EventEmitter<SwimFilter>();
    @Output() icaoChanged = new EventEmitter<string>();
    @Output() openAircraftPopup = new EventEmitter<string>();
   
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    searchIcaoTxt: string;

    columns: ColumnType[] = [];
    tableLocalStorageKey: string;

    constructor(private tableSettingsDialog: MatDialog,private sharedService: SharedService) {
        this.initColumns();
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
        let filter: SwimFilter = {
            filterText: (event.target as HTMLInputElement).value,
            dataType: null
        };
      this.filterChanged.emit(filter);
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
    updateIcao(event: any ){
        this.icaoChanged.emit(event);
    }
    openPopup(tailnumber: string): void{
        this.openAircraftPopup.emit(tailnumber);
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
                    name: 'City',
                },
                {
                    id: 'altitude',
                    name: 'Altitude',
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
                },
            ];
        }
    }
    saveSettings() {
        localStorage.setItem(
            this.tableLocalStorageKey,
            JSON.stringify(this.columns)
        );
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
}
