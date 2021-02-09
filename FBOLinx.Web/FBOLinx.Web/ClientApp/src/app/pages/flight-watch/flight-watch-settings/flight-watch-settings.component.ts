import { ChangeDetectionStrategy } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { keys } from 'lodash';
import { FlightWatch } from '../../../models/flight-watch';
import { AircraftIcons } from '../flight-watch-map/aircraft-icons';

@Component({
    selector: 'app-flight-watch-settings',
    templateUrl: './flight-watch-settings.component.html',
    styleUrls: [ './flight-watch-settings.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FlightWatchSettingsComponent {
    @Input() tableData: Observable<FlightWatch[]>;
    @Input() filteredTypes: string[];
    @Output() typesFilterChanged = new EventEmitter<string[]>();
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

    get aircraftTypes() {
        return keys(AircraftIcons).map(type => ({
            aircraftType: type,
            color: AircraftIcons[type].fillColor,
        }));
    }

    applyFilter(event: Event) {
        this.filterChanged.emit((event.target as HTMLInputElement).value);
    }

    toggleType(type: string) {
        if (this.filteredTypes.includes(type)) {
            this.typesFilterChanged.emit(this.filteredTypes.filter(ft => ft !== type));
        } else {
            this.typesFilterChanged.emit([...this.filteredTypes, type]);
        }
    }
}
