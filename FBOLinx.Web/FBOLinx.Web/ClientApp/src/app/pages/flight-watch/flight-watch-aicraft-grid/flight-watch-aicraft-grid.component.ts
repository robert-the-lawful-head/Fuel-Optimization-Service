import { ChangeDetectionStrategy, SimpleChanges, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SharedService } from 'src/app/layouts/shared-service';
import { SwimFilter } from 'src/app/models/filter';
import { swimTableColumns, swimTableColumnsDisplayText } from 'src/app/models/swim';
import { GridBase } from 'src/app/services/tables/GridBase';
import { ColumnType, TableSettingsComponent } from 'src/app/shared/components/table-settings/table-settings.component';
import { FlightWatchModelResponse } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from '../flight-watch-map/aircraft-images';
import { FlightWatchAircraftDataTableComponent } from './flight-watch-aircraft-data-table/flight-watch-aircraft-data-table.component';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-flight-watch-aicraft-grid',
    styleUrls: ['./flight-watch-aicraft-grid.component.scss'],
    templateUrl: './flight-watch-aicraft-grid.component.html',
})
export class FlightWatchAicraftGridComponent extends GridBase {
    @Input() data: FlightWatchModelResponse[];
    @Input() icao: string;
    @Input() icaoList: string[];
    @Input() filteredTypes: string[];
    @Input() showFilters: boolean =  true;
    @Input() isLobbyView: boolean =  false;
    @Input() selectedAircraft: FlightWatchModelResponse = null;
    @Input() closedAircraft: FlightWatchModelResponse = null;

    @Output() typesFilterChanged = new EventEmitter<string[]>();
    @Output() filterChanged = new EventEmitter<SwimFilter>();
    @Output() icaoChanged = new EventEmitter<string>();
    @Output() updateDrawerButtonPosition = new EventEmitter<any>();
    @Output() openAircraftPopup = new EventEmitter<string>();
    @Output() closeAircraftPopup = new EventEmitter<string>();

    @ViewChild('arrivalsTable') public arrivalsTable: FlightWatchAircraftDataTableComponent;
    @ViewChild('departuresTable') public departuresTable: FlightWatchAircraftDataTableComponent;

    arrivals: FlightWatchModelResponse[] =[];
    departures: FlightWatchModelResponse[] =[];

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

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.data && changes.data.currentValue) {
            this.setData(changes.data.currentValue);
        }
    }
    openPopUpAndCloseExpandedRows(tailNumber: string): void {
        this.openAircraftPopup.emit(tailNumber);
        this.expandRow(tailNumber);
    }
    expandRow(tailNumber: string): void {
        this.arrivalsTable.expandRow(tailNumber);
        this.departuresTable.expandRow(tailNumber);
    }
    collapseRow(tailNumber: string): void {
        let existInArrivals = this.arrivalsTable.hasRowInTable(tailNumber);
        let existOnDepartures = this.departuresTable.hasRowInTable(tailNumber);
        if(existInArrivals){
            this.arrivalsTable.collapseRow(tailNumber);
        }
        else if(existOnDepartures){
            this.departuresTable.collapseRow(tailNumber);
        }
    }
    collapseAllRows(): void {
        this.arrivalsTable.collapseAllRows();
        this.departuresTable.collapseAllRows();
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

    setData(data: FlightWatchModelResponse[]): void {
        this.arrivals = data?.filter((row: FlightWatchModelResponse) => {
            return row.arrivalICAO == row.focusedAirportICAO
        });
        this.departures = data?.filter((row: FlightWatchModelResponse) => {
            return (
                row.departureICAO == row.focusedAirportICAO &&
                row.status != null
            );
        });
    }

    arrivalsDeparturesCommonCols: string[]= [swimTableColumns.status,swimTableColumns.tailNumber,swimTableColumns.flightDepartment,swimTableColumns.icaoAircraftCode,swimTableColumns.ete,swimTableColumns.isAircraftOnGround,swimTableColumns.itpMarginTemplate];

    defaultArrivalCols = [swimTableColumns.etaLocal,swimTableColumns.originAirport].concat(this.arrivalsDeparturesCommonCols);

    defaultDeparturesCols = [swimTableColumns.atdLocal,swimTableColumns.destinationAirport].concat(this.arrivalsDeparturesCommonCols);

    arrivalsDeparturesLobbyCommonCols: string[] = [swimTableColumns.status,swimTableColumns.tailNumber,swimTableColumns.makeModel,swimTableColumns.isAircraftOnGround,swimTableColumns.flightDepartment,swimTableColumns.icaoAircraftCode];

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
