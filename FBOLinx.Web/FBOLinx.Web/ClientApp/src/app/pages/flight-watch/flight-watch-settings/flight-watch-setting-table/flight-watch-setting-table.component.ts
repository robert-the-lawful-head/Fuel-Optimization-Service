import { ElementRef, OnInit, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatChipSelectionChange } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { tail } from 'lodash';
import { SharedService } from 'src/app/layouts/shared-service';
import { SwimFilter } from 'src/app/models/filter';
import { Swim, SwimType } from 'src/app/models/swim';
import {
    ColumnType,
    TableSettingsComponent,
} from 'src/app/shared/components/table-settings/table-settings.component';
import { FlightWatchMapComponent } from '../../flight-watch-map/flight-watch-map.component';

@Component({
    selector: 'app-flight-watch-setting-table',
    templateUrl: './flight-watch-setting-table.component.html',
    styleUrls: ['./flight-watch-setting-table.component.scss'],
})
export class FlightWatchSettingTableComponent implements OnInit {
    @Input() data: Swim[];
    @Input() isArrival: boolean;

    @Output() filterChanged = new EventEmitter<SwimFilter>();
    @Output() openAircraftPopup = new EventEmitter<string>();

    @ViewChild(MatSort, { static: true }) sort: MatSort;

    columns: ColumnType[] = [];
    tableLocalStorageKey: string;

    constructor(private sharedService: SharedService,
      private tableSettingsDialog: MatDialog) {
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
    getSwimDataTypeString(){
      return (this.isArrival)? 'Arrivals': 'Departures';
    }
    initColumns() {
        this.tableLocalStorageKey = `flight-watch-settings-${this.sharedService.currentUser.fboId}-${this.getSwimDataTypeString()}`;
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
    get visibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.id) || []
        );
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
    applyFilter(event: Event) {
        let filter: SwimFilter = {
            filterText: (event.target as HTMLInputElement).value,
            dataType: this.isArrival ? SwimType.Arrival: SwimType.Departure
        };
      this.filterChanged.emit(filter);
    }
    openAircraftPopUpOnMap(row: any){
        this.openAircraftPopup.emit(row.tailnumber);
    }
}
