import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { SharedService } from 'src/app/layouts/shared-service';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import * as SharedEvents from 'src/app/models/sharedEvents';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-incoming-favorite-aircraft-info',
    templateUrl: './incoming-favorite-aircraft-info.component.html',
    styleUrls: ['./incoming-favorite-aircraft-info.component.scss'],
})
export class IncomingFavoriteAircraftInfoComponent implements OnInit {
    notifications: FlightWatchModelResponse[] = [];
    clickedAircraft: FlightWatchModelResponse = null;
    flightWatchUrl: string = '/default-layout/flight-watch';
    notificationTimeOut: number = 15000;

    subscription: Subscription;

    constructor(private router: Router, private sharedService: SharedService) {
        this.subscription = this.router.events
            .pipe(filter((event) => event instanceof NavigationEnd))
            .subscribe((event: NavigationEnd) => {
                if (this.clickedAircraft == null) return;
                this.flyToNotificationEventChange();
            });
    }

    ngOnInit() {}
    ngOnDestroy(): void {
        if(this.subscription)
            this.subscription.unsubscribe();
    }
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
        this.removeNotification(flightwatch);
        if(this.router.url != this.flightWatchUrl)
            this.router.navigate(['/default-layout/flight-watch']);
        else
            this.flyToNotificationEventChange();
    }
    private flyToNotificationEventChange(): void {
        this.sharedService.valueChange({
            event: SharedEvents.flyToOnMapEvent,
            data: this.clickedAircraft,
        });
        this.clickedAircraft = null;
    }
}
