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

    constructor() { }

    ngOnInit() { }

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
}
