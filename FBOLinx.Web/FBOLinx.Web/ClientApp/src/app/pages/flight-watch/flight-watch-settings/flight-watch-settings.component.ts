import { ChangeDetectionStrategy, SimpleChanges, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SharedService } from 'src/app/layouts/shared-service';
import { SwimFilter } from 'src/app/models/filter';
import { swimTableColumns } from 'src/app/models/swim';
import { ColumnType, TableSettingsComponent } from 'src/app/shared/components/table-settings/table-settings.component';
import { FlightWatchModelResponse } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from '../flight-watch-map/aircraft-images';
import { FlightWatchSettingTableComponent } from './flight-watch-setting-table/flight-watch-setting-table.component';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-settings',
    styleUrls: ['./flight-watch-settings.component.scss'],
    templateUrl: './flight-watch-settings.component.html',
})
export class FlightWatchSettingsComponent {
    @Input() arrivals: FlightWatchModelResponse[];
    @Input() departures: FlightWatchModelResponse[];
    @Input() icao: string;
    @Input() icaoList: string[];
    @Input() filteredTypes: string[];
    @Input() showFilters: boolean =  true;
    @Input() isLobbyView: boolean =  false;

    @Output() typesFilterChanged = new EventEmitter<string[]>();
    @Output() filterChanged = new EventEmitter<SwimFilter>();
    @Output() icaoChanged = new EventEmitter<string>();
    @Output() updateDrawerButtonPosition = new EventEmitter<any>();

    @ViewChild('arrivalsTable') public arrivalsTable: FlightWatchSettingTableComponent;
    @ViewChild('departuresTable') public departuresTable: FlightWatchSettingTableComponent;


    searchIcaoTxt: string;

    columns: ColumnType[] = [];
    arrivalsColumns: ColumnType[] = [];
    departuresColumns: ColumnType[] = [];
    tableLocalStorageKey: string;

    constructor(private tableSettingsDialog: MatDialog,private sharedService: SharedService) {
    }
    ngOnInit(): void {
        this.initColumns();
    }
    ngOnChanges(changes: SimpleChanges) {
        if(!changes.arrivals?.previousValue && !changes.departures?.previousValue)
        this.updateDrawerButtonPosition.emit();
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
        this.arrivals = null;
        this.departures = null;
        this.updateDrawerButtonPosition.emit();
        this.icaoChanged.emit(event);
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
        this.tableLocalStorageKey  = `flight-watch-settings-${this.sharedService.currentUser.fboId}`;

        let savedColumns = null;
        if(!this.isLobbyView){
            savedColumns = this.getClientSavedColumns();
        }

        if(savedColumns == null)
            this.columns = this.getColumnDefinition(null);
        else
            this.columns = savedColumns;

        this.arrivalsColumns = this.getColumnDefinition(true);
        this.departuresColumns = this.getColumnDefinition(false);
    }
    private getClientSavedColumns(){
        let localStorageColumns: string = localStorage.getItem(this.tableLocalStorageKey);
        let hasColumnUpdates :boolean = false;

        if (localStorageColumns) {
            let storedCols: ColumnType[] = JSON.parse(localStorageColumns);
            for(let col in storedCols){
                let colName = storedCols[col].id;
                if(swimTableColumns[colName] != undefined) continue;
                hasColumnUpdates = true;
                break;
            }
            if(!hasColumnUpdates){
                return storedCols;
            }
        }
        return null;
    }
    private getColumnDefinition(isArrival: boolean = null): ColumnType[]{
        return [
            {
                id: swimTableColumns.status,
                name: swimTableColumns.status,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.status)
            },
            {
                id: swimTableColumns.tailNumber,
                name: swimTableColumns.tailNumber,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.tailNumber)
            },
            {
                id: swimTableColumns.flightDepartment,
                name: swimTableColumns.flightDepartment,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.flightDepartment),
                sort: 'desc',
            },
            {
                id: swimTableColumns.icaoAircraftCode,
                name: swimTableColumns.icaoAircraftCode,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.icaoAircraftCode)
            },
            {
                id: swimTableColumns.ete,
                name: swimTableColumns.ete,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.ete)
            },
            {
                id: swimTableColumns.atd,
                name: swimTableColumns.atd,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.atd)
            },
            {
                id: swimTableColumns.eta,
                name: swimTableColumns.eta,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.eta)
            },
            {
                id: swimTableColumns.originAirport,
                name: swimTableColumns.originAirport,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.originAirport)
            },
            {
                id: swimTableColumns.originCity,
                name: swimTableColumns.originCity,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.originCity)
            },
            {
                id: swimTableColumns.destinationAirport,
                name: swimTableColumns.destinationAirport,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.destinationAirport)
            },
            {
                id: swimTableColumns.destinationCity,
                name: swimTableColumns.destinationCity,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.destinationCity)
            },
            {
                id: swimTableColumns.makeModel,
                name: swimTableColumns.makeModel,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.makeModel)
            },
            {
                id: swimTableColumns.isAircraftOnGround,
                name: swimTableColumns.isAircraftOnGround,
                hidden : this.isHiddenColumn(isArrival,swimTableColumns.isAircraftOnGround)
            },
            {
                id: swimTableColumns.itpMarginTemplate,
                name: swimTableColumns.itpMarginTemplate,
                hidden : this.isHiddenColumn(isArrival, swimTableColumns.itpMarginTemplate)
            }
        ];
    }
    private isHiddenColumn(isArrival: boolean, column: string){
        let defaultArrivalHiddenCols = [swimTableColumns.originAirport,swimTableColumns.eta];
        let defaultDeparturesHiddenCols = [swimTableColumns.destinationAirport,swimTableColumns.atd];

        let lobbyArrivalCols = [swimTableColumns.status,swimTableColumns.tailNumber,swimTableColumns.makeModel,swimTableColumns.eta,swimTableColumns.originAirport,swimTableColumns.originCity,swimTableColumns.isAircraftOnGround];

        let lobbyDeparturesCols = [swimTableColumns.status,swimTableColumns.tailNumber,swimTableColumns.makeModel,swimTableColumns.atd,swimTableColumns.destinationAirport,swimTableColumns.destinationCity,swimTableColumns.isAircraftOnGround];

        if(this.isLobbyView){
            if(isArrival){
                return !lobbyArrivalCols.includes(column);
            }else{
                return !lobbyDeparturesCols.includes(column);
            }
        }

        if(isArrival){
            return defaultArrivalHiddenCols.includes(column);
        }else{
            return defaultDeparturesHiddenCols.includes(column);
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
