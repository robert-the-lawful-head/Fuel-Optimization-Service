import { ChangeDetectionStrategy } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { FlightWatch } from '../../../models/flight-watch';

@Component({
    selector: 'app-flight-watch-settings',
    templateUrl: './flight-watch-settings.component.html',
    styleUrls: [ './flight-watch-settings.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FlightWatchSettingsComponent {
    @Input() tableData: Observable<FlightWatch[]>;
    @Output() filterChanged = new EventEmitter<string>();

    displayedColumns: string[] = [
        'aircraftHexCode',
        'atcFlightNumber',
        'aircraftTypeCode',
        'groundSpeedKts',
        'trackingDegree',
        'verticalSpeedKts',
        'gpsAltitude',
        'isAircraftOnGround'
    ];

    constructor() {
    }

    applyFilter(event: Event) {
        this.filterChanged.emit((event.target as HTMLInputElement).value);
    }
}
