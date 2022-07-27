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
    @Input() columns: ColumnType[];  

    @Output() openAircraftPopup = new EventEmitter<string>();
    @Output() saveSettings = new EventEmitter();

    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor() { }

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
            this.saveSettings.emit();        });
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
    geMakeModelDisplayString(element: any){
        console.log("🚀 ~ file: flight-watch-setting-table.component.ts ~ line 43 ~ FlightWatchSettingTableComponent ~ geMakeModelDisplayString ~ element", element)
        let str = element.Make;
        str += (element.Make && element.Model) ? '/':'';
        str += element.Model;
        return str;
    }
    getOriginDestinationString(element: any){
        return this.isArrival
                ? element.departureCity
                : element.arrivalCity
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
}
