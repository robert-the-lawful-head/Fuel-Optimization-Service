import { ChangeDetectionStrategy, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { SharedService } from 'src/app/layouts/shared-service';
import { SwimFilter } from 'src/app/models/filter';
import { Swim, swimTableColumns } from 'src/app/models/swim';
import { ColumnType, TableSettingsComponent } from 'src/app/shared/components/table-settings/table-settings.component';
import { FlightWatch } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from '../flight-watch-map/aircraft-images';
import { FlightWatchSettingTableComponent } from './flight-watch-setting-table/flight-watch-setting-table.component';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-settings',
    styleUrls: ['./flight-watch-settings.component.scss'],
    templateUrl: './flight-watch-settings.component.html',
})
export class FlightWatchSettingsComponent {
    @Input() swimArrivals: Swim[];
    @Input() swimDepartures: Swim[];
    @Input() icao: string;
    @Input() icaoList: string[];
    @Input() filteredTypes: string[];

    @Output() typesFilterChanged = new EventEmitter<string[]>();
    @Output() filterChanged = new EventEmitter<SwimFilter>();
    @Output() icaoChanged = new EventEmitter<string>();
    @Output() openAircraftPopup = new EventEmitter<string>();
    @Output() updateDrawerButtonPosition = new EventEmitter<any>();

    @ViewChild('arrivalsTable') public arrivalsTable: FlightWatchSettingTableComponent;
    @ViewChild('departuresTable') public departuresTable: FlightWatchSettingTableComponent;


    searchIcaoTxt: string;

    columns: ColumnType[] = [];
    tableLocalStorageKey: string;

    constructor(private tableSettingsDialog: MatDialog,private sharedService: SharedService) {
        this.initColumns();
    }

    ngOnChanges(changes: SimpleChanges) {
        if((changes.swimArrivals && !changes.swimArrivals.previousValue)
        || (changes.swimDepartures && !changes.swimDepartures.previousValue)){
            this.updateDrawerButtonPosition.emit();
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
        this.swimArrivals = null;
        this.swimDepartures = null;
        this.updateDrawerButtonPosition.emit();
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
            this.updateDrawerButtonPosition.emit();
            this.arrivalsTable.refreshSort();
            this.departuresTable.refreshSort();
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
                    id: 'status',
                    name: swimTableColumns.status,
                },
                {
                    id: 'tailNumber',
                    name: swimTableColumns.tailNumber,
                },
                {
                    id: 'flightDepartment',
                    name: swimTableColumns.flightDepartment,
                    sort: 'desc',
                },
                {
                    id: 'icaoAircraftCode',
                    name: swimTableColumns.icaoAircraftCode,
                },
                {
                    id: 'ete',
                    name: swimTableColumns.ete,
                },
                {
                    id: 'eta-atd',
                    name: swimTableColumns.etaAtd,
                },
                {
                    id: 'origin-destination',
                    name: swimTableColumns.originDestination,
                },
                {
                    id: 'isAircraftOnGround',
                    name: swimTableColumns.isAircraftOnGround,
                },
                {
                    id: 'itpMarginTemplate',
                    name: swimTableColumns.itpMarginTemplate,
                }
            ];
        }
    }
    saveSettings() {
        localStorage.setItem(
            this.tableLocalStorageKey,
            JSON.stringify(this.columns)
        );
    }
    sortChangeSaveSettings(){
        this.saveSettings();
    }
}
