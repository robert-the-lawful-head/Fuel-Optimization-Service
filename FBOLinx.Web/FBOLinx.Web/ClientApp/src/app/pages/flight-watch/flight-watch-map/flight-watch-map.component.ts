import { Component, OnDestroy, OnInit } from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { keyBy, keys } from 'lodash';
import { AirportWatchService } from '../../../services/airportwatch.service';
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
    };

    mapLoadSubscription: Subscription;

    constructor(
        private airportWatchService: AirportWatchService,
        private sharedService: SharedService
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.mapLoadSubscription = interval(5000).subscribe(() => this.loadAirportWatchData());
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
                    if (!this.lat || !this.lng) {
                        this.lat = data[data.length - 1]?.latitude || 0;
                        this.lng = data[data.length - 1]?.longitude || 0;
                    }

                    this.airportWatchMap = keyBy(data, row => row.oid);

                    this.loading = false;
                });
        }
    }

    mapReady() {
        this.mapInitialized = true;
    }
}
