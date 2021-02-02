import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { interval, Subscription } from 'rxjs';
import { keyBy, map, values } from 'lodash';
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
    flightWatchData: FlightWatch[];
    filteredFlightWatchData: {
        [oid: number]: FlightWatch;
    };
    filteredFlightWatchIds: number[];
    filter: string;
    center: google.maps.LatLngLiteral;
    selectedFlightWatch: FlightWatch;
    flightWatchDataSource: MatTableDataSource<FlightWatch>;

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
                    this.flightWatchData = data;
                    this.setFilteredFlightWatchData();
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

    onFilterChanged(filter: string) {
        this.filter = filter;
        this.setFilteredFlightWatchData();
    }

    setFilteredFlightWatchData() {
        if (!this.filter) {
            this.filteredFlightWatchData = keyBy(this.flightWatchData, fw => fw.oid);
            this.filteredFlightWatchIds = map(this.flightWatchData, fw => fw.oid);
            this.flightWatchDataSource = new MatTableDataSource(this.flightWatchData);
        } else {
            const loweredFilter = this.filter.toLowerCase();
            const filteredFlightWatches = values(this.flightWatchData).filter(fw => fw.aircraftHexCode.toLowerCase().includes(loweredFilter)
                || fw.aircraftTypeCode.toLowerCase().includes(loweredFilter)
                || fw.atcFlightNumber.toLowerCase().includes(loweredFilter)
            );
            const filteredResult = keyBy(
                values(this.flightWatchData).filter(fw => fw.aircraftHexCode.toLowerCase().includes(loweredFilter)
                    || fw.aircraftTypeCode.toLowerCase().includes(loweredFilter)
                    || fw.atcFlightNumber.toLowerCase().includes(loweredFilter)
                ),
                (fw => fw.oid)
            );

            this.filteredFlightWatchData = {
                ...filteredResult
            };

            this.filteredFlightWatchIds = map(filteredFlightWatches, fw => fw.oid);
            this.flightWatchDataSource = new MatTableDataSource(filteredFlightWatches);
        }
    }
}
