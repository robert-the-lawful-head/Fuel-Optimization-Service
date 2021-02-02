import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { FlightWatch } from '../../../models/flight-watch';

@Component({
    selector: 'app-flight-watch-settings',
    templateUrl: './flight-watch-settings.component.html',
    styleUrls: [ './flight-watch-settings.component.scss'],
})
export class FlightWatchSettingsComponent {
    @Input() dataSource: MatTableDataSource<FlightWatch>;
    @Output() filterChanged = new EventEmitter<string>();

    displayedColumns: string[] = ['aircraftHexCode', 'atcFlightNumber', 'aircraftTypeCode'];

    constructor() {
    }

    applyFilter(event: Event) {
        this.filterChanged.emit((event.target as HTMLInputElement).value);
    }
}
