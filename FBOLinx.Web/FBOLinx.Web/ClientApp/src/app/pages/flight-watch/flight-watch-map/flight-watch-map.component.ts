import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FlightWatch } from '../../../models/flight-watch';

@Component({
    selector: 'app-flight-watch-map',
    templateUrl: './flight-watch-map.component.html',
    styleUrls: [ './flight-watch-map.component.scss' ],
})
export class FlightWatchMapComponent {
    @ViewChild('flightWatchMap') flightWatchMap: google.maps.Map;
    @Input() center: google.maps.LatLngLiteral;
    @Input() flightWatchData: {
        [oid: number]: FlightWatch;
    };
    @Input() flightWatchIds: number[];
    @Output() flightWatchClicked = new EventEmitter<FlightWatch>();

    // Map Options
    zoom = 8;
    markerImg = '/assets/img/airportMarker.png';
    markerIcons = {
        A0: '/assets/img/map-markers/A0.png',
        A1: '/assets/img/map-markers/A1.png',
        A2: '/assets/img/map-markers/A2.png',
        A3: '/assets/img/map-markers/A3.png',
        A4: '/assets/img/map-markers/A4_A5.png',
        A5: '/assets/img/map-markers/A4_A5.png',
        A6: '/assets/img/map-markers/A6.png',
        A7: '/assets/img/map-markers/A7.png',
        B0: '/assets/img/map-markers/B0.png',
        B3: '/assets/img/map-markers/B3.png',
        default: '/assets/img/map-markers/default.png',
    };
    bounds = new google.maps.LatLngBounds(
        new google.maps.LatLng(85, -180),
        new google.maps.LatLng(-85, 180)
    );

    constructor() {
    }

    onFlightWatchClick(flightWatch: FlightWatch) {
        this.flightWatchClicked.emit(flightWatch);
    }
}
