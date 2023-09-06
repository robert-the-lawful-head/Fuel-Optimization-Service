import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { SharedService } from 'src/app/layouts/shared-service';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import * as SharedEvents from 'src/app/models/sharedEvents';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'app-incoming-favorite-aircraft-info',
    templateUrl: './incoming-favorite-aircraft-info.component.html',
    styleUrls: ['./incoming-favorite-aircraft-info.component.scss'],
})
export class IncomingFavoriteAircraftInfoComponent implements OnInit {
    notifications: FlightWatchModelResponse[] = [];
    clickedAircraft: FlightWatchModelResponse;
    flightWatchUrl: string = '/default-layout/flight-watch';
    notificationTimeOut: number = 15000;

    constructor(private router: Router, private sharedService: SharedService) {
        this.router.events
            .pipe(filter((event) => event instanceof NavigationEnd))
            .subscribe((event: NavigationEnd) => {
                if (event.url != this.flightWatchUrl) return;
                this.sharedService.valueChange({
                    event: SharedEvents.flyToOnMapEvent,
                    data: this.clickedAircraft,
                });
            });
    }

    ngOnInit() {}
    pushCustomNotification(aircraftInfo: FlightWatchModelResponse) {
        this.notifications.unshift(aircraftInfo);

        setTimeout(() => {
            this.removeNotification(aircraftInfo);
        }, this.notificationTimeOut);
    }
    removeNotification(aircraftInfo: FlightWatchModelResponse) {
        const index = this.notifications.indexOf(aircraftInfo);
        if (index !== -1) this.notifications.splice(index, 1);
    }

    goToFlightWatch(flightwatch: FlightWatchModelResponse): void {
        this.clickedAircraft = flightwatch;
        this.router.navigate(['/default-layout/flight-watch']);
        this.sharedService.valueChange({
            event: SharedEvents.flyToOnMapEvent,
            data: this.clickedAircraft,
        });
    }
}
