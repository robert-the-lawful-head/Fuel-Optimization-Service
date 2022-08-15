import { animate, state, style, transition, trigger } from '@angular/animations';
import { OnInit, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { FlightLegStatusEnum, Swim } from 'src/app/models/swim';
import {
    ColumnType,
} from 'src/app/shared/components/table-settings/table-settings.component';
import { GetTimePipe } from 'src/app/shared/pipes/dateTime/getTime.pipe';
import { ToReadableDateTimePipe } from 'src/app/shared/pipes/dateTime/ToReadableDateTime.pipe';
import { ToReadableTimePipe } from 'src/app/shared/pipes/time/ToReadableTime.pipe';

@Component({
    selector: 'app-flight-watch-setting-table',
    templateUrl: './flight-watch-setting-table.component.html',
    styleUrls: ['./flight-watch-setting-table.component.scss'],
    animations: [
        trigger('detailExpand', [
            state('collapsed', style({height: '0px', minHeight: '0'})),
            state('expanded', style({height: '*'})),
            transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
        ]),
    ],
})
export class FlightWatchSettingTableComponent implements OnInit {
    @Input() data: Swim[];
    @Input() isArrival: boolean;
    @Input() columns: ColumnType[];

    @Output() openAircraftPopup = new EventEmitter<string>();
    @Output() saveSettings = new EventEmitter();

    @ViewChild(MatSort, { static: true }) sort: MatSort;

    columnsToDisplay : string[];

    columnsToDisplayWithExpand : any[];
    expandedElement: Swim | null;

    ELEMENT_DATA: Swim[];

    constructor(private getTime : GetTimePipe,
                private toReadableDateTime: ToReadableDateTimePipe,
                private toReadableTime: ToReadableTimePipe) { }

    ngOnInit() {
        this.columnsToDisplay = this.columns.map((element) => {
            return element.name;
        });

        this.columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand'];
    }
    ngAfterViewInit() {
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
    openPopup(row: any) {
        this.openAircraftPopup.emit(row.tailNumber);
    }
    get visibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.id) || []
        );
    }
    getMakeModelDisplayString(element: any){
        let str = (element.Make)?element.Make:"";
        str += (element.Make && element.Make)?"/":"";
        str += (element.Model)?element.Model:"";
        return str;
    }
    getOriginDestinationString(element: Swim){
        return this.isArrival
                ? element.origin
                : element.city
    }
    refreshSort() {
        console.log("🚀 ~ file: flight-watch-setting-table.component.ts ~ line 102 ~ FlightWatchSettingTableComponent ~ refreshSort ~  this.sort",  this.sort)
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
    getSwimDataTypeString(){
        return (this.isArrival)? 'Arrivals': 'Departures';
    }
    getDateObject(dateString: string){
        return new Date(dateString);
    }
    getColumnData(row: Swim, column:string){
        if(column == "Make/Model") return this.getMakeModelDisplayString(row);
        if(column == "Origin/Destination") return this.getOriginDestinationString(row);
        if(column == "ETA/ATD") return this.getTime.transform(this.getDateObject(row.etaLocal));
        if(column == "ETE") return this.toReadableTime.transform(row.ete);
        if(column == "ETA") return this.toReadableDateTime.transform(this.getDateObject(row.etaLocal));
        if(column == "Status") return  FlightLegStatusEnum[row.status];

        let col = this.columns.find( c => c.name == column)
        return row[col.id];
    }
    getColumnHeader(column: string){
            if( column != "origin-destination") return column;
            return this.isArrival
                ? 'Origin'
                : 'Destination';
    }
}

