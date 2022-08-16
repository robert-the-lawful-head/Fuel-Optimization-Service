import { animate, state, style, transition, trigger } from '@angular/animations';
import { OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { SharedService } from 'src/app/layouts/shared-service';
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

    fbo: string;
    icao: string

    constructor(private getTime : GetTimePipe,
                private toReadableDateTime: ToReadableDateTimePipe,
                private toReadableTime: ToReadableTimePipe,
                private sharedService: SharedService) { }

    ngOnInit() {
        this.fbo = localStorage.getItem('fbo');
        this.icao = this.sharedService.currentUser.icao;
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
    ngOnChanges(changes: SimpleChanges) {
        if(changes.columns){
            this.columnsToDisplay = this.getVisibleColumns();
            // this.columnsToDisplay.push('expandedDetail')
            this.columnsToDisplayWithExpand = [...this.getVisibleColumns(), 'expand'];
        }
    }
    openPopup(row: any) {
        this.openAircraftPopup.emit(row.tailNumber);
    }
    getVisibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.name) || []
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
        if(column == "Status") {
            if(row.status == FlightLegStatusEnum.EnRoute)
                return "In Route";
            return  FlightLegStatusEnum[row.status];
        }
        let col = this.columns.find( c => c.name == column)
        return row[col.id];
    }
    getPastArrivalsValue(row: Swim){

            return this.isArrival
                ? row.arrivals
                : row.departures;

    }
    getColumnHeader(column: string){
            if( column != "origin-destination") return column;
            return this.isArrival
                ? 'Origin'
                : 'Destination';
    }
}

