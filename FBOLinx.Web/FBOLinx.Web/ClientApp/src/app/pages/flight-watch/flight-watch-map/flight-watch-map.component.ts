import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, ViewChild } from '@angular/core';
import { isEqual, keys } from 'lodash';
import { FlightWatch } from '../../../models/flight-watch';
import { AircraftIcons } from './aircraft-icons';

@Component({
    selector: 'app-flight-watch-map',
    templateUrl: './flight-watch-map.component.html',
    styleUrls: [ './flight-watch-map.component.scss' ],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FlightWatchMapComponent implements OnChanges {
    @ViewChild('map') map: google.maps.Map;
    @Input() center: google.maps.LatLngLiteral;
    @Input() data: {
        [oid: number]: FlightWatch;
    };
    @Input() isStable: boolean;
    @Output() markerClicked = new EventEmitter<FlightWatch>();

    // Map Options
    zoom = 8;
    markerIcons = AircraftIcons;
    keys: string[] = [];

    bounds = new google.maps.LatLngBounds(
        new google.maps.LatLng(85, -180),
        new google.maps.LatLng(-85, 180)
    );

    constructor() {
    }

    ngOnChanges(changes: SimpleChanges): void {
        const currentData = changes.data.currentValue;
        const oldData = changes.data.previousValue;
        if (!isEqual(currentData, oldData)) {
            this.updateKeys(currentData);
        }
    }

    onFlightWatchClick(flightWatch: FlightWatch) {
        this.markerClicked.emit(flightWatch);
    }

    boundsChanged() {
        this.updateKeys(this.data);
    }

    updateKeys(data: { [oid: number]: FlightWatch }) {
        let newKeys = keys(data);

        if (this.map) {
            const bound = this.map.getBounds();
            newKeys = newKeys.filter(id => {
                const flightWatch = data[Number(id)];
                const flightWatchPosition = new google.maps.LatLng(flightWatch.latitude, flightWatch.longitude);
                return bound.contains(flightWatchPosition);
            });
        }

        this.keys = newKeys.splice(0, 200);
    }
}
