import { ChangeDetectionStrategy } from '@angular/core';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { FlightWatch } from '../../../models/flight-watch';
import { AIRCRAFT_IMAGES } from '../flight-watch-map/aircraft-images';

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
        return AIRCRAFT_IMAGES
            .filter(type => type.label !== 'Other')
            .map(type => ({
                aircraftType: type.id,
                color: type.fillColor,
                label: type.label,
                description: type.description,
            }))
            .concat({
                aircraftType: 'default',
                color: '#5fb4e6',
                label: 'Other',
                description: '',
            });
    }

    applyFilter(event: Event) {
        this.filterChanged.emit((event.target as HTMLInputElement).value);
    }

    toggleType(type: string) {
        if (this.filteredTypes.includes(type)) {
            if (type === 'default') {
                this.typesFilterChanged.emit(this.filteredTypes.filter(ft => !['B0', 'B3', 'default'].includes(ft)));
            } else {
                this.typesFilterChanged.emit(this.filteredTypes.filter(ft => ft !== type));
            }
        } else {
            if (type === 'default') {
                this.typesFilterChanged.emit([...this.filteredTypes, 'B0', 'B3', 'default']);
            } else {
                this.typesFilterChanged.emit([...this.filteredTypes, type]);
            }
        }
    }
}
