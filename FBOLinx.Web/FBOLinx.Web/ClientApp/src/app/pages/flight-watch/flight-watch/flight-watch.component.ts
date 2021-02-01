import { Component, OnDestroy, OnInit } from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { keyBy } from 'lodash';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { FbosService } from '../../../services/fbos.service';
import { SharedService } from '../../../layouts/shared-service';
import { FlightWatch } from '../../../models/flight-watch';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '/default-layout',
    },
    {
        title: 'Flight Watch',
        link: '/default-layout/flight-watch',
    },
];

@Component({
    selector: 'app-flight-watch',
    templateUrl: './flight-watch.component.html',
    styleUrls: [ './flight-watch.component.scss' ],
})
export class FlightWatchComponent implements OnInit, OnDestroy {
    pageTitle = 'Flight Watch';
    breadcrumb: any[] = BREADCRUMBS;

    loading = false;
    mapLoadSubscription: Subscription;
    flightWatchData: {
        [oid: number]: FlightWatch;
    };
    center: google.maps.LatLngLiteral;
    selectedFlightWatch: FlightWatch;

    constructor(
        private airportWatchService: AirportWatchService,
        private fbosService: FbosService,
        private sharedService: SharedService
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.fbosService.getLocation(this.sharedService.currentUser.fboId)
            .subscribe((data: {
                latitude: number;
                longitude: number;
            }) => {
                this.center = { lat: data.latitude, lng: data.longitude };

                this.mapLoadSubscription = interval(3000).subscribe(() => this.loadAirportWatchData());
            });
    }

    ngOnDestroy() {
        if (this.mapLoadSubscription) {
            this.mapLoadSubscription.unsubscribe();
        }
    }

    loadAirportWatchData() {
        if (!this.loading) {
            this.loading = true;
            this.airportWatchService.getAll()
                .subscribe((data: FlightWatch[]) => {
                    this.flightWatchData = keyBy(data, row => row.oid);

                    this.loading = false;
                });
        }
    }

    onFlightWatchClick(flightWatch: FlightWatch) {
        if (this.selectedFlightWatch?.oid === flightWatch.oid) {
            this.selectedFlightWatch = undefined;
        } else {
            this.selectedFlightWatch = flightWatch;
        }
    }

    onAircraftInfoClose() {
        this.selectedFlightWatch = undefined;
    }
}
