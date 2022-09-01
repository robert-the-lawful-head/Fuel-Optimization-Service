import { animate, state, style, transition, trigger } from '@angular/animations';
import { OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatSort, MatSortHeader, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SharedService } from 'src/app/layouts/shared-service';
import { FlightLegStatusEnum, Swim, swimTableColumns } from 'src/app/models/swim';
import {
    ColumnType,
} from 'src/app/shared/components/table-settings/table-settings.component';
import { BooleanToTextPipe } from 'src/app/shared/pipes/boolean/booleanToText.pipe';
import { GetTimePipe } from 'src/app/shared/pipes/dateTime/getTime.pipe';
import { ToReadableTimePipe } from 'src/app/shared/pipes/time/ToReadableTime.pipe';

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

    columnsToDisplay : string[];

    columnsToDisplayWithExpand : any[];
    expandedElement: any | null;

    fbo: string;
    icao: string

    constructor(private getTime : GetTimePipe,
                private toReadableTime: ToReadableTimePipe,
                private sharedService: SharedService,
                private booleanToText: BooleanToTextPipe) { }

    ngOnInit() {
        this.fbo = localStorage.getItem('fbo');
        this.icao = this.sharedService.currentUser.icao;
    }
    ngAfterViewInit() {
        this.dataSource = new MatTableDataSource(this.data);
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
            this.columnsToDisplay = this.getVisibleColumns();
        }
        if (changes.data && this.dataSource){
            this.dataSource.data = changes.data.currentValue;
        }
    }
    getVisibleColumns() {
        return (
            this.columns
                .filter((column) => !column.hidden)
                .map((column) => column.name) || []
        );
    }
    getSlashSeparationDisplayString(e1: any,e2: any){
        let str = (e1)?e1:"";
        str += (e1 && e2)?"/":"";
        str += (e2)?e2:"";
        return str;
    }
    getOriginDestinationString(element: Swim){
        console.log("🚀 ~ file: flight-watch-setting-table.component.ts ~ line 96 ~ FlightWatchSettingTableComponent ~ getOriginDestinationString ~ element", element)
        return this.isArrival
                ? element.origin
                : element.city
    }
    refreshSort() {
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
            if(row.status == FlightLegStatusEnum.EnRoute)
                return "En Route";
            else if(row.status == FlightLegStatusEnum.TaxiingOrigin || row.status == FlightLegStatusEnum.TaxiingDestination)
                return "Taxiing";
            return  FlightLegStatusEnum[row.status];
        }
        if(column == swimTableColumns.etaAtd){
            return this.isArrival
            ? this.getTime.transform(this.getDateObject(row.etaLocal))
            : this.getTime.transform(this.getDateObject(row.atdLocal));
        }
        let col = this.columns.find( c => c.name == column)
        return row[col.id];
    }
    getColumnDisplayString(column:string){
        if(column == swimTableColumns.etaAtd){
            return this.isArrival
            ? "ETA"
            : "ATD";
        }

        if(column == swimTableColumns.originDestination){
            return this.isArrival
            ? "Origin"
            : "Destination";
        }
        return column;
    }
    getOriginCityLabel(){
        return this.isArrival
        ? "Origin City"
        : "Destination City";
    }
    getMakeModelDisplayString(element: Swim){

        var makemodelstr = this.getSlashSeparationDisplayString(element.make,element.model) == ""  ? "" : "" ;

        return makemodelstr == ""  ? "Unknown" : makemodelstr ;
    }
    getTextColor(row: Swim, column:string){
        if(column != swimTableColumns.tailNumber) return "";
        return "";
    }
    getPastArrivalsValue(row: Swim){
        return this.isArrival
            ? row.arrivals
            : row.departures;
}
    sortData(sort: Sort) {
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
              return (this.isArrival) ? this.compare(a.etaZulu, b.etaZulu, isAsc) : this.compare(a.atdZulu, b.atdZulu, isAsc);
              case swimTableColumns.originDestination:
                return (this.isArrival)? this.compare(a.arrivals, b.arrivals, isAsc): this.compare(a.departures, b.departures, isAsc);
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
