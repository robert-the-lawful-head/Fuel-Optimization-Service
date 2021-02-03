import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { interval, Subscription } from 'rxjs';
import { isEmpty, keyBy } from 'lodash';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { SharedService } from '../../../layouts/shared-service';
import { FlightWatch } from '../../../models/flight-watch';
import { ResizeEvent } from 'angular-resizable-element';

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
    filter: string;
    center: google.maps.LatLngLiteral;
    selectedFlightWatch: FlightWatch;
    flightWatchDataSource: MatTableDataSource<FlightWatch>;

    style: any = {};

    constructor(
        private airportWatchService: AirportWatchService,
        private sharedService: SharedService
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {
        this.mapLoadSubscription = interval(3000).subscribe(() => this.loadAirportWatchData());
    }

    ngOnDestroy() {
        if (this.mapLoadSubscription) {
            this.mapLoadSubscription.unsubscribe();
        }
    }

    loadAirportWatchData() {
        if (!this.loading) {
            this.loading = true;
            this.airportWatchService.getAll(this.sharedService.currentUser.fboId)
                .subscribe((data: any) => {
                    if (!this.center) {
                        this.center = { lat: data.fboLocation.latitude, lng: data.fboLocation.longitude };
                    }
                    this.flightWatchData = data.flightWatchData;
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
        let originalData: FlightWatch[];
        if (!this.filter) {
            originalData = this.flightWatchData;
        } else {
            const loweredFilter = this.filter.toLowerCase();
            originalData = this.flightWatchData.filter(fw => fw.aircraftHexCode.toLowerCase().includes(loweredFilter)
            || fw.aircraftTypeCode.toLowerCase().includes(loweredFilter)
            || fw.atcFlightNumber.toLowerCase().includes(loweredFilter));
        }

        this.filteredFlightWatchData = keyBy(originalData, fw => fw.oid);
        if (!this.flightWatchDataSource) {
            this.flightWatchDataSource = new MatTableDataSource(originalData);
        } else {
            this.flightWatchDataSource.data = originalData;
        }
    }

    validate(event: ResizeEvent): boolean {
        const MAX_DIMENSIONS_PX = 800;
        const MIN_DIMENSIONS_PX = 0;
        if (
          event.rectangle.width &&
          (event.rectangle.width > MAX_DIMENSIONS_PX ||
            event.rectangle.width < MIN_DIMENSIONS_PX)
        ) {
          return false;
        }
        return true;
    }

    onResizeEnd(event: ResizeEvent): void {
        this.style = {
            position: 'fixed',
            left: `${event.rectangle.left}px`,
            right: `${event.rectangle.right}px`,
            width: `${event.rectangle.width}px`,
            height: `${event.rectangle.height}px`,
        };
    }

    get mapWidth() {
        if (isEmpty(this.style)) {
            return 'calc(100% - 400px)';
        }
        return `calc(100% - ${this.style.width})`;
    }
}
