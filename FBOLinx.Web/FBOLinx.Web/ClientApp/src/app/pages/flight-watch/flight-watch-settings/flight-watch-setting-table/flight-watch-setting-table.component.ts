import { animate, state, style, transition, trigger } from '@angular/animations';
import { OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatSort, MatSortable, MatSortHeader, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { SharedService } from 'src/app/layouts/shared-service';
import { stautsTextColor, Swim, swimTableColumns, swimTableColumnsDisplayText, tailNumberTextColor } from 'src/app/models/swim';
import {
    ColumnType,
} from 'src/app/shared/components/table-settings/table-settings.component';
import { BooleanToTextPipe } from 'src/app/shared/pipes/boolean/booleanToText.pipe';
import { GetTimePipe } from 'src/app/shared/pipes/dateTime/getTime.pipe';
import { ToReadableTimePipe } from 'src/app/shared/pipes/time/ToReadableTime.pipe';
import { FlightWatchHelper } from "../../FlightWatchHelper.service";
import { FlightLegStatus } from "../../../../enums/flight-watch.enum";

@Component({
    selector: 'app-flight-watch-setting-table',
    templateUrl: './flight-watch-setting-table.component.html',
    styleUrls: ['./flight-watch-setting-table.component.scss'],
    animations: [
        trigger('detailExpand', [
            state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
            state('expanded', style({ height: '*' })),
            transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
            transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
          ])
    ]
})
export class FlightWatchSettingTableComponent implements OnInit {
    @Input() data: Swim[];
    @Input() isArrival: boolean;
    @Input() columns: ColumnType[];

    @Output() openAircraftPopup = new EventEmitter<string>();
    @Output() saveSettings = new EventEmitter();

    @ViewChild(MatSort) sort: MatSort;

    dataSource: MatTableDataSource<Swim>;

    allColumnsToDisplay: string[];
    dataColumnsToDisplay: string[];
    columnsToDisplayDic = new Object();

    columnsToDisplayWithExpand : any[];
    expandedElement: any | null;

    fbo: string;
    icao: string

    hasChangeDefaultSort = false;

    constructor(private getTime : GetTimePipe,
                private toReadableTime: ToReadableTimePipe,
                private sharedService: SharedService,
                private booleanToText: BooleanToTextPipe,
                private flightWatchHelper: FlightWatchHelper) { }

    ngOnInit() {
        this.fbo = localStorage.getItem('fbo');
        this.icao = this.sharedService.currentUser.icao;
    }
    ngAfterViewInit() {
        if(this.isArrival){
            this.dataSource = new MatTableDataSource(this.data?.sort((a, b) => { return this.compare(a.etaLocal, b.etaLocal, false); }));
        }else{
            this.dataSource =  new MatTableDataSource(this.setManualSortOnDepartures(this.data));
        }
        this.dataSource.sort = this.sort;


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
            this.saveSettings.emit();
        });
    }
    ngOnChanges(changes: SimpleChanges) {
        if(changes.columns){
            this.allColumnsToDisplay = this.getVisibleColumns();
            this.dataColumnsToDisplay = this.getVisibleDataColumns();
            this.setVisibleColumnsName();
        }
        if (changes.data && this.dataSource){
            this.dataSource.data = changes.data.currentValue;
            if(this.hasChangeDefaultSort){
                this.dataSource.data = changes.data.currentValue;
                this.refreshSort();
                return;
            }

            if(this.isArrival){
                this.dataSource.sort.sort(<MatSortable>({id: swimTableColumns.etaAtd, start: 'desc'}));
            }else{
                this.dataSource.data = this.setManualSortOnDepartures(changes.data.currentValue);
            }
        }
    }
    setManualSortOnDepartures(data: Swim[]){
        var taxiing = data?.filter((row) => { return FlightLegStatus.TaxiingDestination == row.status || FlightLegStatus.TaxiingOrigin == row.status; }) || [];
        taxiing = taxiing.sort((a, b) => { return this.compare(a.atdLocal, b.atdLocal, false); });

        var departing = data?.filter((row) => { return FlightLegStatus.Departing == row.status }) || [];
        departing = departing.sort((a, b) => { return this.compare(a.atdLocal, b.atdLocal, false); });

        var enRoute = data?.filter((row) => { return FlightLegStatus.EnRoute == row.status; }) || [];
        enRoute = enRoute.sort((a, b) => { return this.compare(a.atdLocal, b.atdLocal, false); });

        return (taxiing.concat(departing)).concat(enRoute);
    }
    getVisibleDataColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => column.id) ||
            [];
    }
    getVisibleColumns() {
        var result = ['expand-icon'];
        result.push(...
            this.getVisibleDataColumns()
        );

        return result;
    }
    setVisibleColumnsName() {
        this.columns.filter((column) => !column.hidden).forEach((col) => {
            this.columnsToDisplayDic[col.id] = col.name;
        });
    }
    getOriginDestinationString(element: Swim){
        return this.isArrival
                ? element.origin
                : element.arrivalICAO
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
    getDateObject(dateString: string){
        if (dateString === null || dateString.trim() === "") return null;
        return new Date(dateString);
    }
    getColumnData(row: Swim, column:string){
        if(column == "expandedDetail") return;
        if(column == swimTableColumns.originDestination) return this.getOriginDestinationString(row);
        if (column == swimTableColumns.ete) { return !row.ete ? '' : this.toReadableTime.transform(row.ete);}
        if(column == swimTableColumns.isAircraftOnGround) return this.booleanToText.transform(row.isAircraftOnGround);
        if(column == swimTableColumns.status) {
            if(row.status == FlightLegStatus.EnRoute)
                return "En Route";
            else if(row.status == FlightLegStatus.TaxiingOrigin || row.status == FlightLegStatus.TaxiingDestination)
                return "Taxiing";
            return  FlightLegStatus[row.status];
        }
        if(column == swimTableColumns.etaAtd){
            return this.isArrival
            ? this.getTime.transform(this.getDateObject(row.etaLocal))
            : this.getTime.transform(this.getDateObject(row.atdLocal));
        }

        let col = this.columns.find( c => {
            return c.id == column
        });
        return row[col.id];
    }
    getColumnDisplayString(column:string){
        if(column == swimTableColumns.originDestination || column == swimTableColumns.etaAtd){
            return this.isArrival
            ? swimTableColumnsDisplayText[column].arrivals
            : swimTableColumnsDisplayText[column].departures;
        }

        return swimTableColumnsDisplayText[column];
    }
    getOriginCityLabel(){
        return this.isArrival
        ? "Origin City"
        : "Destination City";
    }
    getMakeModelDisplayString(element: Swim){
        var makemodelstr = this.flightWatchHelper.getSlashSeparationDisplayString(element.make,element.model);
        return this.flightWatchHelper.getEmptyorDefaultStringText(makemodelstr);
    }
    getTextColor(row: Swim, column:string){
        if(column == swimTableColumns.tailNumber)
            return this.getTailNumberTextColor(row);

        if(column == swimTableColumns.status){
            return stautsTextColor[FlightLegStatus[row.status]];
        }

        return "black";
    }
    getTailNumberTextColor(row: Swim){
        if(row.isInNetwork)return tailNumberTextColor.inNetwork;
        if(row.isOutOfNetwork) return tailNumberTextColor.outOfNetwork;
        if(row.isActiveFuelRelease) return tailNumberTextColor.activeFuelRelease;

        return tailNumberTextColor.fuelerLinx;
    }
    getPastArrivalsValue(row: Swim){
        return this.isArrival
            ? row.arrivals
            : row.departures;
}
    sortData(sort: Sort) {
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
              case swimTableColumns.etaAtd:
              return (this.isArrival) ? this.compare(a.etaLocal, b.etaLocal, isAsc) : this.compare(a.atdLocal, b.atdLocal, isAsc);
              case swimTableColumns.originDestination:
                return (this.isArrival)? this.compare(a.origin, b.origin, isAsc): this.compare(a.arrivalICAO, b.arrivalICAO, isAsc);
              case swimTableColumns.isAircraftOnGround:
              return this.compare(a.isAircraftOnGround.toString(), b.isAircraftOnGround.toString(), isAsc);
              case swimTableColumns.itpMarginTemplate:
                return this.compare(a.itpMarginTemplate, b.itpMarginTemplate, isAsc);
            default:
              return 0;
          }
        });
    }
    compare(a: number | string , b: number | string, isAsc: boolean) {
        var result =  (a < b ? -1 : 1) * (isAsc ? 1 : -1);
        return result;
    }
}
