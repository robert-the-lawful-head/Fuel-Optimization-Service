import { Pipe, PipeTransform } from '@angular/core';
import { FlightWatch } from '../../../models/flight-watch';
import { AircraftIcons } from './aircraft-icons';

const defaultSquareSize = 22;

@Pipe({
  name: 'markerIcon'
})
export class FlightWatchMapMarkerIconPipe implements PipeTransform {
  transform(flightWatch: FlightWatch, args?: any): google.maps.Symbol {
        const markerIcon = AircraftIcons[flightWatch.aircraftTypeCode] || AircraftIcons.default;
        return {
            ...markerIcon,
            rotation: flightWatch.trackingDegree,
            anchor: new google.maps.Point(defaultSquareSize / 2 * markerIcon.scale, defaultSquareSize / 2 * markerIcon.scale)
        };
  }
};
