import { ChangeDetectionStrategy, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SharedService } from 'src/app/layouts/shared-service';
import { SwimFilter } from 'src/app/models/filter';
import { swimTableColumns, swimTableColumnsDisplayText } from 'src/app/models/swim';
import { GridBase } from 'src/app/services/tables/GridBase';
import { ColumnType, TableSettingsComponent } from 'src/app/shared/components/table-settings/table-settings.component';
import { FlightWatchModelResponse } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from '../flight-watch-map/aircraft-images';
import { FlightWatchSettingTableComponent } from './flight-watch-setting-table/flight-watch-setting-table.component';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-settings',
    styleUrls: ['./flight-watch-settings.component.scss'],
    templateUrl: './flight-watch-settings.component.html',
})
export class FlightWatchSettingsComponent extends GridBase {
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

    customers: any[] = [];

    constructor(private tableSettingsDialog: MatDialog,
        private sharedService: SharedService,
        private customerInfoByGroupService: CustomerinfobygroupService
    ) {
        super();
    }
    ngOnInit(): void {
        this.initColumns();
        this.getCustomersList(this.sharedService.currentUser.groupId,this.sharedService.currentUser.fboId);
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
            this.columns = [...result]
            this.updateDrawerButtonPosition.emit();
            this.saveSettings();
            this.departuresTable.updateColumns(this.getFilteredDefaultColumns(false, this.isLobbyView,this.columns));
            this.arrivalsTable.updateColumns(this.getFilteredDefaultColumns(true, this.isLobbyView,this.columns));
            this.arrivalsTable.refreshSort();
            this.departuresTable.refreshSort();
        });
    }
    initColumns() {
        this.tableLocalStorageKey = (this.isLobbyView) ? `lobby-settings-${this.sharedService.currentUser.fboId}` : `flight-watch-settings-${this.sharedService.currentUser.fboId}`;

        this.columns = this.getClientSavedColumns(this.tableLocalStorageKey, this.getSettingsColumnDefinition());

        this.arrivalsColumns =  this.getFilteredDefaultColumns(true, this.isLobbyView,this.columns);
        this.departuresColumns =  this.getFilteredDefaultColumns(false, this.isLobbyView,this.columns);
    }
    private getSettingsColumnDefinition(): ColumnType[]{
        let cols = [];
        if(this.isLobbyView){
            cols = [...new Set(this.lobbyArrivalCols.concat(this.lobbyDeparturesCols))]
        }
        else{
            cols = [...new Set(this.defaultArrivalCols.concat(this.defaultDeparturesCols))]
        }
        return this.tableColumns?.filter((column) => { return cols.includes(column.id) }) || [];

    }
    private getFilteredDefaultColumns(isArrival: boolean, isLobbyView, columns: ColumnType[]) : ColumnType[]{
        if(isLobbyView){
            if(isArrival){
                return columns?.filter((column) => { return this.lobbyArrivalCols.includes(column.id) }) || [];
            }else{
                return columns?.filter((column) => { return this.lobbyDeparturesCols.includes(column.id) }) || [];
            }
        }

        if(isArrival){
            return columns?.filter((column) => { return this.defaultArrivalCols.includes(column.id) }) || [];
        }else{
            return columns?.filter((column) => { return this.defaultDeparturesCols.includes(column.id)}) || [];
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

    getCustomersList(groupId,fboId) {
        this.customerInfoByGroupService
            .getCustomersListByGroupAndFbo(
                groupId,
                fboId
            )
            .subscribe((customers: any[]) => {
                this.customers = customers;
            });
    }

    arrivalsDeparturesCommonCols: string[]= [swimTableColumns.status,swimTableColumns.tailNumber,swimTableColumns.flightDepartment,swimTableColumns.icaoAircraftCode,swimTableColumns.ete,swimTableColumns.isAircraftOnGround,swimTableColumns.itpMarginTemplate];

    defaultArrivalCols = [swimTableColumns.etaLocal,swimTableColumns.originAirport].concat(this.arrivalsDeparturesCommonCols);

    defaultDeparturesCols = [swimTableColumns.atdLocal,swimTableColumns.destinationAirport].concat(this.arrivalsDeparturesCommonCols);

    arrivalsDeparturesLobbyCommonCols: string[] = [swimTableColumns.status,swimTableColumns.tailNumber,swimTableColumns.makeModel,swimTableColumns.isAircraftOnGround];

    lobbyArrivalCols= [swimTableColumns.etaLocal,swimTableColumns.originAirport,swimTableColumns.originCity].concat(this.arrivalsDeparturesLobbyCommonCols);

    lobbyDeparturesCols = [swimTableColumns.atdLocal,swimTableColumns.destinationAirport,swimTableColumns.destinationCity,swimTableColumns.isAircraftOnGround].concat(this.arrivalsDeparturesLobbyCommonCols);

    tableColumns :ColumnType[]= [
        {
            id: swimTableColumns.status,
            name: swimTableColumnsDisplayText[swimTableColumns.status],
        },
        {
            id: swimTableColumns.tailNumber,
            name: swimTableColumnsDisplayText[swimTableColumns.tailNumber],
        },
        {
            id: swimTableColumns.flightDepartment,
            name: swimTableColumnsDisplayText[swimTableColumns.flightDepartment],
            sort: 'desc',
        },
        {
            id: swimTableColumns.icaoAircraftCode,
            name: swimTableColumnsDisplayText[swimTableColumns.icaoAircraftCode],
        },
        {
            id: swimTableColumns.ete,
            name: swimTableColumnsDisplayText[swimTableColumns.ete],
        },
        {
            id: swimTableColumns.atdLocal,
            name: swimTableColumnsDisplayText[swimTableColumns.atdLocal],
        },
        {
            id: swimTableColumns.etaLocal,
            name: swimTableColumnsDisplayText[swimTableColumns.etaLocal],
        },
        {
            id: swimTableColumns.originAirport,
            name: swimTableColumnsDisplayText[swimTableColumns.originAirport],
        },
        {
            id: swimTableColumns.originCity,
            name: swimTableColumnsDisplayText[swimTableColumns.originCity],
        },
        {
            id: swimTableColumns.destinationAirport,
            name: swimTableColumnsDisplayText[swimTableColumns.destinationAirport],
        },
        {
            id: swimTableColumns.destinationCity,
            name: swimTableColumnsDisplayText[swimTableColumns.destinationCity],
        },
        {
            id: swimTableColumns.makeModel,
            name: swimTableColumnsDisplayText[swimTableColumns.makeModel],
        },
        {
            id: swimTableColumns.isAircraftOnGround,
            name: swimTableColumnsDisplayText[swimTableColumns.isAircraftOnGround],
        },
        {
            id: swimTableColumns.itpMarginTemplate,
            name: swimTableColumnsDisplayText[swimTableColumns.itpMarginTemplate],
        }
    ];
}
