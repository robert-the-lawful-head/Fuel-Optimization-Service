import { Component, OnDestroy, OnInit } from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { keyBy, keys } from 'lodash';
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
    selector: 'app-flight-watch-map',
    templateUrl: './flight-watch-map.component.html',
    styleUrls: [ './flight-watch-map.component.scss' ],
})
export class FlightWatchMapComponent implements OnInit, OnDestroy {
    pageTitle = 'Analytics';
    breadcrumb: any[] = BREADCRUMBS;

    // Map Options
    lat: number;
    lng: number;
    zoom = 15;
    markerImg = '/assets/img/airportMarker.png';

    mapInitialized = false;
    loading = false;
    airportWatchMap: {
        [oid: number]: FlightWatch;
    } = {};

    mapLoadSubscription: Subscription;

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
                this.lat = data.latitude;
                this.lng = data.longitude;

                this.mapLoadSubscription = interval(1000).subscribe(() => this.loadAirportWatchData());
            });
    }

    ngOnDestroy() {
        if (this.mapLoadSubscription) {
            this.mapLoadSubscription.unsubscribe();
        }
    }

    get airportWatchData() {
        return keys(this.airportWatchMap);
    }

    loadAirportWatchData() {
        if (!this.loading) {
            this.loading = true;
            this.airportWatchService.getAll()
                .subscribe((data: FlightWatch[]) => {
                    this.airportWatchMap = keyBy(data, row => row.oid);

                    this.loading = false;
                });
        }
    }

    mapReady() {
        this.mapInitialized = true;
    }
}
