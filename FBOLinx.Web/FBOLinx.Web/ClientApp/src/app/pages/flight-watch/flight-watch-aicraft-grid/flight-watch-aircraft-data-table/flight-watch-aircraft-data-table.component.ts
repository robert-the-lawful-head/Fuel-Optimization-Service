import { state, style, trigger } from '@angular/animations';
import { ChangeDetectorRef, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatSort, MatSortable, MatSortHeader, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { SharedService } from 'src/app/layouts/shared-service';
import { stautsIcons, stautsTextColor, Swim, swimTableColumns, swimTableColumnsDisplayText, tailNumberTextColor } from 'src/app/models/swim';
import {
    ColumnType,
} from 'src/app/shared/components/table-settings/table-settings.component';
import { BooleanToTextPipe } from 'src/app/shared/pipes/boolean/booleanToText.pipe';
import { GetTimePipe } from 'src/app/shared/pipes/dateTime/getTime.pipe';
import { ToReadableTimePipe } from 'src/app/shared/pipes/time/ToReadableTime.pipe';
import { FlightWatchHelper } from "../../FlightWatchHelper.service";
import { FlightLegStatus } from "../../../../enums/flight-watch.enum";
import { CallbackComponent } from 'src/app/shared/components/favorite-icon/favorite-icon.component';
import { defaultStringsEnum } from 'src/app/enums/strings.enums';
import { FlightWatchMapSharedService } from '../../services/flight-watch-map-shared.service';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';

@Component({
    selector: 'app-flight-watch-aircraft-data-table',
    templateUrl: './flight-watch-aircraft-data-table.component.html',
    styleUrls: ['./flight-watch-aircraft-data-table.component.scss'],
    animations: [
        trigger('detailExpand', [
            state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
            state('expanded', style({ height: '*' }))
          ])
    ],
    providers: [GetTimePipe,ToReadableTimePipe,BooleanToTextPipe]
})
export class FlightWatchAircraftDataTableComponent implements OnInit {
    @Input() data: Swim[];
    @Input() isArrival: boolean;
    @Input() columns: ColumnType[];
    @Input() isLobbyView: boolean =  false;
    @Input() customers: any[] =  [];

    @Output() openAircraftPopup = new EventEmitter<string>();
    @Output() closeAircraftPopup = new EventEmitter<string>();

    @Output() saveSettings = new EventEmitter();

    @ViewChild(MatSort) sort: MatSort;

    dataSource: MatTableDataSource<Swim> = new MatTableDataSource<Swim>();
    expandedDetailData: FlightWatchModelResponse | Swim | null;

    allColumnsToDisplay: string[];
    dataColumnsToDisplay: string[];
    columnsToDisplayDic = new Object();

    columnsToDisplayWithExpand : any[];
    expandedElement: any | null;

    fboId: number;
    fbo: string;
    icao: string

    hasChangeDefaultSort = false;

    constructor(private getTime : GetTimePipe,
                private toReadableTime: ToReadableTimePipe,
                private sharedService: SharedService,
                private booleanToText: BooleanToTextPipe,
                private flightWatchHelper: FlightWatchHelper,
                private flightWatchMapSharedService: FlightWatchMapSharedService,
                private cdr: ChangeDetectorRef) {
                    this.flightWatchMapSharedService.aicraftDetails$.subscribe( (data: FlightWatchModelResponse) => {
                    this.expandedDetailData = data;
                    });
                }

    ngOnInit() {
        this.fbo = localStorage.getItem('fbo');
        this.fboId = this.sharedService.currentUser.fboId;
        this.icao = this.sharedService.currentUser.icao;
    }
    ngAfterViewInit() {
        if(this.isArrival){
            this.dataSource = new MatTableDataSource(this.data?.sort((a, b) => { return this.compare(b.etaLocal, a.etaLocal, false); }));
        }else{
            this.dataSource =  new MatTableDataSource(this.setManualSortOnDepartures(this.data));
        }
        this.dataSource.sort = this.sort;


        this.sort?.sortChange.subscribe(() => {
            this.columns = this.columns.map((column) =>
                column.id === this.sort.active
                    ? { ...column, sort: this.sort.direction }
                    : {
                          hidden: column.hidden,
                          id: column.id,
                          name: column.name,
                      }
            );
            this.saveSettings.emit();
        });
    }
    ngOnChanges(changes: SimpleChanges) {
        if(changes.columns){
            this.allColumnsToDisplay = this.getVisibleColumns();
            this.dataColumnsToDisplay = this.getVisibleDataColumns();
        }
        if (changes.data){
            if(this.hasChangeDefaultSort){
                this.dataSource.data =  changes.data.currentValue;
                return;
            }

            if(this.isArrival){
                this.dataSource.sort.sort(<MatSortable>({id: swimTableColumns.etaLocal, start: 'asc'}));
            }else{
                this.dataSource.data = this.setManualSortOnDepartures(changes.data.currentValue);
            }
        }
        if(changes.selectedAircraft?.currentValue?.tailNumber){
            this.expandedElement = changes.selectedAircraft.currentValue.tailNumber;
        }
    }

    updateColumns(columns: ColumnType[]): void{
        this.columns = columns;
        this.allColumnsToDisplay = this.getVisibleColumns();
        this.dataColumnsToDisplay = this.getVisibleDataColumns();
    }
    setManualSortOnDepartures(data: Swim[]){
        var taxiing = data?.filter((row) => { return FlightLegStatus.TaxiingDestination == row.status || FlightLegStatus.TaxiingOrigin == row.status; }) || [];
        taxiing = taxiing.sort((a, b) => { return this.compare(b.atdLocal, a.atdLocal, false); });

        var departing = data?.filter((row) => { return FlightLegStatus.Departing == row.status }) || [];
        departing = departing.sort((a, b) => { return this.compare(b.atdLocal, a.atdLocal, false); });

        var enRoute = data?.filter((row) => { return FlightLegStatus.EnRoute == row.status; }) || [];
        enRoute = enRoute.sort((a, b) => { return this.compare(b.atdLocal, a.atdLocal, false); });

        var result = (taxiing.concat(departing)).concat(enRoute);
        return result;
    }
    getVisibleDataColumns() {
        return this.columns
            .filter((column) => {
                if(column.hidden) return false;
                return true;
            })
            .map((column) => column.id) || [];
    }
    getVisibleColumns() {
        var result = ['expand-icon'];
        result.push(...
            this.getVisibleDataColumns()
        );

        return result;
    }
    refreshSort() {
        this.hasChangeDefaultSort= true;
        let sortedColumn = this.columns.find(
            (column) => !column.hidden && column.sort
        );
        this.sort.sort({
            disableClear: false,
            id: null,
            start: sortedColumn?.sort || 'asc',
        });
        this.sort.sort({
            disableClear: false,
            id: sortedColumn?.name,
            start: sortedColumn?.sort || 'asc',
        });
        (
            this.sort.sortables.get(sortedColumn?.name) as MatSortHeader
        )?._setAnimationTransitionState({ toState: 'active' });
    }
    getSwimDataTypeString(){
        return (this.isArrival)? 'Arrivals': 'Departures';
    }

    getColumnData(row: Swim, column:string){
        switch(column){
            case "expandedDetail" :
                return;
            case swimTableColumns.makeModel:
                return this.getMakeModelDisplayString(row);
            case swimTableColumns.etaLocal:
                return this.getTime.transform(row.etaLocal,defaultStringsEnum.tbd);
            case swimTableColumns.atdLocal:
                return this.getTime.transform(row.atdLocal,defaultStringsEnum.tbd);
            case swimTableColumns.ete:
                return  this.toReadableTime.transform(row.ete,defaultStringsEnum.tbd);
            case swimTableColumns.isAircraftOnGround:
                return this.booleanToText.transform(row.isAircraftOnGround);
            case swimTableColumns.status:
                row.statusDisplayString = FlightLegStatus[row.status];
                return row.statusDisplayString;
            case swimTableColumns.flightDepartment:
                return row.flightDepartment ?? row.faaRegisteredOwner;
            case swimTableColumns.icaoAircraftCode:
                return this.getMakeModelDisplayString(row);

            default:
                let col = this.columns.find( c => {
                    return c.id == column
                });
                return row[col.id];
        }
    }
    getColumnDisplayString(column:string){
        return swimTableColumnsDisplayText[column];
    }
    getMakeModelDisplayString(element: Swim){
        let makemodelstr = (element.make) ?
            this.flightWatchHelper.getSlashSeparationDisplayString(element.make,element.model) :
            this.flightWatchHelper.getSlashSeparationDisplayString(element.fAAMake,element.fAAModel);

            return this.flightWatchHelper.getEmptyorDefaultStringText(makemodelstr);
    }
    getTextColor(row: Swim, column:string){
        if(column == swimTableColumns.tailNumber)
            return this.getTailNumberTextColor(row);

        if(column == swimTableColumns.status){
            return stautsTextColor[FlightLegStatus[row.status]];
        }
        return stautsTextColor.default;
    }
    hasIcon(column:string): boolean{
        return column == swimTableColumns.status;
    }
    getIcon(row: Swim): string{
        return stautsIcons[FlightLegStatus[row.status]];
    }
    getTailNumberTextColor(row: Swim) {
        if (row.isActiveFuelRelease) return tailNumberTextColor.activeFuelRelease;
        if (row.isFuelerLinxClient) return tailNumberTextColor.fuelerLinx;
        if (row.isInNetwork) return tailNumberTextColor.inNetwork;

        return tailNumberTextColor.outOfNetwork;
    }
    sortData(sort: Sort){
        this.hasChangeDefaultSort = true;
        this.dataSource.data.sort((a, b) => {
          const isAsc = sort.direction === 'asc';
          switch (sort.active) {
            case swimTableColumns.status:
              return this.compare(a.status, b.status, isAsc);
            case swimTableColumns.tailNumber:
              return this.compare(a.tailNumber, b.tailNumber, isAsc);
            case swimTableColumns.flightDepartment:
              return this.compare(a.flightDepartment, b.flightDepartment, isAsc);
            case swimTableColumns.icaoAircraftCode:
              return this.compare(a.icaoAircraftCode, b.icaoAircraftCode, isAsc);
            case swimTableColumns.ete:
              return this.compare(a.ete, b.ete, isAsc);
            case swimTableColumns.etaLocal:
            return this.compare(a.etaLocal, b.etaLocal, isAsc);
            case swimTableColumns.atdLocal:
                return this.compare(a.atdLocal, b.atdLocal, isAsc);
            case swimTableColumns.originAirport:
                return this.compare(a.origin, b.origin, isAsc);
            case swimTableColumns.destinationAirport:
                return this.compare(a.arrivalICAO, b.arrivalICAO, isAsc);
            case swimTableColumns.isAircraftOnGround:
                return this.compare(a.isAircraftOnGround, b.isAircraftOnGround, isAsc);
            case swimTableColumns.itpMarginTemplate:
                return this.compare(a.itpMarginTemplate, b.itpMarginTemplate, isAsc);
            default:
              return 0;
          }
        });
    }
    compare(a: number | string , b: number | string, isAsc: boolean): number{
        var result =  (a < b ? -1 : 1) * (isAsc ? 1 : -1);
        return result;
    }
    getNoDataToDisplayString(){
        return (this.isArrival) ? "No upcoming arrivals": "No upcoming departures";
    }
    isFavoriteButtonVisible(column: any): boolean {
        return column == swimTableColumns.status && !this.isLobbyView;
    }
    setIsFavoriteProperty(aircraft: any): any {
        aircraft.isFavorite = aircraft.favoriteAircraft != null;
        return aircraft;
    }
    get getCallBackComponent(): CallbackComponent{
        return CallbackComponent.aircraft;
    }
    onRowrowClick(element: Swim) {
        this.flightWatchMapSharedService.getAndUpdateAircraftWithHistorical(this.fboId, this.icao, element);
        this.expandedDetailData = element;

        if(this.expandedElement != element.tailNumber)
            this.closeAircraftPopup.emit(this.expandedElement);

        this.expandedElement = this.expandedElement === element.tailNumber ? null : element.tailNumber

        if(this.expandedElement == null)
            this.closeAircraftPopup.emit(element.tailNumber);
        else
            this.openAircraftPopup.emit(this.expandedElement);
    }
    hasRowInTable(tailNumber: string): boolean{
        return this.data.find(x => x.tailNumber == tailNumber) ? true : false;
    }
    expandRow(tailNumber: string):  void {
        if(this.hasRowInTable(tailNumber)){
            this.expandedElement = tailNumber;
            const selectedRow = document.getElementById(tailNumber);
            selectedRow.scrollIntoView({block: 'center', behavior: 'smooth'});
        }
        else{
            this.expandedElement = null;
        }

        this.cdr.detectChanges();
    }
    collapseRow(tailNumber: string): void {
        if(!tailNumber)
            this.expandedElement = null;

        if(this.expandedElement == tailNumber)
            this.expandedElement = null;

        this.cdr.detectChanges();
    }
    collapseAllRows(): void {
        this.expandedElement = null;
        this.cdr.detectChanges();
    }
}

