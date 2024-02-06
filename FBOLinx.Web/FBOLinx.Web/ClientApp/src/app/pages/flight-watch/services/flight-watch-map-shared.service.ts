import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { FlightWatchService } from 'src/app/services/flightwatch.service';

@Injectable({
  providedIn: 'root'
})
export class FlightWatchMapSharedService {
    initialData : FlightWatchModelResponse =  null;

    constructor(private flightWatchService: FlightWatchService) { }

    private sharedDataSubject = new BehaviorSubject<FlightWatchModelResponse>(this.initialData);
    sharedData$ = this.sharedDataSubject.asObservable();

    getAndUpdateAircraftWithHistorical(fboId: number, icao: string, data: any) {
        let id = data.airportWatchLiveDataId ?? data.swimFlightLegId;
        this.flightWatchService.getAircarftLiveDataHistorical(fboId, icao, id).subscribe(response => {
            this.sharedDataSubject.next(response.result);
        });
    }
}
