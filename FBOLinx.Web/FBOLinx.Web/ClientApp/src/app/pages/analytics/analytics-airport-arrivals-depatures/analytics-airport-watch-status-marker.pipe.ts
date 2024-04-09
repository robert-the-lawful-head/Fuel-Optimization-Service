import { Pipe, PipeTransform } from '@angular/core';
import { FlightWatchStatus } from 'src/app/models/flight-watch-historical';

@Pipe({
  name: 'airportWatchStatus'
})
export class AnalyticsAirportWatchStatusMarkerPipe implements PipeTransform {
  transform(status: FlightWatchStatus, args?: any): string {
    if (status === FlightWatchStatus.Landing) {
      return 'Arrival';
    }
    return 'Departure';
  }
};
