import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Aircraftwatch, FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { FlightWatchService } from 'src/app/services/flightwatch.service';

@Injectable({
  providedIn: 'root'
})
export class FlightWatchMapSharedService {
    constructor(private flightWatchService: FlightWatchService) { }

    private aircraftDetailsSubject = new Subject<FlightWatchModelResponse>();
    aicraftDetails$ = this.aircraftDetailsSubject.asObservable();

    private aicraftCompanyAssignSubject = new Subject<Aircraftwatch>();
    aicraftCompanyAssign$ = this.aicraftCompanyAssignSubject.asObservable();

    getAndUpdateAircraftWithHistorical(fboId: number, icao: string, data: any) {
        let id = data.airportWatchLiveDataId ?? data.swimFlightLegId;
        this.flightWatchService.getAircarftLiveDataHistorical(fboId, icao, id).subscribe(response => {
            this.aircraftDetailsSubject.next(response);
        });
    }
    updateCustomerAicraftData(data: Aircraftwatch) : void{
        this.aicraftCompanyAssignSubject.next(data);
    }
}
